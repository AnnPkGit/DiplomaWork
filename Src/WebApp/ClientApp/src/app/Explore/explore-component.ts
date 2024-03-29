import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserFollower } from '../profile-page/profile.component';

@Component({
  selector: 'explore-component',
  templateUrl: './explore-component.html',
  styleUrls: ['./explore-component.css']
})
export class ExploreComponent implements OnInit {

    input: string = '';
    accounts: UserFollower[] | undefined;
    searchInProgress: boolean = false;

    ngOnInit(): void {
        // throw new Error('Method not implemented.');
    }
    
    constructor(private httpClient: HttpClient) {

    }

    search() {
      if(this.searchInProgress) {
        return;
      }
      
      this.searchInProgress = true;

      this.httpClient.get<UserFollower[]>("api/v1/account/search?Text=" + this.input.trim()).subscribe((response) => {
        this.accounts = response;
        this.searchInProgress = false;
      },
      (error) => {
          this.searchInProgress = false;
        }
      );
    }

    NothingWasFound() {
      if(!this.accounts) {
        return false;
      }
      return this.accounts.length < 1;
    }
}