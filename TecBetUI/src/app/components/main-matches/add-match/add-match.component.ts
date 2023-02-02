import { Component, ElementRef, Renderer2 } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';

@Component({
  selector: 'app-add-match',
  templateUrl: './add-match.component.html',
  styleUrls: ['./add-match.component.css']
})
export class AddMatchComponent {

  addMatchRequest: Match = {
    id: '',
    teamAScore: 0,
    teamBScore: 0,
    matchState: 0,
    dateOfMatch: new Date(),
    participatingTeams: [],
    state: ''
  };

  teams: Team[] = [];
  errorMessage: string = "";
  
    constructor(private teamsService: TeamsService, private matchesService: MatchesService,
      private router: Router, private location: Location, 
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
              availability: team.isAvailable ? 'No' : 'Yes'
            };
          });
          if (teams)
          {
            this.Hideloader();
          }  
          // Debugging
          // console.log(this.teams);
        },
        error: (response) => {
          console.log(response);
        }
      });       
    }
  
    Hideloader() {
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#availableteamcontainer'), 'display', 'block');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
    }

    createMatch()
    {
      this.matchesService.addMatch(this.addMatchRequest)
      .subscribe({
        next: (members) => {
          this.router.navigate(['matches']);
        }
      });
    }
}
