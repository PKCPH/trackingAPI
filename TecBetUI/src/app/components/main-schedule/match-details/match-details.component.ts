import { Component, ElementRef, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription, interval, switchMap } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { LoginService } from 'src/app/services/login.service';
import { MatchesService } from 'src/app/services/matches.service';
import { BettingWindowComponent } from '../../betting/betting-window/betting-window.component';

@Component({
  selector: 'app-match-details',
  templateUrl: './match-details.component.html',
  styleUrls: ['./match-details.component.css']
})
export class MatchDetailsComponent implements OnInit, OnDestroy {
  startTime: Date | any;
  stopwatch: string | any;
  matchDetails: Match | any;
  credentials: LoginModel | any;
  storedCredentialsString: any;
  role: any;
  errorMessage: string = "";
  drawRequest: Team = {
    id: '',
    name: 'draw',
    isAvailable: true,
    matches: [],
    availability: '',
    score: 0,
    result: 0,
    players:[],
    rating: 0,
  };


  updateSubscription: Subscription;
  id: any;

  ngOnInit(): void {
    // set the starting datetime
    //this.startTime = new Date('2023-04-24T11:00:00Z');
    this.startTime = new Date(this.matchDetails.dateOfMatch);
    console.log("test time: " + this.startTime)

    // update the stopwatch every 10 milliseconds
    setInterval(() => {
      // get the current time
      const now = new Date();

      // calculate the elapsed time in milliseconds
      const elapsedTime = now.getTime() - this.startTime.getTime();

      // convert the elapsed time to minutes, seconds, and milliseconds
      const minutes = Math.floor(elapsedTime / 60000);
      const seconds = Math.floor((elapsedTime % 60000) / 1000);
      const milliseconds = elapsedTime % 1000;

      // format the time as a string
      this.stopwatch = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    }, 10); // update the stopwatch every 10 milliseconds
  }

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private route: ActivatedRoute, private matchesService: MatchesService, private router: Router,
    private modalService: NgbModal, private loginService: LoginService,
    private el: ElementRef, private renderer: Renderer2) {

    this.getCredentials();
    window.addEventListener('userLoggedIn', this.getCredentials.bind(this));
    this.getId();

    this.fetch();

    this.updateSubscription = interval(1500).subscribe(() => {
      this.fetch();
    });

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    });



  }

  fetch() {
    this.matchesService.getMatchDetails(this.id).subscribe({
      next: (response) => {
        this.matchDetails = response;
        console.log(this.matchDetails);

        // Set the start time after the match details have been fetched
        this.startTime = new Date(this.matchDetails.dateOfMatch);

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

  getId() {
    this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
      }
    })
  }

  goBack() {
    this.router.navigateByUrl("/leagues")
  }

  onBetTeamA() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet' });
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.matchDetails.participatingTeams[0];
  }

  onBetTeamB() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet' });
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.matchDetails.participatingTeams[1];
  }

  onBetDraw() {
    const modalRef = this.modalService.open(BettingWindowComponent, { centered: true, windowClass: 'modal-bet' });
    modalRef.componentInstance.match = this.matchDetails;
    modalRef.componentInstance.team = this.drawRequest;
  }

  //Simple function to read credentials in localstorage, and then if the contents exist, parse the array with the help of JSON and then set my "role" variable to the value of the .role property in localstorage
  getCredentials() {
    let storedCredentials;

    this.storedCredentialsString = localStorage.getItem("credentials");
    if (this.storedCredentialsString) {
      storedCredentials = JSON.parse(this.storedCredentialsString);
      this.role = storedCredentials.role
    }

  }



}
