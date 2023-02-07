import { Component, ElementRef, Renderer2 } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { interval, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-main-schedule',
  templateUrl: './main-schedule.component.html',
  styleUrls: ['./main-schedule.component.css']
})
export class MainScheduleComponent {
  matches: Match[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;
  
    constructor(private matchesService: MatchesService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {
  
        this.matchesService.errorMessage.subscribe(error => {
          this.errorMessage = error;
        })
  
      this.matchesService.getSchedule()
      .subscribe({
        next: (matches) => {
          this.matches = matches.map(match => {
            return {
              ...match,
            }
          });
          console.log(this.matches);
          if (matches)
          {
            this.Hideloader();
          } 
        },
        error: (response) => {
          console.log(response);
        }
      });     

      this.updateSubscription = interval(500).pipe(
        switchMap(() => this.matchesService.getSchedule())
      ).subscribe({
        next: (matches) => {
          this.matches = matches;
        },
        error: (error) => {
          // Handle error
        }
      });
    
    }

    Hideloader() {
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#schedulecontainer'), 'display', 'block');         
    }
}
