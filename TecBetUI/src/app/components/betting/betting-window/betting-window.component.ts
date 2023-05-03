import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { Bet } from 'src/app/models/bet.model';
import { BetService } from 'src/app/services/bet.service';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-betting-window',
  templateUrl: './betting-window.component.html',
  styleUrls: ['./betting-window.component.css']
})
export class BettingWindowComponent {

  match: Match | any;
  team: Team | any;
  user: LoginModel = { userName: '', password: '', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: '' };
  errorMessage: string = "";
  amount: number = 100;
  max = 100;
  min = 100;
  showTicks = false;
  step = 100;
  thumbLabel = false;
  storedCredentials: any;

  constructor(
    public activeModal: NgbActiveModal,
    private betService: BetService,
    private authService: AuthguardService
  ) {
    this.getCredentials();

    this.fetch();
  }

  getCredentials() {
    this.storedCredentials;
    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString) {
      this.storedCredentials = JSON.parse(storedCredentialsString);
    }
  }

  fetch() {
    const userName = this.storedCredentials.userName
    if (userName) {
      //Call API
      this.authService.getUser(userName)
        .subscribe({
          next: (response) => {
            this.user.id = response.id
            this.user.balance = response.balance,
              this.user.userName = response.userName
          }
        });
    }
  }

  //Redefines the bet model with the parsed data from matchdetails component, with those and amount value taken from the slider/input field, the bet gets POST'd into our database through our WebAPI

  onSubmit() {
    if (this.amount > 0) {
      const bet: Bet = {
        id: '',
        matchId: this.match.id,
        loginId: this.user.id,
        team: this.team.name,
        amount: this.amount,
        payoutAmount: 0,
        betTime: new Date(),
        betResult: '0',
        betState: '0',
        participatingTeams: []
      };

      // //Debugging purposes
      // console.log("Team: " + this.team.name);
      // console.log("Amount: " + this.amount);
      // console.log("Match ID: " + this.match.id);
      // console.log("Errormessage:" + this.errorMessage);

      this.betService.placeBet(bet).subscribe(
        (bet: Bet) => {
          this.activeModal.close();
        },
        (error: any) => {
          // Handle error
          this.errorMessage = error.error;
          // console.log(this.errorMessage);
        }
      );

    }
  }
}