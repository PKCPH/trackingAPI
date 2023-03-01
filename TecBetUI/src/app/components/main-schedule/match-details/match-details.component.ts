import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, interval, switchMap } from 'rxjs';
import { Match } from 'src/app/models/matches.model';
import { MatchesService } from 'src/app/services/matches.service';

@Component({
  selector: 'app-match-details',
  templateUrl: './match-details.component.html',
  styleUrls: ['./match-details.component.css']
})
export class MatchDetailsComponent {

  matchDetails: Match = {
    id: '',
    teamAScore: 0,
    teamBScore: 0,
    matchState: 0,
    dateOfMatch: new Date(),
    participatingTeams: [],
    state: ''
  };

  updateSubscription: Subscription;
  id: any;

  constructor(private route: ActivatedRoute, private matchesService: MatchesService) {

    this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        if (this.id) {
          //Call API
this.matchesService.getMatchDetails(this.id)
.subscribe({
  next: (response) => {
this.matchDetails = response;
console.log(this.matchDetails);
  }
});       
        }
      }
    })

    this.updateSubscription = interval(2500).pipe(
      switchMap(() => this.matchesService.getMatchDetails(this.id))
    )
    .subscribe({
      next: (response) => {
        this.matchDetails = response
        console.log(this.matchDetails);
        // console.log(this.games);
        if (this.matchDetails)
        {
          // this.Hideloader();
        }
        this.matchesService.errorMessage.subscribe(error => {
          // this.errorMessage = error;
          if (this.matchDetails == null)
          {
            // this.errorMessage = "";
          }
        });
      },
      error: (response) => {
        console.log(response);
      }
    });   



  }

  
}
