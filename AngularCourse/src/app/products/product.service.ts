import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';


import { IProduct } from './product';

@Injectable()
export class ProductService {

    private productUrl = './api/products/products.json';
    constructor(private http: HttpClient) { }

    getProducts(): Observable<IProduct[]> {
        return this.http.get<IProduct[]>(this.productUrl).do(data => console.log(JSON.stringify(data))); // .catch(this.handleError);
    }

    handleError(err: HttpErrorResponse): any {
        console.log(err.message);
        return Observable.throw(err.message);
    }
}
