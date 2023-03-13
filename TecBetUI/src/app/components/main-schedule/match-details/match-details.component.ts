import { Component, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription, interval, switchMap } from 'rxjs';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { MatchesService } from 'src/app/services/matches.service';
import { BettingWindowComponent } from '../../betting/betting-window/betting-window.component';

@Component({
  selector: 'app-match-details',
  templateUrl: './match-details.component.html',
  styleUrls: ['./match-details.component.css']
})
export class MatchDetailsComponent implements OnDestroy {

  matchDetails: Match = {
    id: '',
    matchState: 0,
    dateOfMatch: new Date(),
    participatingTeams: [],
    state: ''
  };

  drawRequest: Team = {
    id: '',
    name: 'draw',
    isAvailable: true,
    matches: [],
    availability: '',
    score: 0,
    result: 0,
    players:[]
  };
  

  updateSubscription: Subscription;
  id: any;

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private route: ActivatedRoute, private matchesService: MatchesService, private router: Router, private modalService: NgbModal) {

    this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        if (this.id) {
          //Call API
          this.matchesService.getMatchDetails(this.id)
            .subscribe({
              next: (response) => {
                this.matchDetails = response;
                // console.log(this.matchDetails);
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
          // console.log(this.matchDetails);
          // console.log(this.games);
          if (this.matchDetails) {
            if (this.matchDetails.matchState == 2) {
              this.modalService.dismissAll(BettingWindowComponent);
            }
            // this.Hideloader();
          }
          this.matchesService.errorMessage.subscribe(error => {
            // this.errorMessage = error;
            if (this.matchDetails == null) {
              // this.errorMessage = "";
            }
          });
        },
        error: (response) => {
          console.log(response);
        }
      });



  }

  goBack() {
    this.router.navigateByUrl("/schedule")
  }

  onBetTeamA() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet'});
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.matchDetails.participatingTeams[0];
  }

  onBetTeamB() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet'});
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.matchDetails.participatingTeams[1];
  }

  onBetDraw() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet'});
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.drawRequest;
  }


}
