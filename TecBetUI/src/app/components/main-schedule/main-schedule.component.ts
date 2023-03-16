import { Component, ElementRef, Renderer2, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';
import { interval, Subscription } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-main-schedule',
  templateUrl: './main-schedule.component.html',
  styleUrls: ['./main-schedule.component.css']
})
export class MainScheduleComponent implements OnDestroy {
  games: Match[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;

  // Paginator configurations
  pageIndex = 0;
  pageSize = 8;

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  } 
  
    constructor(private matchesService: MatchesService, 
      private el: ElementRef, private renderer: Renderer2, private router: Router) {

        this.fetch();

        this.updateSubscription = interval(2500).subscribe(() => {
          this.fetch();
        });
    }

    fetch() {    
      this.matchesService.getSchedule().subscribe({
        next: (games) => {
          this.games = games.map(game => {
            return {
              ...game,
            }
          });
          // console.log(this.games);
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

    fetchFin() {    
      this.matchesService.getFinishedMatches().subscribe({
        next: (games) => {
          this.games = games.map(game => {
            return {
              ...game,
            }
          });
          // console.log(this.games);
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

    GoMatchDetails(id: string)
    {
      this.router.navigateByUrl("details/" + id);
    }

    Hideloader() {
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#schedulecontainer'), 'display', 'block');
    }

    showArchivedMatches() {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatches'), 'display', 'table'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatches'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatchesButton'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatchesButton'), 'display', 'inline-block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#paginator'), 'display', 'block'); 
      this.updateSubscription.unsubscribe();

      this.fetchFin();

      this.updateSubscription = interval(2500).subscribe(() => {
        this.fetchFin();
      });
    }

    
    showActiveMatches() {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatches'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatches'), 'display', 'table'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatchesButton'), 'display', 'inline-block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatchesButton'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#paginator'), 'display', 'none'); 
      this.updateSubscription.unsubscribe();

      this.fetch();

      this.updateSubscription = interval(2500).subscribe(() => {
        this.fetch();
      });
    }

    onPageChange(event: any) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }

    get pagedGames() {
      const startIndex = this.pageIndex * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      return this.games.slice(startIndex, endIndex);
    }

}
