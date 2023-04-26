import { Component, ElementRef, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription, interval, switchMap } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';
import { Match } from 'src/app/models/matches.model';
import { Team } from 'src/app/models/teams.model';
import { LoginService } from 'src/app/services/login.service';
import { MatchesService } from 'src/app/services/matches.service';

@Component({
  selector: 'app-betting',
  templateUrl: './betting.component.html',
  styleUrls: ['./betting.component.css']
})
export class BettingComponent {
  startTime: Date | any;
  stopwatch: string | any;
  matchDetails: Match | any;
}

