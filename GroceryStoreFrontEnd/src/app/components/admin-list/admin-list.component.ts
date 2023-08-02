import { Component, OnInit } from '@angular/core';
import { SuperAdminService } from 'src/app/services/super-admin.service';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-list',
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.css'],
})
export default class AdminListComponent implements OnInit {
  adminList: any[] = [];
  faPlus = faPlus;
  constructor(
    private superAdminService: SuperAdminService,
    private router: Router,
    private toastService: ToastrService
  ) {}
  ngOnInit(): void {
    this.superAdminService.AdminListUpdated.subscribe((response) => {
      this.getAdminList();
    });
    this.getAdminList();
  }

  goToAddAdminPage() {
    this.router.navigate(['/add-admin']);
  }

  getAdminList() {
    this.superAdminService.getAllAdmins().subscribe((response) => {
      console.log(response);
      this.adminList = response;
      this.adminList.sort((a, b) => {
        if (a.userName < b.userName) return -1;
        if (a.userName > b.userName) return 1;
        return 0;
      });
    });
  }
  deleteAdmin(userId: number) {
    this.superAdminService.removeAdmin(userId).subscribe(
      (response) => {
        this.showSuccessToast('Success', 'Admin deleted successfully');
        this.superAdminService.AdminListUpdated.next();
      },
      (err) => {
        this.showErrorToast('Error', "Can't delete admin");
        console.log(err.error);
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
