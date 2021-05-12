import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Categories} from '../models/categories';
import {environment} from '../../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class CategoriesService {

    constructor(private httpClient: HttpClient) {
    }

    async getCategories(): Promise<Categories> {
        return await this.httpClient.get(environment.chuckCategoriesUrl).toPromise().then((categories: Categories) => {
            return categories;
        }).catch(res => {
            return null;
        })
    }
}
