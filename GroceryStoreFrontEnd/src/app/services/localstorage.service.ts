import { Injectable } from '@angular/core';
import { User } from '../models/User';
import { Roles } from 'src/assets/Roles';

@Injectable({
  providedIn: 'root',
})
export class LocalstorageService {
  constructor() {}

  storeUser(user: User) {
    console.log(user);
    var keys = Object.keys(user) as Array<keyof User>;
    console.log(keys);
    keys.map((key) => {
      if (key != 'password') this.storeLoginInfo(key, user[key].toString());
    });
  }
  storeLoginInfo(key: string, value: string) {
    localStorage.setItem(key, value);
  }

  RemoveLoginInfo() {
    var keys: Array<keyof User> = [
      'phoneNumber',
      'userEmail',
      'userId',
      'userName',
      'userRole',
    ];
    keys.map((key) => {
      localStorage.removeItem(key);
    });
  }

  getCurrentUserId(): number | null {
    var userId = localStorage.getItem('userId');
    if (userId == null) {
      return null;
    }
    return parseInt(userId);
  }

  getRole(): string {
    return localStorage.getItem('userRole') as string;
  }

  getCurrentUserName() {
    var userName = localStorage.getItem('userName');
    if (userName == null) {
      return null;
    }
    return userName;
  }

  getCurrentUserEmail() {
    var userEmail = localStorage.getItem('userEmail');
    if (userEmail == null) {
      return null;
    }
    return userEmail;
  }
}
