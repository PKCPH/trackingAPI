import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-signal-r',
  templateUrl: './signal-r.component.html',
  styleUrls: ['./signal-r.component.css']
})
export class SignalRComponent implements OnInit, OnDestroy {
constructor(
  public signalrService: SignalrService
  ){}

  ngOnInit() {
    this.signalrService.startConnection();

    setTimeout(() => {
      this.signalrService.askServerListener();
      this.signalrService.askServer();
    }, 2000);
  }

  ngOnDestroy(): void {
    this.signalrService.hubConnection?.off("askServerResponse");
  }
}
