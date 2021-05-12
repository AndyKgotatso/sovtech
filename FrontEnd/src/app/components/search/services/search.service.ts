import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {SearchResponse} from '../models/search-result';

@Injectable({
    providedIn: 'root'
})
export class SearchService {

    constructor(private httpClient: HttpClient) {
    }

    async getSearchResponse(query: string, page: number): Promise<SearchResponse> {
        return await this.httpClient.get(environment.searchUrl + query + '&Page=' + page).toPromise()
            .then((searchResponse: SearchResponse) => {
                return searchResponse;
            }).catch(res => {
                return null;
            })
    }
}
