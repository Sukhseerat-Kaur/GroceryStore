import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/User';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { Roles } from 'src/assets/Roles';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css'],
})
export class UserLoginComponent implements OnInit {
  userLoginForm!: FormGroup;
  user!: User;
  loggedIn: boolean = false;
  role: string = '';
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private storageService: LocalstorageService,
    private router: Router,
    private toastService: ToastrService
  ) {}
  ngOnInit(): void {
    this.userLoginForm = this.fb.group({
      userEmail: ['', Validators.required],
      password: ['', Validators.required],
    });
    this.userService.updateMenu.subscribe((response) => {
      this.userLoginForm.reset();
    });
  }

  onSubmit() {
    var email = this.userLoginForm.controls['userEmail'].value;
    var password = this.userLoginForm.controls['password'].value;
    console.log(email, password);
    this.userService
      .login({
        userEmail: email,
        password: password,
      })
      .subscribe(
        (response) => {
          console.log(response);
          this.user = response.user;
          console.log(this.user, response);
          this.storageService.storeUser(this.user);
          this.userService.setlogin(true);
          if (this.storageService.getRole() === 'admin') {
            this.userService.setAdmin(true);
            console.log('isAdmin', this.userService.isAdmin);
          }
          this.userService.updateMenu.next();
          this.userLoginForm.reset();
          this.router.navigate(['/dashboard']);
          this.showSuccessToast('Success', 'Login Successful');
        },
        (err) => {
          this.showErrorToast('Login Failed', 'Invalid email or password');
          console.log(err.error.message);
        }
      );
  }

  showSuccessToast(title: string, message: string) {
    this.toastService.success(message, title, {
      closeButton: true,
      progressBar: true,
    });
  }

  showErrorToast(title: string, message: string) {
    this.toastService.error(message, title, {
      closeButton: true,
      progressBar: true,
    });
  }
}
