import { Component, ElementRef, Renderer2, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { interval, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-main-teams',
  templateUrl: './main-teams.component.html',
  styleUrls: ['./main-teams.component.css']
})
export class MainTeamsComponent implements OnDestroy {

  teams: Team[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;

//ngOnDestroy is when you route out of a component it triggers, and inside it I unsubscribe to everything, so it doesnt keep running while on another component.

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }
  
    constructor(private teamsService: TeamsService, private router: Router, 
      private el: ElementRef, private renderer: Renderer2) {

        //Basically this is the checker, to prevent users from trying to navigate to this url without being an admin it checks the localstorage for role credentials
        //If you dont have admin you'll be redirected to the homepage

        let storedCredentials;

        let storedCredentialsString = localStorage.getItem("credentials");
        if (storedCredentialsString)
        {
        storedCredentials = JSON.parse(storedCredentialsString);

        let role = storedCredentials.role;

        if (role === 'Admin') {
          this.router.navigate(['/teams']);
          } else {
            this.router.navigate(['/']);
          }  
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

        this.updateSubscription = interval(1500).pipe(
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
            if (teams)
            {
              this.toggleOverflowDiv();
              this.Hideloader();
            }
            this.teamsService.errorMessage.subscribe(error => {
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'none');
              this.errorMessage = error;
                           
              if (this.errorMessage === '')
              {
                this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
              }
            });
          }
        });   
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
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
    }
  
    GoAddTeam() {
      this.router.navigateByUrl('/teams/add')
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
