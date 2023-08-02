import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from './auth.service';
import { environment } from 'environments/environment';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  reviews = new Subject<void>();
  constructor(private http: HttpClient, private authService: AuthService) {}

  getReviews(productId: number) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.getToken()}`,
    });
    var queryParams = new HttpParams().append('productId', productId);
    return this.http.get<any>(environment.baseApiUrl + 'review/get-reviews', {
      params: queryParams,
      headers: headers,
    });
  }

  addReview(userId: number, productId: number, review: string) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.getToken()}`,
    });

    console.log(this.getCurrentDateinSqlFormat());
    console.log(userId, productId, review);

    var body = {
      userId,
      productId,
      time: this.getCurrentDateinSqlFormat(),
      reviewString: review,
    };
    return this.http.post<any>(
      environment.baseApiUrl + 'review/add-review',
      body,
      {
        headers: headers,
      }
    );
  }

  deleteComment(
    fromUserId: number,
    reviewerUserId: number,
    productId: number,
    time: string
  ) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.getToken()}`,
    });
    var queryParams = new HttpParams()
      .append('fromUserId', fromUserId)
      .append('userId', reviewerUserId)
      .append('productId', productId)
      .append('time', time);
    return this.http.delete<any>(
      environment.baseApiUrl + 'review/delete-review',
      { params: queryParams, headers: headers }
    );
  }

  updateReview(oldReview: any, newReview: any) {
    var headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.getToken()}`,
    });

    var queryParams = new HttpParams()
      .append('userId', oldReview.userId)
      .append('productId', oldReview.productId)
      .append('time', oldReview.time);
    return this.http.put<any>(
      environment.baseApiUrl + 'review/update-review',
      newReview,
      { params: queryParams, headers: headers }
    );
  }

  getCurrentDateinSqlFormat() {
    const now = new Date();

    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hour = String(now.getHours()).padStart(2, '0');
    const minute = String(now.getMinutes()).padStart(2, '0');
    const second = String(now.getSeconds()).padStart(2, '0');
    const millisecond = String(now.getMilliseconds()).padStart(3, '0');

    return `${year}-${month}-${day}T${hour}:${minute}:${second}.${millisecond}`;
  }

  get ReviewsUpdated() {
    return this.reviews;
  }
}
