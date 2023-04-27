import { Component, ElementRef, Input, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Match } from 'src/app/models/matches.model';
import { timer, Subscription, interval } from "rxjs";
import { MatchesService } from 'src/app/services/matches.service';

@Component({
  selector: 'app-betting',
  templateUrl: './betting.component.html',
  styleUrls: ['./betting.component.css']
})
export class BettingComponent implements OnInit {
  startTime: Date | any;
  stopwatch: string | any;
  matchDetails: Match | any;
  isRunning: true | any;
  stopwatchInterval: any;
  team1: string | undefined;
  team2: string | undefined;
  score1: any;
  score2: any;
  teamNames: string[] = [
    "Liverpool",
    "West Ham United",
    "Manchester United",
    "Chelsea",
    "Manchester City",
    "Southampton",
    "Juventus",
    "Ajax"
  ];

  ngOnInit(): void {
    this.matchStopwatch();
    this.selectDemoTeams();
    this.selectDemoScore();
    window.addEventListener('scroll', () => {
      const scrollPosition = window.scrollY;

      if (scrollPosition >= 500) {
        this.slideInDiv.nativeElement.classList.add('slide-in-active');
      }
    });
  }

  @ViewChild('slideInDiv') slideInDiv!: ElementRef;

  selectDemoTeams() {
    const randomIndex1 = Math.floor(Math.random() * this.teamNames.length);
    let randomIndex2 = Math.floor(Math.random() * this.teamNames.length);

    // if they are the same teams
    if (randomIndex1 === randomIndex2) {
      randomIndex2 = (randomIndex2 + 1) % this.teamNames.length;
    }

    this.team1 = this.teamNames[randomIndex1];
    this.team2 = this.teamNames[randomIndex2];
  }

  selectDemoScore() {
    const randomScoreA = Math.floor(Math.random() * 2);
    const randomScoreB = Math.floor(Math.random() * 2);
    this.score1 = randomScoreA;
    this.score2 = randomScoreB;
  }

  matchStopwatch() {
    const randomTime = Math.floor(Math.random() * 5400);

    // set the start time to the current time minus the random time
    this.startTime = new Date(Date.now() - randomTime * 1000);

    const maxDuration = 90 * 60 * 1000;

    this.stopwatchInterval = setInterval(() => {
      const now = new Date();
      const elapsedTime = now.getTime() - this.startTime.getTime();

      // if the elapsed time exceeds the maximum duration, stop the stopwatch
      if (elapsedTime >= maxDuration) {
        clearInterval(this.stopwatchInterval);
        this.isRunning = false;
        this.stopwatch = "00:00";
        return;
      }

      // convert the elapsed time to minutes and seconds
      const minutes = Math.floor(elapsedTime / 60000);
      const seconds = Math.floor((elapsedTime % 60000) / 1000);

      // format the time as a string
      this.stopwatch = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    }, 10);

    this.isRunning = true;
  }
}
