import { Component, ElementRef, Renderer2 } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';
import { ParticipatingTeam } from 'src/app/models/schedule.model';

@Component({
  selector: 'app-main-schedule',
  templateUrl: './main-schedule.component.html',
  styleUrls: ['./main-schedule.component.css']
})
export class MainScheduleComponent {

  matches: Match[] = [];
  schedule: ParticipatingTeam[] = [];
  teams: Team[] = [];
  errorMessage: string = "";
  
    constructor(private matchesService: MatchesService, private teamsService: TeamsService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {
  
        this.matchesService.errorMessage.subscribe(error => {
          this.errorMessage = error;
        })
  
      this.matchesService.getSchedule()
      .subscribe({
        next: (schedule) => {
          // this.schedule = schedule;
          console.log(this.schedule);
          if (schedule)
          {
            this.Hideloader();
          } 
        },
        error: (response) => {
          console.log(response);
        }
      }); 
     
      this.teamsService.getAllTeams()
      .subscribe({
        next: (teams) => {
          this.teams = teams.map(team => {
            return {
              ...team,
              availability: team.isAvailable ? 'No' : 'Yes'
            };
          });
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
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#matchcontainer'), 'display', 'block');
    }

    getTeamName(teamId: string) {
      const id = teamId as any;
      const teamz = this.teams.find(team => team.id === id);
      return teamz ? teamz.name : 'Team not found';
    }
}
