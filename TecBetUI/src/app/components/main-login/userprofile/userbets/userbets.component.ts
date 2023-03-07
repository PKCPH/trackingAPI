import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Bet } from 'src/app/models/bet.model';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { BetService } from 'src/app/services/bet.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-userbets',
  templateUrl: './userbets.component.html',
  styleUrls: ['./userbets.component.css']
})
export class UserbetsComponent {

  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};
  // bets: Bet = {id: '', gameMatchId: '', loginId: '', team: '', amount: 0, payoutAmount: 0, betTime: new Date(), result: null };
  bets: Bet[] = [];
  test: any;

  // id: string;
  // gameMatchId: string;
  // loginId: string;
  // team: Team;
  // amount: number;
  // payoutAmount: number;
  // betTime: Date;
  // result: BetResult | null;

  constructor(private route: ActivatedRoute, private authService: AuthguardService, private betService: BetService, private _location: Location) {
    this.route.paramMap.subscribe({
      next: (params) => {
        const userName = params.get('username');
        if (userName) {
          //Call API
this.authService.getUser(userName)
.subscribe({
  next: (response) => {
this.credentials.id = response.id,
this.credentials.userName = response.userName
this.betService.getUserBets(this.credentials.id)
.subscribe({
  next: (bets) => {
    this.bets = bets.map(bets => {
      return {
        ...bets,
      }
    });
    console.log(this.bets);
  }
});   
  }
});
        }
      }
    }) 
  }

  goBack() {
    this._location.back();
  }

}
