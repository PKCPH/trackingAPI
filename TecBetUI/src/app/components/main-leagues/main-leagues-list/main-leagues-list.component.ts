import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';

@Component({
  selector: 'app-main-leagues-list',
  templateUrl: './main-leagues-list.component.html',
  styleUrls: ['./main-leagues-list.component.css']
})
export class MainLeaguesListComponent implements OnInit {

  @ViewChild('myTable') myTable: ElementRef | any;

  leagues: Leagues[] = [
    // {
    //   id: '9a9a57f2-5b8e-4de6-90b6-1ce076382dea',
    //   name: 'Star League',
    //   leagueState: 'NotStarted',
    //   startDate: new Date()
    // },
    // {
    //   id: '9a9a57n2-5b8e-4de6-90b6-1ce076382dea',
    //   name: 'Esport League',
    //   leagueState: 'NotStarted',
    //   startDate: new Date()
    // },
    // {
    //   id: '9a9a57a2-5b8e-4de6-90b6-1ce076382dea',
    //   name: 'National League',
    //   leagueState: 'NotStarted',
    //   startDate: new Date()
    // },
  ];
  constructor(private leaguesService: LeaguesService) { }

  ngOnInit(): void {
    this.leaguesService.getAllLeagues()
    .subscribe({
      next: (leagues) => {
        this.leagues = leagues;
        console.log(this.leagues);
      },
      error: (response) => {
        console.log(response)
      }
    })
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