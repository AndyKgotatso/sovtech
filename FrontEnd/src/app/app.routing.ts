import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {BrowserModule} from '@angular/platform-browser';
import {Routes, RouterModule} from '@angular/router';
import {CategoriesComponent} from './components/categories/categories.component';
import {CategoryDetailComponent} from './components/category-detail/category-detail.component';
import {PeopleComponent} from './components/people/people.component';
import {SearchComponent} from './components/search/search.component';

const routes: Routes = [
    {path: '', redirectTo: 'categories', pathMatch: 'full'},
    {path: 'categories', component: CategoriesComponent},
    {path: 'categories/:category', component: CategoryDetailComponent},
    {path: 'people', component: PeopleComponent},
    {path: 'search', component: SearchComponent},
    {path: '**', redirectTo: 'categories', pathMatch: 'full'},
];

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        RouterModule.forRoot(routes)
    ],
    exports: [],
})
export class AppRoutingModule {
}
