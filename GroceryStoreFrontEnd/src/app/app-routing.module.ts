import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { CartComponent } from './components/cart/cart.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { ProductComponent } from './components/product/product.component';
import { AdminGuard } from './guards/admin.guard';
import { LoginGuard } from './guards/login.guard';
import { NotAuthorizedComponent } from './components/not-authorized/not-authorized.component';
import { MostOrderedComponent } from './components/most-ordered/most-ordered.component';
import { OrdersComponent } from './components/orders/orders.component';
import { UserGuard } from './guards/user.guard';
import { SuperAdminGuard } from './guards/super-admin.guard';
import AdminListComponent from './components/admin-list/admin-list.component';
import { AddAdminComponent } from './components/add-admin/add-admin.component';
import { NotLoginGuard } from './guards/not-login.guard';
import { OrderPlacedComponent } from './components/order-placed/order-placed.component';

const routes: Routes = [
  {
    path: 'login',
    component: UserLoginComponent,
    canActivate: [NotLoginGuard],
  },
  {
    path: 'register',
    component: UserRegisterComponent,
    canActivate: [NotLoginGuard],
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
  },
  {
    path: 'add-product',
    component: AddProductComponent,
    canActivate: [LoginGuard, AdminGuard],
  },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [LoginGuard, UserGuard],
  },
  {
    path: 'product/edit/:productId',
    component: EditProductComponent,
    canActivate: [LoginGuard, AdminGuard],
  },
  {
    path: 'product/:productId',
    component: ProductComponent,
  },
  {
    path: 'not-authorized',
    component: NotAuthorizedComponent,
  },
  {
    path: 'most-ordered',
    component: MostOrderedComponent,
    canActivate: [LoginGuard, AdminGuard],
  },
  {
    path: 'my-orders',
    component: OrdersComponent,
    canActivate: [LoginGuard, UserGuard],
  },
  {
    path: 'admin-list',
    component: AdminListComponent,
    canActivate: [LoginGuard, SuperAdminGuard],
  },
  {
    path: 'add-admin',
    component: AddAdminComponent,
    canActivate: [LoginGuard, SuperAdminGuard],
  },
  {
    path: 'order-placed/:orderId',
    component: OrderPlacedComponent,
    canActivate: [LoginGuard],
  },
  {
    path: '',
    component: DashboardComponent,
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
