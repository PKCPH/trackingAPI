import { Component, ElementRef, Renderer2, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { interval, Subscription, switchMap } from 'rxjs';
import { Sort, MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-main-teams',
  templateUrl: './main-teams.component.html',
  styleUrls: ['./main-teams.component.css']
})
export class MainTeamsComponent implements OnDestroy {

  teams: Team[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription | any;
  storedCredentialsString: any;
  role: any;
  sortedTeams: Team[] = [];

  //ViewChield is used to fetch a components html element object

  @ViewChild(MatSort, {static: true}) sort: MatSort | any;

  //ngOnDestroy is when you route out of a component it triggers, and inside it I unsubscribe to everything, so it doesnt keep running while on another component.

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private teamsService: TeamsService, private router: Router,
    private el: ElementRef, private renderer: Renderer2) {

    //This is used to hide and show various buttons that only admins should have access to, but the page itself is viewable by everyone now.

    this.getCredentials();

    this.fetch();

    this.updateSubscription = interval(2500).subscribe(() => {
      this.fetch();
    });

    window.addEventListener('userLoggedIn', this.getCredentials.bind(this));
  }


  fetch() {

    this.teamsService.errorMessage.subscribe(error => {
      this.errorMessage = error;
    });

    this.teamsService.getAllTeams()
      .subscribe({
        next: (teams) => {
          this.teams = teams.map(team => {
            return {
              ...team,
              availability: team.isAvailable ? 'No' : 'Yes'
            }
          });
          console.log(this.teams);
          if (teams) {
            this.sortedTeams = this.teams.slice();
            this.toggleOverflowDiv();
            this.Hideloader();
            this.sortData(this.sort);
          }
          if (teams.length > 0) {
            this.errorMessage = "";
          }
        }
      });
      }
    

  //The getall method that repeats itself every 1.5 seconds, to dynamically update the view with repeated API calls. 
  //It calls the getAllTeams method, and fills out the "teams" variable with all the entities from the SQL database
  //Custom return, made new property called availability since I wanna translate the true/false to either a yes or no instead for better user view.
  //After teams have been filled or attempted to, have two functions. One is for hiding the loader animation and other is to display a div when theres overflow in the table
  //Then if a error happened it would get fetched from teamsService (which uses a custom error handler) to set the message and hide the Add Button
  //If error string is empty, display the add button
  //.subscribe() is a method that is called on an Observable to start listening for values that are emitted by the Observable. 

  //switchMap() is an operator in the RxJS library that can be used to transform an Observable into a new Observable. 
  //It's commonly used when you have an Observable that emits a value and you need to make another asynchronous call that depends on that value

  Oldfetch() {
    this.updateSubscription = interval(3000).pipe(
      switchMap(() => this.teamsService.getAllTeams())
    )
      .subscribe({
        next: (teams) => {
          this.teams = teams.map(team => {
            return {
              ...team,
              availability: team.isAvailable ? 'No' : 'Yes'
            }
          });
          // console.log(this.teams);
          if (teams) {
            this.toggleOverflowDiv();
            this.Hideloader();
          }
          this.teamsService.errorMessage.subscribe(error => {
            this.errorMessage = error;
            if (teams.length > 0) {
              this.errorMessage = "";
            }
          });
        },
        error: (response) => {
          console.log(response);
        }
      });
  }

  //Simple function to read credentials in localstorage, and then if the contents exist, parse the array with the help of JSON and then set my "role" variable to the value of the .role property in localstorage

  getCredentials() {
    let storedCredentials;

    this.storedCredentialsString = localStorage.getItem("credentials");
    if (this.storedCredentialsString) {
      storedCredentials = JSON.parse(this.storedCredentialsString);

      this.role = storedCredentials.role;
    }
  }

  //This functions take the sorted teams and updates them

  updateSortedTeams() {
    const sortColumn = this.sort.active;
    const sortDirection = this.sort.direction;
    this.sortedTeams = this.teams.slice().sort((a, b) => {
      const isAsc = sortDirection === 'asc';
      switch (sortColumn) {
        case 'name': return compare(a.name, b.name, isAsc);
        default: return 0;
      }
    });
  }

  //This functions saves the sort event, to dynamically update the sorted teams, whenever it gets re-fetched

  sortData(event: any) {
    this.sort = event; // Store sort state for dynamic data update
    this.updateSortedTeams();
  }
  
  //Simple delete, it gets parsed the string and it deletes the correspondant team. After I do another getall, to update my view.

  deleteTeam(id: string) {
    this.teamsService.deleteTeam(id)
      .subscribe({
        next: (response) => {
          this.teamsService.getAllTeams()
            .subscribe({
              next: (teams) => {
                this.teams = teams.map(team => {
                  return {
                    ...team,
                    availability: team.isAvailable ? 'No' : 'Yes'
                  }
                });
              },
              error: (response) => {
                console.log(response);
              }
            });
        }
      });
  }

  //Hides loader, and shows relevant divs that should only appear after loading have finished.

  Hideloader() {
    // Setting display of spinner element to none
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
    this.renderer.setStyle(this.el.nativeElement.querySelector('#teamcontainer'), 'display', 'block');
  }

  GoAddTeam() {
    this.router.navigateByUrl('/teams/add')
  }

  GoEditTeam(id: string) {
    this.router.navigateByUrl('/teams/edit/' + id)
  }

  //Functions to check if a certain table is overflowing, and if it is display a div.

  isTableOverflowing(): boolean {
    return this.el.nativeElement.querySelector('#tablediv').scrollHeight > this.el.nativeElement.querySelector('#tablediv').clientHeight;
  }

  toggleOverflowDiv(): void {
    if (this.isTableOverflowing()) {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#overflow-div'), 'display', 'block');
    } else {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#overflow-div'), 'display', 'none');
    }
  }

}

//Compare function that simply sorts

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
