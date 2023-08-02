import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';
@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
})
export class UserRegisterComponent {
  userRegistrationForm: FormGroup;
  patterRegex = new RegExp('ab+c');
  constructor(
    private fb: FormBuilder,
    private api: UserService,
    private router: Router,
    private toastService: ToastrService
  ) {
    this.userRegistrationForm = this.fb.group({
      name: ['', Validators.required],
      userEmail: [
        '',
        Validators.compose([Validators.email, Validators.required]),
      ],
      phonenumber: [
        '',
        Validators.compose([
          Validators.required,
          Validators.pattern('[- +()0-9]+'),
        ]),
      ],
      password: ['', Validators.compose([Validators.required])],
      confirmPassword: [''],
    });
    this.userRegistrationForm.addValidators(
      this.createCompareValidator(
        this.userRegistrationForm.get('password') as AbstractControl,
        this.userRegistrationForm.get('confirmPassword') as AbstractControl
      )
    );
  }

  onSubmit() {
    var user = {
      UserName: this.userRegistrationForm.get('name')?.value,
      UserEmail: this.userRegistrationForm.get('userEmail')?.value,
      PhoneNumber: this.userRegistrationForm.get('phonenumber')?.value,
      Password: this.userRegistrationForm.get('password')?.value,
      UserRole: 'user',
    };
    this.api.registerUser(user).subscribe(
      (response) => {
        console.log(user);
        this.userRegistrationForm.reset();
        this.showSuccessToast('Success', 'User registered Successfully');
        this.router.navigate(['/login']);
      },
      (error) => {
        this.showErrorToast('Error', 'User Not Registered');
        console.log(error);
      }
    );
  }

  createCompareValidator(
    controlOne: AbstractControl,
    controlTwo: AbstractControl
  ) {
    return () => {
      if (controlOne.value != controlTwo.value) {
        return { match_error: true };
      }
      return null;
    };
  }

  get userRegistrationFormControls() {
    return this.userRegistrationForm.controls;
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
