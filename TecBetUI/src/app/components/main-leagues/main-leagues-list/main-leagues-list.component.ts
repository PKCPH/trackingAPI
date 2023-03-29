import { Component, ElementRef, ViewChild } from '@angular/core';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';
import { faCircleChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faCircleChevronUp } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-main-leagues-list',
  templateUrl: './main-leagues-list.component.html',
  styleUrls: ['./main-leagues-list.component.css']
})
export class MainLeaguesListComponent {

   arrow = faCircleChevronDown;

  @ViewChild('myTable') myTable: ElementRef | any;

  leagues: Leagues[] = [];

  constructor(private leaguesService: LeaguesService) { 

this.fetch();

  }

  fetch() {    
    // this.leaguesService.errorMessage.subscribe(error => {
    //   this.errorMessage = error;
    // });

    this.leaguesService.getAllLeagues().subscribe({
      next: (leagues) => {
        this.leagues = leagues.map(league => {

          let matches = league.match;

          // console.log(matches);
          console.log(leagues);

          this.nullCheck(matches);

          return {
            ...league,
            match: matches
          }
        });
        if (leagues)
        {
          // this.sortedGames = this.games.slice();
          // this.Hideloader();
          // this.sortData(this.sort);
        }
        if (leagues.length > 0)
        {
          // this.errorMessage = "";
        }
      },
    });   
  }

  nullCheck(matches: any) { 
    for (let i = 0; i < matches.length; i++) {

      for (let k = 0; k < matches[i].participatingTeams.length; k++)
      {
        if (matches[i].participatingTeams[k] == null) {
          matches[i].participatingTeams[k] = {
            name: 'TBD',
            id: '00000000-0000-0000-0000-000000000000',
          };
        }
        // console.log(matches[i].participatingTeams[k].name);
      }
  }
  }

  toggleTable(leagueName: string, leagueId: string) {
    const table = document.getElementById(leagueName) as HTMLElement;
    const icon = document.getElementById(leagueId) as HTMLElement;
    if (table.style.display === 'none') {
      table.style.display = 'block';
      this.arrow = faCircleChevronUp;   
    } else {
      table.style.display = 'none';
      this.arrow = faCircleChevronDown; 
    }
  }
}
