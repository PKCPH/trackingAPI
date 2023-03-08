import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { Bet } from 'src/app/models/bet.model';
import { BetService } from 'src/app/services/bet.service';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-betting-window',
  templateUrl: './betting-window.component.html',
  styleUrls: ['./betting-window.component.css']
})
export class BettingWindowComponent {

  match: Match | any;
  // match: Match = {
  //   id: '',
  //   matchState: 0,
  //   dateOfMatch: new Date(),
  //   participatingTeams: [],
  //   state: ''
  // };
  team: Team | any;
  user: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};
  errorMsg: string = '';
  amount: number = 0;

  max = 100;
  min = 0;
  showTicks = false;
  step = 100;
  thumbLabel = false;
  value = 0;

  constructor(
    public activeModal: NgbActiveModal,
    private betService: BetService,
    private authService: AuthguardService
  ) {

    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString)
    {
    storedCredentials = JSON.parse(storedCredentialsString);
    }
    
        const userName = storedCredentials.userName
        if (userName) {
          //Call API
this.authService.getUser(userName)
.subscribe({
  next: (response) => {
this.user.id = response.id
this.user.balance = response.balance,
this.user.userName = response.userName,
this.user.role = response.role
// this.max = response.balance,

// console.log(this.user);
// console.log(this.max, this.min);
  }
});
        }
  }

  onSubmit() {
    if (this.amount > 0) {
      const bet: Bet = {
        id: '',
        gameMatchId: this.match.id,
        loginId: this.user.id,
        team: this.team.name,
        amount: this.amount,
        payoutAmount: 0,
        betTime: new Date(),
        result: null,
      };

      // console.log("Team: " + this.team.name);
      // console.log("Amount: " + this.amount);
      // console.log("Match ID: " + this.match.id);

      this.betService.placeBet(bet).subscribe(
        (bet: Bet) => {
          // Update match details component with bet information
          // ...
          let storedCredentials = {
            userName: this.user.userName,
            role: this.user.role,
          };

          localStorage.setItem("credentials", JSON.stringify(storedCredentials));
          this.activeModal.close();
        },
        (error: any) => {
          // Handle error
          this.errorMsg = error;
          console.log(this.errorMsg);
        }
      );
    }
  }
}