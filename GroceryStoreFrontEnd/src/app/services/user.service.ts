import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { Subject, of } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  loggedIn: boolean = false;
  admin: boolean = false;
  superAdmin: boolean = false;
  private _updateMenu = new Subject<void>();
  get updateMenu() {
    return this._updateMenu;
  }
  constructor(private http: HttpClient, private authService: AuthService) {}

  login(user: any) {
    var email = user.userEmail;
    var password = user.password;
    console.log(email, password);
    return this.authService.login(user);
  }

  logout() {
    this.loggedIn = false;
    return this.authService.logout();
  }

  registerUser(user: any) {
    console.log(user);
    var response = this.http.post<any>(
      environment.baseApiUrl + 'user/register',
      user
    );
    return response;
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  setAdmin(adminValue: boolean) {
    this.admin = adminValue;
  }

  setlogin(loginValue: boolean) {
    this.loggedIn = loginValue;
  }

  get isAdmin() {
    return (localStorage.getItem('userRole') as string) === 'admin';
  }

  get isSuperAdmin() {
    return (localStorage.getItem('userRole') as string) === 'superAdmin';
  }
}
