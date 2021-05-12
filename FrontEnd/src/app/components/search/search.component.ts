import {Component, OnDestroy, OnInit} from '@angular/core';
import {SearchService} from './services/search.service';
import {SearchResponse} from './models/search-result';

@Component({
    selector: 'app-search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, OnDestroy {

    isLoading: boolean;
    searchResponse: SearchResponse;
    delay: any;
    query: string;

    constructor(private searchService: SearchService) {
    }

    async ngOnInit(): Promise<void> {
        this.isLoading = true;
        const navbar = document.getElementsByTagName('app-navbar')[0].children[0];
        navbar.classList.remove('navbar-transparent');
        this.isLoading = false;
    }

    ngOnDestroy() {
        const navbar = document.getElementsByTagName('app-navbar')[0].children[0];
    }

    async search(value: string) {
        this.isLoading = true;
        this.query = value;
        this.searchResponse = await this.searchService.getSearchResponse(value, 1);
        this.isLoading = false;
    }

    async onPageChange(pageNumber: number) {
        this.isLoading = true;
        this.searchResponse = await this.searchService.getSearchResponse(this.query, pageNumber);
        this.isLoading = false;
    }
}
