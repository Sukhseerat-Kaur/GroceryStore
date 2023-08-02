import { Injectable, inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Subject, of, tap } from 'rxjs';
import { User } from '../models/User';
import { UserService } from './user.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLogin = false;
  loggedInUser!: User;
  private _updateMenu = new Subject<void>();

  get updateMenu() {
    return this._updateMenu;
  }
  constructor(private router: Router, private http: HttpClient) {}

  login(user: any) {
    var email = user.userEmail;
    var password = user.password;
    console.log(email, password);
    return this.http
      .post<any>(environment.baseApiUrl + 'user/authenticate', {
        UserEmail: email,
        Password: password,
      })
      .pipe(
        tap((response) => {
          const token = response.token;
          localStorage.setItem('token', token);
        })
      );
  }

  logout() {
    this.isLogin = false;
    localStorage.removeItem('token');
    return of({ success: this.isLogin, role: '' });
  }

  getToken(): string {
    return localStorage.getItem('token') as string;
  }

  isLoggedIn() {
    const loggedIn = localStorage.getItem('loggedIn');
    if (loggedIn == 'true') this.isLogin = true;
    else this.isLogin = false;
    return this.isLogin;
  }

  getRole() {
    this.loggedInUser.userRole = localStorage.getItem('role') as string;
    return this.loggedInUser.userRole;
  }

  CanActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let url: string = state.url;
    return this.checkUserLogin(next, url);
  }

  checkUserLogin(route: ActivatedRouteSnapshot, url: any): boolean {
    if (this.isLoggedIn()) {
      const userRole = this.getRole();
      if (route.data['role'] && route.data['role'].indexOf(userRole) === -1) {
        this.router.navigate(['/login']);
        return false;
      }
      return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
}
