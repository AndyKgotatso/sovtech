import {Component, OnDestroy, OnInit} from '@angular/core';
import {CategoriesService} from './services/categories.service';
import {Categories} from './models/categories';

@Component({
    selector: 'app-categories',
    templateUrl: './categories.component.html',
    styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit, OnDestroy {
    categories: Categories;
    isLoading: boolean;

    constructor(private categoriesService: CategoriesService) {
    }

    async ngOnInit(): Promise<void> {
        this.isLoading = true;
        const navbar = document.getElementsByTagName('app-navbar')[0].children[0];
        navbar.classList.remove('navbar-transparent');

        this.categories = await this.categoriesService.getCategories();
        this.isLoading = false;
    }

    ngOnDestroy() {
        let navbar = document.getElementsByTagName('app-navbar')[0].children[0];
    }
}
