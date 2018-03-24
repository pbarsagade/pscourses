import { Component, OnInit } from '@angular/core';
import { IProduct } from './product';
import { ProductService } from './product.service';

@Component({
    // tslint:disable-next-line:component-selector
   // selector: 'pm-products',
    templateUrl: './product-list.component.html',
    styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
    pageTitle = 'Product List';
    imageWidth = 50;
    imageMargin = 2;
    showImage = false;
    _filterList: string;
    errorMessage: string;

    get filterList(): string {
        return this._filterList;
    }

    set filterList(value: string) {
        this._filterList = value;
        this.filteredProducts = this.filterList ? this.performFilter(this.filterList) : this.products;
    }

    filteredProducts: IProduct[];

    products: IProduct[];

    constructor(private productService: ProductService) {
    }

    toggleImage = function () {
        this.showImage = !this.showImage;
    };
    ngOnInit(): void {
        this.productService.getProducts()
            .subscribe(data => { this.products = data; this.filteredProducts = this.products; },
                error => this.errorMessage = <any>error);
    }

    performFilter(filterBy: string): IProduct[] {
        filterBy = filterBy.toLocaleLowerCase();
        return this.products.filter((product: IProduct) =>
            product.productName.toLocaleLowerCase().indexOf(filterBy) !== -1);
    }
    onRatingClicked(message: string): void {
        this.pageTitle = 'Product List : ' + message;
    }
}
