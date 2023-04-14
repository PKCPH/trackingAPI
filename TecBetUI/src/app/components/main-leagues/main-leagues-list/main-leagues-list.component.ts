import { Component, ElementRef, ViewChild } from '@angular/core';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';
import { faCircleChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faCircleChevronUp } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-leagues-list',
  templateUrl: './main-leagues-list.component.html',
  styleUrls: ['./main-leagues-list.component.css']
})
export class MainLeaguesListComponent {
  arrowDown = faCircleChevronDown;
  arrowUp = faCircleChevronUp;
  output: number = 0;
  byeCheese: boolean = false;

  iconStates: any = {};

  // @ViewChild('myTable') myTable: ElementRef | any;

  leagues: Leagues[] = [];

  constructor(private leaguesService: LeaguesService, private router: Router) {

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
          this.GetRoundTerm(matches);

          return {
            ...league,
            match: matches
          }
        });
        if (leagues) {
          // this.sortedGames = this.games.slice();
          // this.Hideloader();
          // this.sortData(this.sort);
        }
        if (leagues.length > 0) {
          // this.errorMessage = "";
        }
      },
    });
  }

  nullCheck(matches: any) {
    for (let i = 0; i < matches.length; i++) {

      for (let k = 0; k < matches[i].participatingTeams.length; k++) {
        if (matches[i].participatingTeams[k] == null && matches[i].matchState == 6) {
          matches[i].participatingTeams[k] = {
            name: 'BYE',
            id: '00000000-0000-0000-0000-000000000000',
          };
        }
        else if (matches[i].participatingTeams[k] == null && matches[i].matchState != 6) {
          matches[i].participatingTeams[k] = {
            name: 'TBD',
            id: '00000000-0000-0000-0000-000000000000',
          };
        }
        // console.log(matches[i].participatingTeams[k].name);
      }
    }
  }

  GetRoundTerm(matches: any) {
    for (let i = 0; i < matches.length; i++) {

      if (matches[i].round == 1) matches[i].roundTerm = "Grand Finale";
      else if (matches[i].round == 2) matches[i].roundTerm = "Semi-Finale";
      else if (matches[i].round == 3) matches[i].roundTerm = "Quarter-Finale";
      else {
        this.output = 2;
        for (var k = 2; k <= matches[i].round; k++) {
          this.output *= 2;
        }
        matches[i].roundTerm = `Round of ${this.output}`;
      }
    }

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

  toggleTable(startDate: string, leagueId: string) {
    const table = document.getElementById(startDate) as HTMLElement;
    const icon = document.getElementById(leagueId) as HTMLElement;
    if (table.style.display === 'none') {
      table.style.display = 'block';
      this.iconStates[leagueId] = 'down';
    } else {
      table.style.display = 'none';
      this.iconStates[leagueId] = 'up';
    }
  }

  getCredentials() {
    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString) {
      storedCredentials = JSON.parse(storedCredentialsString);

      let role = storedCredentials.role;

      if (role === 'Admin') {
        this.router.navigate(['/matches']);
      } else {
        this.router.navigate(['/']);
      }
    }
    else if (!storedCredentialsString) {
      this.router.navigate(['/']);
    }
  }
}
