import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/models/Product';
import { ProductService } from 'src/app/services/product.service';
import { CartService } from 'src/app/services/cart.service';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { ReviewService } from 'src/app/services/review.service';
import {
  faStar,
  faCircleMinus,
  faEdit,
  faTrash,
} from '@fortawesome/free-solid-svg-icons';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent implements OnInit {
  faStar = faStar;
  faCircleMinus = faCircleMinus;
  faEdit = faEdit;
  faTrash = faTrash;
  productId: number = 0;
  isLoading: boolean = true;
  productData!: Product;
  productImageURL!: string;
  quantityInputValue: string = '1';
  maxInput: string = '';
  userId!: number;
  isAdmin!: boolean;
  isSuperAdmin!: boolean;
  reviews: any[] = [];
  reviewInput: string = '';
  userName: string = '';
  userEmail: string = '';
  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private localStorageService: LocalstorageService,
    private userService: UserService,
    private router: Router,
    private reviewService: ReviewService,
    private toastService: ToastrService
  ) {}

  ngOnInit(): void {
    this.userId = this.localStorageService.getCurrentUserId() as number;
    this.userName = this.localStorageService.getCurrentUserName() as string;
    this.userEmail = this.localStorageService.getCurrentUserEmail() as string;

    this.isAdmin = this.userService.isAdmin;
    this.isSuperAdmin = this.userService.isSuperAdmin;
    let productIdString = this.route.snapshot.paramMap.get('productId');
    this.productId = productIdString !== null ? parseInt(productIdString) : 0;
    this.productService.getProductById(this.productId).subscribe(
      (response) => {
        this.productData = { ...response };
        this.maxInput = this.productData.productQuantity.toString();
        console.log(this.productData);
        this.productImageURL = this.productService.getImageUrl(
          this.productData.productId
        );
        this.getReviews();
      },
      (err) => {
        this.showErrorToast('Error', '');
        console.log(err);
      },
      () => {
        this.isLoading = false;
      }
    );
    this.reviewService.ReviewsUpdated.subscribe(() => {
      this.getReviews();
    });
  }

  getReviews() {
    this.reviewService.getReviews(this.productId).subscribe((response) => {
      this.reviews = [...response];
      console.log(this.reviews);
    });
  }
  getImageUrl() {
    return this.productImageURL;
  }

  checkQuantity() {
    var quantity = parseInt(this.quantityInputValue);
    if (quantity > this.productData.productQuantity) {
      this.showInfoToast(
        'Info',
        `Only ${this.productData.productQuantity} pieces available`
      );
    } else if (quantity <= 0) {
      this.showErrorToast('Error', 'Select a valid quantity');
    }
  }

  disableAddToCart() {
    var quantity = parseInt(this.quantityInputValue);
    if (quantity > this.productData.productQuantity || quantity <= 0) {
      return true;
    }
    return false;
  }

  goToEditPage() {
    this.router.navigate([`/product/edit/${this.productId}`]);
  }

  deleteProduct() {
    this.productService.deleteProduct(this.productId).subscribe(
      (response) => {
        this.router.navigate(['/dashboard']);
        this.showSuccessToast('Success', 'Product Deleted');
      },
      (err) => {
        this.showErrorToast('Error', "Coudn't delete product");
        console.log(err);
      }
    );
  }

  isOutOfStock() {
    return this.productData.productQuantity <= 0;
  }

  postReview() {
    console.log(this.reviewInput);
    this.reviewService
      .addReview(this.userId, this.productId, this.reviewInput)
      .subscribe(
        (response) => {
          this.showSuccessToast('Success', 'Review added');
          this.reviewService.ReviewsUpdated.next();
        },
        (err) => {
          this.showErrorToast('Error', '');
          console.log(err);
        },
        () => {
          this.reviewInput = '';
        }
      );
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  deleteReview(reviewerUserId: number, productId: number, time: string) {
    this.reviewService
      .deleteComment(this.userId, reviewerUserId, productId, time)
      .subscribe(
        (response) => {
          this.showSuccessToast('Success', 'Review deleted');
          this.reviewService.ReviewsUpdated.next();
        },
        (err) => {
          this.showErrorToast('Error', "Product can't be deleted");
          console.log(err);
        }
      );
  }
  addToCart() {
    var quantity = parseInt(this.quantityInputValue);
    if (quantity > this.productData.productQuantity) {
      this.showInfoToast(
        'Info',
        `Only ${this.productData.productQuantity} pieces available`
      );
      return;
    } else if (quantity <= 0) {
      this.showInfoToast('Info', 'Select valid quantity');
      return;
    }
    console.log(this.userId, this.productData.productId, quantity);
    this.cartService
      .addToCart(this.userId, this.productData.productId, quantity)
      .subscribe(
        (response) => {
          this.showSuccessToast('Success', 'Added to cart');
        },
        (err) => {
          this.showErrorToast('Error', err.error.error);
          console.log(err);
        }
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
