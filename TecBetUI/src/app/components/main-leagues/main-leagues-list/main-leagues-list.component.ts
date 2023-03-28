import { Component, ElementRef, ViewChild } from '@angular/core';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';

@Component({
  selector: 'app-main-leagues-list',
  templateUrl: './main-leagues-list.component.html',
  styleUrls: ['./main-leagues-list.component.css']
})
export class MainLeaguesListComponent {

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

          // this.nullCheck(matches);

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
        console.log(matches[i].participatingTeams[k].name);
      }
  }
  }

  toggleTable(leagueName: string) {
    const table = document.getElementById(leagueName) as HTMLElement;
    if (table.style.display === 'none') {
      table.style.display = 'block';
    } else {
      table.style.display = 'none';
    }
  }
}
