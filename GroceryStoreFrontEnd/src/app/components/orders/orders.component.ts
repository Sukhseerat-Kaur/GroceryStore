import { Component, OnInit } from '@angular/core';
import { OrdersService } from 'src/app/services/orders.service';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { Router } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';

interface OrderIdToOrderInterface {
  [key: string]: any[];
}
@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
})
export class OrdersComponent implements OnInit {
  userId!: number;
  orders: any[] = [];
  isLoading: boolean = true;
  constructor(
    private ordersService: OrdersService,
    private router: Router,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.ordersService.getOrders().subscribe(
      (response) => {
        this.orders = [...response];
        console.log(this.orders);
      },
      (err) => {},
      () => {
        this.isLoading = false;
      }
    );
  }

  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  goToProductPage(productId: number) {
    this.router.navigate([`/product/${productId}`]);
  }
}
