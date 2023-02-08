import { Component, ElementRef, Renderer2, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';
import { interval, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-main-schedule',
  templateUrl: './main-schedule.component.html',
  styleUrls: ['./main-schedule.component.css']
})
export class MainScheduleComponent implements OnDestroy {
  games: Match[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  } 
  
    constructor(private matchesService: MatchesService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {
  
        this.updateSubscription = interval(1500).pipe(
          switchMap(() => this.matchesService.getSchedule())
        )
        .subscribe({
          next: (games) => {
            this.games = games.map(game => {
              return {
                ...game,
              }
            });
            // console.log(this.matches);
            if (games)
            {
              this.Hideloader();
            }
            this.matchesService.errorMessage.subscribe(error => {
              this.errorMessage = error;
              if (games.length > 0)
              {
                this.errorMessage = "";
              }
            });
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
              this.renderer.setStyle(this.el.nativeElement.querySelector('#schedulecontainer'), 'display', 'block'); 
    }

}
