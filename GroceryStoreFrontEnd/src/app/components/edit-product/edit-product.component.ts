import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductService } from 'src/app/services/product.service';
import { ActivatedRoute } from '@angular/router';
import { CategoryService } from 'src/app/services/category.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css'],
})
export class EditProductComponent implements OnInit {
  availableCategories: string[] = [];
  editProductForm: FormGroup = new FormGroup({});
  selectedFile!: File;
  fetchedFile!: File;
  isDataFetched: boolean = false;
  productId: number = 0;
  selectedCategories: string[] = [];
  productData: Product = {
    productId: 0,
    productName: '',
    productCategories: '',
    productDescription: '',
    productPrice: 0,
    productDiscount: 0,
    productQuantity: 0,
    imagePath: '',
    isDeleted: false,
  };
  constructor(
    private productService: ProductService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private toastService: ToastrService
  ) {}
  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe(
      (response) => {
        this.isDataFetched = false;
        this.availableCategories = [...response];
        console.log(this.availableCategories);
        let productIdString = this.route.snapshot.paramMap.get('productId');
        this.productId =
          productIdString !== null ? parseInt(productIdString) : 0;

        this.productService.getProductById(this.productId).subscribe(
          (responseProduct) => {
            this.productData = { ...responseProduct };
            this.selectedCategories =
              this.productData.productCategories.split(',');
            console.log(response);
            console.log(this.productData);
            this.productService
              .getImage(this.productId)
              .subscribe((imageResponse) => {
                var imageExtension = this.productData.imagePath
                  .split('.')
                  .pop();
                console.log(imageExtension);
                var blob = new Blob([imageResponse], {
                  type: `image/${imageExtension}`,
                });
                var fetchedFile = new File([blob], `image.${imageExtension}`, {
                  type: blob.type,
                });
                console.log(fetchedFile);
                this.editProductForm = this.fb.group({
                  name: [
                    this.productData.productName,
                    Validators.compose([
                      Validators.required,
                      Validators.maxLength(100),
                    ]),
                  ],
                  description: [
                    this.productData.productDescription,
                    Validators.compose([
                      Validators.required,
                      
                    ]),
                  ],
                  quantity: [
                    this.productData.productQuantity,
                    Validators.compose([
                      Validators.required,
                      Validators.pattern('^[0-9]*$'),
                    ]),
                  ],
                  price: [
                    this.productData.productPrice,
                    Validators.compose([
                      Validators.required,
                      Validators.pattern('^[0-9]+(.[0-9]{0,2})?$'),
                    ]),
                  ],
                  discount: [
                    this.productData.productDiscount,
                    Validators.pattern('^[0-9]+(.[0-9]{0,2})?$'),
                  ],
                });
                this.selectedFile = fetchedFile;
                this.addCategoriesToForm();
                this.isDataFetched = true;
              });
          },
          (err) => {
            this.showErrorToast('Error', err.error.message);
          }
        );
      },
      (err) => {
        this.showErrorToast('Error', 'Server error');
      }
    );
  }

  addCategoriesToForm() {
    this.availableCategories.forEach((category) => {
      this.editProductForm.addControl(
        category,
        new FormControl(this.selectedCategories.includes(category))
      );
    });
  }

  get editProductFormControl() {
    return this.editProductForm.controls;
  }

  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
  get getImage() {
    return this.editProductForm.value.image;
  }
  getImageUrl(id: number) {
    return this.productService.getImageUrl(id);
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
  updateProduct() {
    var control = this.editProductForm.controls;
    var newSelectedCategories = [];
    for (let i = 0; i < this.availableCategories.length; i++) {
      if (control[this.availableCategories[i]].value === true)
        newSelectedCategories.push(this.availableCategories[i]);
    }
    var categoriesString = newSelectedCategories.join(',');

    const formData = new FormData();
    formData.append('productId', this.productId.toString());
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
    formData.append('productDiscount', control['discount'].value.toString());
    formData.forEach((value, key) => {
      console.log(key);
      console.log(value);
    });

    this.productService.updateProduct(formData).subscribe(
      (response) => {
        this.showSuccessToast('Success', response.message);
      },
      (err) => {
        this.showErrorToast('Error', err.error.message);
      }
    );
  }
}
