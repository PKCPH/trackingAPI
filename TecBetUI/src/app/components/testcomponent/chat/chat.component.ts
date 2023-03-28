import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnDestroy {
  constructor(public signalRService: SignalrService, private http: HttpClient) { }
  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();   
    this.startHttpRequest();
  }
  
  private startHttpRequest = () => {
    this.http.get('https://localhost:5001/api/chart')
      .subscribe(res => {
        console.log(res);
      })
  }

  ngOnDestroy(): void {
    // this.signalRService.closeConnection();
  }
}