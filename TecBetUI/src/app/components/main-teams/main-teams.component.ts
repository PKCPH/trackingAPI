import { Component, ElementRef, Renderer2 } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main-teams',
  templateUrl: './main-teams.component.html',
  styleUrls: ['./main-teams.component.css']
})
export class MainTeamsComponent {

  teams: Team[] = []
  deletedObject: boolean = false;
  errorMessage: string = "";
  
    constructor(private teamsService: TeamsService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {
  
        this.teamsService.errorMessage.subscribe(error => {
          this.errorMessage = error;
        })
  
      this.teamsService.getAllTeams()
      .subscribe({
        next: (teams) => {
          this.teams = teams.map(team => {
            return {
              ...team,
              availability: team.IsAvailable ? 'Yes' : 'No'
            }
          });
          if (teams)
          {
            this.Hideloader();
          }  
        },
        error: (response) => {
          console.log(response);
        }
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
                    availability: team.IsAvailable ? 'Yes' : 'No'
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
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#playercontainer'), 'display', 'block');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
    }
  
    GoAddTeam() {
      this.router.navigateByUrl('/teams/add')
    }

}
