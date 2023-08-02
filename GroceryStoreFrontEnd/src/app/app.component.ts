import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from './services/user.service';
import { LocalstorageService } from './services/localstorage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'GroceryStoreFrontEnd';
  loggedIn: boolean = false;
  isAdmin: boolean = false;
  isSuperAdmin: boolean = false;
  userName: string = '';
  isNavbarCollapsed: boolean = true;
  constructor(
    private router: Router,
    private userService: UserService,
    private storageService: LocalstorageService
  ) {}

  ngOnInit(): void {
    if (this.storageService.getCurrentUserId() != null) {
      this.userService.loggedIn = true;
      this.menuDisplay();
    }
    this.userService.updateMenu.subscribe((res) => {
      this.menuDisplay();
    });
    this.menuDisplay();
  }

  menuDisplay() {
    this.loggedIn = this.userService.isLoggedIn();
    this.isAdmin = this.userService.isAdmin;
    this.isSuperAdmin = this.userService.isSuperAdmin;
    this.userName = localStorage.getItem('userName') as string;
  }

  logOut() {
    this.userService.logout().subscribe((response) => {
      console.log(response);
      this.storageService.RemoveLoginInfo();
      this.userService.updateMenu.next();
      this.router.navigate(['/login']);
    });
  }
}
