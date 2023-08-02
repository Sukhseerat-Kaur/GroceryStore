import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { AuthService } from './auth.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SuperAdminService {
  updateAdminList = new Subject<void>();
  constructor(private http: HttpClient, private authService: AuthService) {}

  addAdmin(user: any) {
    console.log(user);
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    var response = this.http.post<any>(
      environment.baseApiUrl + 'user/add-admin',
      user,
      { headers: header }
    );
    return response;
  }

  getAllAdmins() {
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<any>(environment.baseApiUrl + 'user/get-admins', {
      headers: header,
    });
  }

  removeAdmin(userId: number) {
    var token = this.authService.getToken();
    var header = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    console.log(token);
    var queryParams = new HttpParams().append('userId', userId);
    console.log(userId);
    return this.http.delete<any>(environment.baseApiUrl + 'user/remove-admin', {
      params: queryParams,
      headers: header,
    });
  }

  get AdminListUpdated() {
    return this.updateAdminList;
  }
}
