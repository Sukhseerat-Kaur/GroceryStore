import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getCart(userId: number) {
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    let queryParams = new HttpParams().append('userId', userId);
    return this.http.get<any>(environment.baseApiUrl + 'cart/get-cart', {
      params: queryParams,
      headers: header,
    });
  }

  removeFromCart(userId: number, productId: number) {
    let queryParams = new HttpParams()
      .append('userId', userId)
      .append('productId', productId);
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.delete<any>(
      environment.baseApiUrl + 'cart/remove-from-cart',
      { params: queryParams, headers: header }
    );
  }

  updateProductQuantityInCart(
    userId: number,
    productId: number,
    newQuantity: number
  ) {
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post<any>(
      environment.baseApiUrl + 'cart/update',
      {
        userId,
        productId,
        quantity: newQuantity,
      },
      { headers: header }
    );
  }

  addToCart(userId: number, productId: number, quantity: number) {
    var body = {
      userId,
      productId,
      quantity,
    };
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post<any>(
      environment.baseApiUrl + 'cart/add-to-cart',
      body,
      { headers: header }
    );
  }
}
