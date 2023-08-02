import { Component, OnInit } from '@angular/core';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { CartService } from 'src/app/services/cart.service';
import { ProductService } from 'src/app/services/product.service';
import { OrdersService } from 'src/app/services/orders.service';
import { Router } from '@angular/router';
import { Observable, concatMap, filter, from, map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  userId!: number;
  cartProducts: any[] = [];
  quantityValues: any = {};
  grandTotal: number = 0;
  priceWithoutDiscount: number = 0;
  availableProductsInCart: any[] = [];
  isLoading: boolean = true;
  newCart: any[] = [];
  constructor(
    private cartService: CartService,
    private localStorageService: LocalstorageService,
    private productService: ProductService,
    private ordersService: OrdersService,
    private router: Router,
    private toastService: ToastrService
  ) {}

  ngOnInit(): void {
    this.userId = this.localStorageService.getCurrentUserId() as number;
    console.log(this.userId);
    this.getValidCartProducts().subscribe(
      (result) => {
        this.newCart.push(result);
      },
      (err) => {
        this.showErrorToast('Error', err.error.message);
      },
      () => {
        this.availableProductsInCart = this.newCart;
        console.log(this.availableProductsInCart);
        this.newCart = [];
        this.grandTotal = this.getTotalCartPrice();
        this.priceWithoutDiscount = this.getPriceWithoutDiscount();
        this.isLoading = false;
      }
    );
  }
  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
  }

  removeFromCart(productId: number) {
    this.cartService.removeFromCart(this.userId, productId).subscribe(
      (response) => {
        this.getValidCartProducts().subscribe(
          (result) => {
            this.newCart.push(result);
          },
          (err) => {},
          () => {
            console.log(this.availableProductsInCart);
            this.availableProductsInCart = this.newCart;
            this.newCart = [];
            this.grandTotal = this.getTotalCartPrice();
            this.priceWithoutDiscount = this.getPriceWithoutDiscount();
          }
        );
        this.showSuccessToast('Success', 'Removed from cart');
      },
      (err) => {
        this.showErrorToast('Error', err.error.message);
      }
    );
  }
  updateProductQuantity(quantity: string, productId: number) {
    var newQuantity: number = parseInt(quantity);
    var availableQuantity;
    this.productService.getProductQuantity(productId).subscribe((response) => {
      availableQuantity = response;
      if (newQuantity > availableQuantity) {
        this.showInfoToast(
          'Invalid',
          `Only ${availableQuantity} stocks available`
        );
        return;
      }
      console.log(this.userId, productId, newQuantity, availableQuantity);
      this.cartService
        .updateProductQuantityInCart(this.userId, productId, newQuantity)
        .subscribe(
          (response) => {
            this.getValidCartProducts().subscribe(
              (result) => {
                this.newCart.push(result);
              },
              (err) => {},
              () => {
                this.availableProductsInCart = this.newCart;
                this.newCart = [];
                this.grandTotal = this.getTotalCartPrice();
                this.priceWithoutDiscount = this.getPriceWithoutDiscount();
              }
            );
            this.showSuccessToast('Success', 'Product quantity updated');
          },
          (err) => {
            this.showErrorToast('Error', err.error.message[0].errorMessage);
          }
        );
    });
  }

  getTotalCartPrice() {
    var totalPrice = 0;
    this.availableProductsInCart.forEach((product) => {
      totalPrice +=
        product.productQuantityInCart *
        (product.productPrice - product.productDiscount);
    });
    console.log('total', totalPrice);
    return totalPrice;
  }

  getPriceWithoutDiscount() {
    var totalPrice = 0;
    this.availableProductsInCart.forEach((product) => {
      totalPrice += product.productQuantityInCart * product.productPrice;
    });
    console.log('total', totalPrice);
    return totalPrice;
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  placeOrder() {
    let orders: any[] = [];

    this.availableProductsInCart.forEach((cartItem) => {
      let orderItem = {
        userId: this.userId,
        productId: cartItem.productId,
        quantity: cartItem.productQuantityInCart,
        buyingPrice: cartItem.productPrice - cartItem.productDiscount,
      };
      orders.push(orderItem);
    });

    this.ordersService.placeOrder(orders).subscribe(
      (response) => {
        this.grandTotal = 0;
        this.priceWithoutDiscount = 0;
        this.showSuccessToast('Success', response.message);
        this.router.navigate(['/order-placed/' + response.orderId]);
        this.getValidCartProducts().subscribe(
          (result) => {
            this.newCart = [];
          },
          (err) => {},
          () => {
            console.log(this.availableProductsInCart);
            this.availableProductsInCart = this.newCart;
            this.newCart = [];
          }
        );
      },
      (err) => {
        this.showErrorToast('Error', "Order coudn't be placed");
        console.log(err);
      }
    );
  }

  getValidCartProducts(): Observable<any> {
    return this.cartService.getCart(this.userId).pipe(
      concatMap((response) => {
        this.cartProducts = [...response];
        return from(response).pipe(
          concatMap((product: any) => {
            return this.productService
              .getProductQuantity(product.productId)
              .pipe(
                filter((quantity) => {
                  return quantity > 0 && !product.isDeleted;
                }),
                map((quantity) => {
                  return product;
                })
              );
          })
        );
      })
    );
  }

  showSuccessToast(title: string, message: string) {
    this.toastService.success(message, title, {
      closeButton: true,
      progressBar: true,
    });
  }

  showErrorToast(title: string, message: string) {
    this.toastService.error(message, title, {
      closeButton: true,
      progressBar: true,
    });
  }

  showInfoToast(title: string, message: string) {
    this.toastService.info(message, title, {
      closeButton: true,
      progressBar: true,
    });
  }
}
