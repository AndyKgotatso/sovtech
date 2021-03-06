import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {NouisliderModule} from 'ng2-nouislider';
import {JwBootstrapSwitchNg2Module} from 'jw-bootstrap-switch-ng2';
import {RouterModule} from '@angular/router';
import {NavigationComponent} from './navigation/navigation.component';
import {ComponentsComponent} from './components.component';
import {CategoriesComponent} from './categories/categories.component';
import { CategoryDetailComponent } from './category-detail/category-detail.component';
import { PeopleComponent } from './people/people.component';
import { SearchComponent } from './search/search.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        NouisliderModule,
        RouterModule,
        JwBootstrapSwitchNg2Module
    ],
    declarations: [
        ComponentsComponent,
        NavigationComponent,
        CategoriesComponent,
        CategoryDetailComponent,
        PeopleComponent,
        SearchComponent
    ],
    exports: [ComponentsComponent]
})
export class ComponentsModule {
}
