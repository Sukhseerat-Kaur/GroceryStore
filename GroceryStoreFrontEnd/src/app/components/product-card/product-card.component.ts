import { Component, Input } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductService } from 'src/app/services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css'],
})
export class ProductCardComponent {
  @Input() product!: Product;

  constructor(private productService: ProductService, private router: Router) {}

  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
  }

  goToProductPage() {
    this.router.navigate(['/product/' + this.product.productId]);
  }
}
