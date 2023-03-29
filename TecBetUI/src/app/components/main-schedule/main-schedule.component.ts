import { Component, ElementRef, Renderer2, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';
import { interval, Subscription, takeUntil } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Team } from 'src/app/models/teams.model';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-main-schedule',
  templateUrl: './main-schedule.component.html',
  styleUrls: ['./main-schedule.component.css']
})
export class MainScheduleComponent implements OnDestroy {
  games: Match[] = [];
  finishedGames: Match[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription | any;
  sortedGames: Match[] = [];
  sortedFinGames: Match[] = [];
  byeCheese: boolean = false;
  timerSubscription: Subscription | any;
  timerStopSignal = new Subject<void>();
  duration: number = 0;

  // Paginator configurations
  pageIndex = 0;
  pageSize = 8;

  @ViewChild(MatSort, {static: true}) sort: MatSort | any;

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
    this.stopTimer();
  } 
  
    constructor(private matchesService: MatchesService, 
      private el: ElementRef, private renderer: Renderer2, private router: Router) {

        this.fetchFin();
        this.fetch();

        this.updateSubscription = interval(2500).subscribe(() => {
          this.fetch();
        });
    }

    fetch() {    
      this.matchesService.errorMessage.subscribe(error => {
        this.errorMessage = error;
      });

      this.matchesService.getSchedule().subscribe({
        next: (games) => {
          this.games = games.map(game => {

            let participatingTeams = game.participatingTeams;
            let timeLogs = game.timeLogs

            this.nullCheck(participatingTeams);

            if(game.matchState != 0)
            {
            this.Timer(timeLogs[0]);
          }

            console.log(game);

            return {
              ...game,
              participatingTeams: participatingTeams,
              league: game.league ?? "Friendly Match",
            }
          });
          if (games)
          {
            this.sortedGames = this.games.slice();
            this.Hideloader();
            this.sortData(this.sort);
          }
          if (games.length > 0)
          {
            this.errorMessage = "";
          }
        },
      });   
    }

    nullCheck(participatingTeams: any) { 
      for (let i = 0; i < participatingTeams.length; i++) {
        if (participatingTeams[i].name == null) {
          participatingTeams[i] = {
            name: this.byeCheese ? 'BYE' : 'TBD',
            id: '00000000-0000-0000-0000-000000000000',
            isAvailable: true,
            matches: [],
            availability: '',
            players: [],
            score: 0,
            result: 0,
            rating: 0,
            round: participatingTeams[i].round,
          };
        }
      }
    }

    stopTimer() {
      this.timerStopSignal.next();
      this.timerStopSignal.complete();
    }

    Timer(timeLogs: any) 
    { 

      const dateObj = new Date(`1970-01-01T${timeLogs.timeStamp}`);

      console.log(dateObj);
      // Get the current datetime
      const unixTimestamp = dateObj.getTime();

      console.log(unixTimestamp);
      
      // Start the timer if the match has started
      if (!this.timerSubscription) {
        console.log("hi");
        this.timerSubscription = interval(1000)
          // .pipe(takeUntil(this.timerStopSignal)) // timerStopSignal is an Observable that signals when to stop the timer
          .subscribe(() => {

            const newTimestamp = unixTimestamp + 1000;
            timeLogs.timeStamp = newTimestamp;

          });
      }
    }

    fetchFin() {    
      this.matchesService.getFinishedMatches().subscribe({
        next: (finishedGames) => {
          // console.log(this.finishedGames);
          this.finishedGames = finishedGames.map(finishedGame => {

            let participatingTeams = finishedGame.participatingTeams;

            this.nullCheck(participatingTeams);

            return {
              ...finishedGame,
              participatingTeams: participatingTeams,
              league: finishedGame.league ?? "Friendly Match",
            }
          });
          if (finishedGames)
          {
            this.sortedFinGames = this.finishedGames.slice();
            this.Hideloader();
            this.sortData(this.sort);
          }
          this.matchesService.errorMessage.subscribe(error => {
            this.errorMessage = error;
            if (finishedGames.length > 0)
            {
              this.errorMessage = "";
            }
          });
        },
      });   
    }

    GoMatchDetails(id: string, participatingTeams: Team[])
    {
    if(participatingTeams[0].name != 'TBD' && participatingTeams[1].name != 'TBD')
    {
     if(participatingTeams[0].name != 'BYE' && participatingTeams[1].name != 'BYE')
     {
     this.router.navigateByUrl("details/" + id);
     }
    }
    }

    Hideloader() {
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#schedulecontainer'), 'display', 'block');
    }

    showArchivedMatches() {

      this.byeCheese = true;

      this.updateSubscription.unsubscribe();

      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatchesButton'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#paginator'), 'display', 'block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatchesButton'), 'display', 'inline-block'); 

      this.fetchFin();

      this.updateSubscription = interval(2500).subscribe(() => {
        this.fetchFin();
      });
      
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatches'), 'display', 'block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatches'), 'display', 'none'); 
    }

    
    showActiveMatches() {

      this.byeCheese = false;

      this.updateSubscription.unsubscribe();
      
      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatchesButton'), 'display', 'none'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatchesButton'), 'display', 'inline-block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#paginator'), 'display', 'none'); 

      this.fetch();

      this.updateSubscription = interval(2500).subscribe(() => {
        this.fetch();
      });

      this.renderer.setStyle(this.el.nativeElement.querySelector('#activeMatches'), 'display', 'block'); 
      this.renderer.setStyle(this.el.nativeElement.querySelector('#archivedMatches'), 'display', 'none'); 
    }

      //This functions take the sorted teams and updates them

  updateSortedGames() {
    const sortColumn = this.sort.active;
    const sortDirection = this.sort.direction;
    this.sortedGames = this.games.slice().sort((a, b) => {
      const isAsc = sortDirection === 'asc';
      switch (sortColumn) {
        case 'dateOfMatch': return compare(a.dateOfMatch, b.dateOfMatch, isAsc);
        case 'name': return compare(a.participatingTeams[0].name, b.participatingTeams[0].name, isAsc);
        case 'league': return compare(a.league, b.league, isAsc);
        default: return 0;
      }
    });
  }

  updateSortedFinGames() {
    const sortColumn = this.sort.active;
    const sortDirection = this.sort.direction;
    this.sortedFinGames = this.finishedGames.slice().sort((a, b) => {
      const isAsc = sortDirection === 'asc';
      switch (sortColumn) {
        case 'dateOfMatch': return compare(a.dateOfMatch, b.dateOfMatch, isAsc);
        case 'name': return compare(a.participatingTeams[0].name, b.participatingTeams[0].name, isAsc);
        case 'league': return compare(a.league, b.league, isAsc);
        default: return 0;
      }
    });
  }

  //This functions saves the sort event, to dynamically update the sorted teams, whenever it gets re-fetched

  sortData(event: any) {
    this.sort = event; // Store sort state for dynamic data update
    this.updateSortedGames();
    this.updateSortedFinGames();
  }

    //Detects pagechange and updates the data. Get pagedGames is for the pagination, where it slices the data for display, it keeps track of pageindex to know where to pick up from.

    onPageChange(event: any) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }

    get pagedGames() {
      const startIndex = this.pageIndex * this.pageSize;
      const endIndex = startIndex + this.pageSize;
      return this.sortedFinGames.slice(startIndex, endIndex);
    }

}

function compare(a: Date | string, b: Date | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
