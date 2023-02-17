import { Component, ElementRef, Renderer2, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
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

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }
  
    constructor(private teamsService: TeamsService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {

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
            // console.log(this.matches);
            if (teams)
            {
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
          },

        });   
  
    }
  
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
  
    Hideloader() {
              // Setting display of spinner element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#teamcontainer'), 'display', 'block');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
    }
  
    GoAddTeam() {
      this.router.navigateByUrl('/teams/add')
    }

}
