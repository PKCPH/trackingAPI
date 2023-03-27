import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';

@Component({
  selector: 'app-add-league',
  templateUrl: './add-league.component.html',
  styleUrls: ['./add-league.component.css']
})
export class AddLeagueComponent implements OnInit {

  addLeagueRequest: Leagues = {
    id: '',
    name: '',
    startDate: ''
  }
  constructor(private leagueService: LeaguesService, private router: Router){
  }

  ngOnInit(): void {
  }

  addLeague(){
    this.leagueService.addLeague(this.addLeagueRequest)
    .subscribe({
      next: (league) => {
        this.router.navigate(['leagues']);
      }
    });
  }
}
