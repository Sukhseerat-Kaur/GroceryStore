import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpStatusCode,
} from '@angular/common/http';
import { environment } from 'environments/environment';
import { Observable, of, tap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient, private authSerivce: AuthService) {}

  addProduct(product: any) {
    var token = this.authSerivce.getToken();
    var headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.post<any>(
      environment.baseApiUrl + 'product/add',
      product,
      {
        headers: headers,
      }
    );
  }

  getAllProducts() {
    // var token = this.authSerivce.getToken();
    // var header = new HttpHeaders({
    //   Authorization: `Bearer ${token}`,
    // });
    // console.log(header);
    // if(token){
    return this.http.get<any>(
      environment.baseApiUrl + 'product/available-products'
    );
    // }
    // else
    //   return throwError({
    //     message: "UnAuthorized Access",
    //     error: HttpStatusCode.Unauthorized
    //   })
  }

  getImageUrl(id: number) {
    return `${environment.baseImageApiUrl}${id}`;
  }

  getProductById(productId: number) {
    let queryParams = new HttpParams().append('productId', productId);
    return this.http.get<any>(environment.baseApiUrl + 'product/get-product', {
      params: queryParams,
    });
  }

  getImage(id: number) {
    var imageUrl = this.getImageUrl(id);
    return this.http.get(imageUrl, { responseType: 'blob' });
  }

  updateProduct(product: any) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authSerivce.getToken()}`,
    });
    return this.http.put<any>(
      environment.baseApiUrl + 'product/edit',
      product,
      { headers }
    );
  }

  getProductQuantity(productId: number) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authSerivce.getToken()}`,
    });
    var queryParams = new HttpParams().append('productId', productId);
    return this.http.get<number>(
      environment.baseApiUrl + 'product/available-stock',
      {
        headers,
        params: queryParams,
      }
    );
  }

  deleteProduct(productId: number) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authSerivce.getToken()}`,
    });
    var queryParams = new HttpParams().append('productId', productId);
    return this.http.delete<any>(environment.baseApiUrl + 'product/delete', {
      params: queryParams,
      headers: headers,
    });
  }
}
