import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/app/models/Product';
import { ProductService } from 'src/app/services/product.service';
import { CategoryService } from 'src/app/services/category.service';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  faPlus = faPlus;
  categories: string[] = [];
  categoryForm: FormGroup = new FormGroup({});
  selectedCategories: string[] = [];
  filteredProducts: Product[] = [];
  products: Product[] = [];
  searchInput: string = '';
  isLoading: boolean = true;
  collapseCategories = true;
  isAdmin!: boolean;
  isSuperAdmin!: boolean;

  page: number = 1;
  count: number = 0;
  tableSize: number = 4;

  constructor(
    private router: Router,
    private productService: ProductService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private userService: UserService,
    private toastService: ToastrService
  ) {}

  ngOnInit(): void {
    this.isAdmin = this.userService.isAdmin;
    this.isSuperAdmin = this.userService.isSuperAdmin;
    this.productService.getAllProducts().subscribe(
      (response) => {
        this.products = [...response];
        this.filteredProducts = [...response];
        this.count = this.filteredProducts.length;
        this.categoryService.getAllCategories().subscribe((response) => {
          this.categories = [...response];
        });
        console.log(this.filteredProducts);
        this.isLoading = false;
      },
      (err) => {
        this.showErrorToast('Error', "Products can't be loaded");
        console.log(err);
      }
    );
  }

  handleCategoryChange(category: any) {
    const idx = this.selectedCategories.indexOf(category);
    if (idx === -1) {
      this.selectedCategories.push(category);
    } else {
      this.selectedCategories.splice(idx, 1);
    }
    console.log(this.selectedCategories);
    console.log('categories', this.categories);
    this.filterProducts();

    console.log(this.filteredProducts);

    console.log(category);
  }

  handleSearchChange(e: any) {
    this.searchInput = e.target.value;
    console.log(this.searchInput);
    this.filterProducts();
  }

  filterProducts() {
    if (this.selectedCategories.length === 0) {
      this.filteredProducts = this.products.filter((product) => {
        if (this.searchInput === '') return product;
        else
          return product.productName.toLowerCase().includes(this.searchInput);
      });
    } else {
      this.filteredProducts = this.products
        .filter((product) => {
          let productCategories = product.productCategories.split(',');
          return productCategories.some((category) =>
            this.selectedCategories.includes(category)
          );
        })
        .filter((product) => {
          if (this.searchInput === '') return product;
          else
            return product.productName.toLowerCase().includes(this.searchInput);
        });
    }

    this.count = this.filteredProducts.length;
  }
  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
  }

  onTableDataChange(event: any) {
    this.page = event;
  }
  goToProductPage(productId: number) {
    this.router.navigate(['/product/' + productId]);
  }

  goToAddProductPage() {
    this.router.navigate(['/add-product']);
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
