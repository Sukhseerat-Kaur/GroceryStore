import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';
import { CategoryService } from 'src/app/services/category.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent implements OnInit {
  availableCategories: string[] = [];
  addProductForm: FormGroup = new FormGroup({});
  selectedFile!: File;
  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private categoryService: CategoryService,
    private toastService: ToastrService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.addProductForm = this.fb.group({
      name: [
        '',
        Validators.compose([Validators.required, Validators.maxLength(100)]),
      ],
      description: [
        '',
        Validators.compose([Validators.required]),
      ],
      quantity: [
        '',
        Validators.compose([
          Validators.required,
          Validators.pattern('^[0-9]*$'),
        ]),
      ],
      price: [
        '',
        Validators.compose([
          Validators.required,
          Validators.pattern('^[0-9]+(.[0-9]{0,2})?$'),
        ]),
      ],
      discount: [0, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')],
    });
    this.categoryService.getAllCategories().subscribe(
      (response) => {
        this.availableCategories = response;
        this.addCategoriesToForm();
      },
      (error) => {
        alert(error.error.message);
      }
    );
  }

  addCategoriesToForm() {
    this.availableCategories.forEach((category) => {
      this.addProductForm.addControl(category, new FormControl(false));
    });
  }
  addProduct() {
    var control = this.addProductForm.controls;
    var selectedCategories = [];
    for (let i = 0; i < this.availableCategories.length; i++) {
      if (control[this.availableCategories[i]].value === true)
        selectedCategories.push(this.availableCategories[i]);
    }
    var categoriesString = selectedCategories.join(',');

    const formData = new FormData();
    formData.append('productName', control['name'].value);
    formData.append('productDescription', control['description'].value);
    formData.append('productCategories', categoriesString);
    formData.append('productQuantity', control['quantity'].value.toString());
    formData.append('imageFile', this.selectedFile);
    formData.append('productPrice', control['price'].value.toString());
    formData.append(
      'productDiscount',
      control['discount'].value === null ||
        control['discount'].value.toString() === ''
        ? '0'
        : control['discount'].value.toString()
    );
    formData.forEach((value, key) => {
      console.log(key);
      console.log(value);
    });

    this.productService.addProduct(formData).subscribe(
      (response) => {
        this.showSuccessToast('Success', response.message);
      },
      (err) => {
        console.log(err.error);
      },
      () => {
        this.addProductForm.reset();
      }
    );
  }
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
  get addProductFormControl() {
    return this.addProductForm.controls;
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
