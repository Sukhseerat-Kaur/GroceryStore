import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SuperAdminService } from 'src/app/services/super-admin.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-admin',
  templateUrl: './add-admin.component.html',
  styleUrls: ['./add-admin.component.css'],
})
export class AddAdminComponent {
  addAdminForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private adminService: SuperAdminService,
    private toastService: ToastrService
  ) {
    this.addAdminForm = this.fb.group({
      name: ['', Validators.required],
      userEmail: [
        '',
        Validators.compose([Validators.email, Validators.required]),
      ],
      phonenumber: [
        '',
        Validators.compose([Validators.pattern('[- +()0-9]+')]),
      ],
      password: ['', Validators.required],
      confirmPassword: [''],
    });
    this.addAdminForm.addValidators(
      this.createCompareValidator(
        this.addAdminForm.get('password') as AbstractControl,
        this.addAdminForm.get('confirmPassword') as AbstractControl
      )
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

  onSubmit() {
    var user = {
      UserName: this.addAdminForm.get('name')?.value,
      UserEmail: this.addAdminForm.get('userEmail')?.value,
      PhoneNumber: this.addAdminForm.get('phonenumber')?.value,
      Password: this.addAdminForm.get('password')?.value,
      UserRole: 'admin',
    };
    this.adminService.addAdmin(user).subscribe(
      (response) => {
        console.log(user);
        this.addAdminForm.reset();
        this.router.navigate(['/login']);
        this.showSuccessToast('Success', 'Admin added successfully');
      },
      (error) => {
        this.showErrorToast('Error', "Couldn't add admin");
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
        return { match_error: 'Value does not match!' };
      }
      return null;
    };
  }
  get addAdminFormControls() {
    return this.addAdminForm.controls;
  }
}
