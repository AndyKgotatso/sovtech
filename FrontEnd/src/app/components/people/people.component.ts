import {Component, OnDestroy, OnInit} from '@angular/core';
import {PeopleService} from './services/people.service';
import {People} from './models/people';

@Component({
    selector: 'app-people',
    templateUrl: './people.component.html',
    styleUrls: ['./people.component.scss']
})
export class PeopleComponent implements OnInit, OnDestroy {

    isLoading: boolean;
    people: People;
    pageCounter = 1;

    constructor(private peopleService: PeopleService) {
    }

    async ngOnInit(): Promise<void> {
        this.isLoading = true;
        const navbar = document.getElementsByTagName('app-navbar')[0].children[0];
        navbar.classList.remove('navbar-transparent');

        this.people = await this.peopleService.getPeople(this.pageCounter);
        this.isLoading = false;
    }

    ngOnDestroy() {
        let navbar = document.getElementsByTagName('app-navbar')[0].children[0];
    }

    async prevOrNextCounter(count: number) {
        this.isLoading = true;
        this.pageCounter = this.pageCounter + count;
        this.people = await this.peopleService.getPeople(this.pageCounter);
        this.isLoading = false;
    }

}
