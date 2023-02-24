import { Component } from '@angular/core';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';

@Component({
  selector: 'app-main-leagues',
  templateUrl: './main-leagues.component.html',
  styleUrls: ['./main-leagues.component.css']
})
export class MainLeaguesComponent {
  leagues: Leagues[] = [];
  constructor(private leaguesService: LeaguesService){ 

    this.leaguesService.getAllLeagues()
    .subscribe({
      next: (leagues) => {
        this.leagues = leagues
      },
      error: (response) => { 
        console.log(response);
    }
  })
  }


  // ngOninit(): void{
  //   this.leaguesService.getAllLeagues().subscribe({
  //     next: (leagues) => {
  //       this.leagues = leagues;
  //     },
  //     error: (response) => { 
  //       console.log(response);
  //   }
  // }

  //}
}
