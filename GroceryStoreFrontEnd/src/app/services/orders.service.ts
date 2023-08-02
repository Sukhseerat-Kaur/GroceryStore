import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'environments/environment';
import { LocalstorageService } from './localstorage.service';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  constructor(
    private http: HttpClient,
    private storageService: LocalstorageService,
    private router: Router,
    private authService: AuthService
  ) {}

  placeOrder(orderItems: any[]) {
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    console.log('from service', orderItems);
    return this.http.post<any>(
      environment.baseApiUrl + 'order/place-order',
      orderItems,
      { headers: header }
    );
  }

  getOrders() {
    var userId = this.storageService.getCurrentUserId();
    if (userId === null) {
      this.router.navigate(['/not-authorized']);
    }
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<any>(
      environment.baseApiUrl + `order/get-orders/${userId}`,
      { headers: header }
    );
  }

  getMostOrderedProducts(year: number, month: number, quantity: number) {
    let queryParams = new HttpParams()
      .append('year', year)
      .append('month', month)
      .append('quantity', quantity);
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<any>(environment.baseApiUrl + 'order/most-ordered', {
      params: queryParams,
      headers: header,
    });
  }
}
