import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Categories} from '../../categories/models/categories';
import {environment} from '../../../../environments/environment';
import {CategoryDetail} from '../models/category-detail';

@Injectable({
    providedIn: 'root'
})
export class CategoryDetailService {

    constructor(private httpClient: HttpClient) {
    }

    async getCategoryDetail(category: string): Promise<CategoryDetail> {
        return await this.httpClient.get(environment.chuckCategoryDetailUrl + category).toPromise()
            .then((categoryDetail: CategoryDetail) => {
                return categoryDetail;
            }).catch(res => {
                return null;
            })
    }
}
