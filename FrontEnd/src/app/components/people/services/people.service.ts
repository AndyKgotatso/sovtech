import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import {People} from '../models/people';

@Injectable({
    providedIn: 'root'
})
export class PeopleService {

    constructor(private httpClient: HttpClient) {
    }

    async getPeople(page: number): Promise<People> {
        return await this.httpClient.get(environment.startWarsPeopleUrl + page).toPromise().then((people: People) => {
            return people;
        }).catch(res => {
            return null;
        })
    }
}
