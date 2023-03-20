import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Bet } from 'src/app/models/bet.model';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { BetService } from 'src/app/services/bet.service';
import { Location } from '@angular/common';
import { Subscription, interval, switchMap } from 'rxjs';

@Component({
  selector: 'app-userbets',
  templateUrl: './userbets.component.html',
  styleUrls: ['./userbets.component.css']
})
export class UserbetsComponent implements OnDestroy {

  credentials: LoginModel = { userName: '', password: '', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: '' };
  bets: Bet[] = [];
  updateSubscription: Subscription | any;

  constructor(private route: ActivatedRoute, private authService: AuthguardService, private betService: BetService, private _location: Location) {

    //Regular fetch first so we can get the data instantaneously
    //Then we wrap the fetch function around with a updateSubscription which will run the function every 2.5 seconds (2500 ms)

    this.fetch();

    this.updateSubscription = interval(2500).subscribe(() => {
      this.fetch();
    });

  }

  //Go back function that takes advantage of the built-in Location service that Angular provides to send you back one page from your current location / Kinda replicates going back one in your history.

  goBack() {
    this._location.back();
  }

  //To not have multiple subscriptions ongoing at the same time, this function is used to unsubscribe, when a component "gets destroyed" - same as navigating away from that page.

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

//This fetch functions, first grabs a parameter from the route (url), in this case 'username', and then we use that to get a specific user by username
//It'll populate the credentials model with the properties we specify.
//We need the ID property of the specific user, so we can parse it into the getUserBets function
//Which will populate our bets model.

  fetch() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const userName = params.get('username');
        if (userName) {
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
                          matchId: bets.matchId.substring(0, 8),
                          participatingTeams: bets.participatingTeams.map(team => ({...team, name: team.name.substring(0, 10)})),
                        }
                      });
                      // console.log(this.bets);
                    }
                  });
              }
            });
        }
      }
    })
  }

}
