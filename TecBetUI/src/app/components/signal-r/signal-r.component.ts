import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';
import { HubConnectionBuilder} from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
.withUrl('/schedule')
.build();

@Component({
  selector: 'app-signal-r',
  templateUrl: './signal-r.component.html',
  styleUrls: ['./signal-r.component.css']
})
export class SignalRComponent implements OnInit, OnDestroy {
  leagues: Leagues[] = [];
  counter: any;
constructor(
  public signalrService: SignalrService,
  private leaguesService: LeaguesService){
    this.leaguesService.getAllLeagues()
    .subscribe({
      next: (leagues) => {
        this.leagues = leagues
      },
      error: (response) => {
        console.log(response);
    }
  })

  }

///update counter does not work
///try setting up connection with a different hub on test page

  ngOnInit() {

    connection.start();
    connection.on('updateCounter', (counter : any) => {
      this.counter = counter;
    })
    // this.signalrService.startConnection();

    // setTimeout(() => {
    //   this.signalrService.askServerListener();
    //   this.signalrService.askServer();
    // }, 2000);
  }

  ngOnDestroy(): void {
    this.signalrService.hubConnection?.off("askServerResponse");
  }
}
