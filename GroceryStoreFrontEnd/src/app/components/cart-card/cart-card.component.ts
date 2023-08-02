import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-card',
  templateUrl: './cart-card.component.html',
  styleUrls: ['./cart-card.component.css'],
})
export class CartCardComponent implements OnInit {
  faXMark = faXmark;
  availableQuantity!: number;
  isLoading = true;
  @Input() inputValue: any;
  @Input() productData: any;
  @Output() changeQuantityInCart = new EventEmitter<string>();
  @Output() removeProductFromCart = new EventEmitter<number>();

  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    this.productService
      .getProductQuantity(this.productData.productId)
      .subscribe(
        (response) => {
          this.availableQuantity = response;
          this.isLoading = false;
        },
        (err) => {
          console.log(err);
        }
      );
  }

  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
  }

  emitChangeQuantityInCart(newQuantity: string) {
    this.changeQuantityInCart.emit(newQuantity);
  }

  emitRemoveProductFromCart() {
    this.removeProductFromCart.emit(this.productData.productId);
  }

  viewProduct() {
    this.router.navigate([`/product/${this.productData.productId}`]);
  }

  isOutOfStock() {
    return this.availableQuantity <= 0;
  }
}
