import { Component } from '@angular/core';
import { OrdersService } from 'src/app/services/orders.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-most-ordered',
  templateUrl: './most-ordered.component.html',
  styleUrls: ['./most-ordered.component.css'],
})
export class MostOrderedComponent {
  monthAndYear: string = this.getCurrentMonthAndYear();
  mostOrderedProducts: any[] = [];
  constructor(private orderService: OrdersService, private router: Router) {
    this.fetchMostOrderedProducts();
  }

  onChange() {
    this.fetchMostOrderedProducts();
  }

  getCurrentMonthAndYear() {
    var date = new Date();
    var month = date.getMonth() + 1;
    var monthString = month.toString();
    if (monthString.length == 1) monthString = '0' + monthString;
    var year = date.getFullYear();
    return year.toString() + '-' + monthString;
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  fetchMostOrderedProducts() {
    let year: number = parseInt(this.monthAndYear.substr(0, 4));
    let month: number = parseInt(this.monthAndYear.substr(5, 2));
    this.orderService
      .getMostOrderedProducts(year, month, 5)
      .subscribe((response) => {
        this.mostOrderedProducts = [...response];
        console.log(this.mostOrderedProducts);
      });
  }
}
