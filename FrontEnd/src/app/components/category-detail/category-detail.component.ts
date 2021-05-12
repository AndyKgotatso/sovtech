import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {CategoryDetailService} from './services/category-detail.service';
import {CategoryDetail} from './models/category-detail';

@Component({
    selector: 'app-category-detail',
    templateUrl: './category-detail.component.html',
    styleUrls: ['./category-detail.component.scss']
})
export class CategoryDetailComponent implements OnInit {

    categoryDetail: CategoryDetail
    isLoading: boolean;
    categoryName : string;

    constructor(private route: Router, private categoryDetailService: CategoryDetailService) {
    }

    ngOnInit(): void {
        this.isLoading = true;
        const navbar = document.getElementsByTagName('app-navbar')[0].children[0];
        navbar.classList.remove('navbar-transparent');
        this.getCategoryDetail();
    }

    async getCategoryDetail(): Promise<void> {
        this.categoryName  = this.route.url.split('/').reverse()[0];
        this.categoryDetail = await this.categoryDetailService.getCategoryDetail(this.categoryName)
        this.isLoading = false;
    }
}
