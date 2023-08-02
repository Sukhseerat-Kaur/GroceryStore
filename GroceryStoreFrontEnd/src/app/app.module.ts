import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NgbCollapseModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { CartComponent } from './components/cart/cart.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { ProductComponent } from './components/product/product.component';
import { OrdersComponent } from './components/orders/orders.component';
import { NotAuthorizedComponent } from './components/not-authorized/not-authorized.component';
import { MostOrderedComponent } from './components/most-ordered/most-ordered.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CartCardComponent } from './components/cart-card/cart-card.component';
import { AddAdminComponent } from './components/add-admin/add-admin.component';
import AdminListComponent from './components/admin-list/admin-list.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { OrderPlacedComponent } from './components/order-placed/order-placed.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    AddProductComponent,
    CartComponent,
    EditProductComponent,
    ProductComponent,
    OrdersComponent,
    NotAuthorizedComponent,
    MostOrderedComponent,
    ProductCardComponent,
    CartCardComponent,
    AddAdminComponent,
    AdminListComponent,
    UserRegisterComponent,
    UserLoginComponent,
    AdminListComponent,
    OrderPlacedComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule,
    FontAwesomeModule,
    NgbCollapseModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    CommonModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
