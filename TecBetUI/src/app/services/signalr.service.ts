import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { ChartModel } from 'src/app/models/chart.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public data: ChartModel[] | any;
  private hubConnection: signalR.HubConnection | any

  constructor(private http: HttpClient) {}

    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:5001/chart')
                              .withAutomaticReconnect()
                              .configureLogging(signalR.LogLevel.Information)
                              .build();

                              this.hubConnection.onreconnected(() => {
                                this.http.get('https://localhost:5001/api/chart')
                                  .subscribe((res: any) => {
                                    console.log(res);
                                  })
                              })

      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch((err: any) => console.log('Error while starting connection: ' + err))
    }

    public closeConnection = () => {
      if (this.hubConnection) {
        this.hubConnection
          .stop()
          .then(() => console.log('Connection stopped'))
          .catch((err: any) => console.log('Error while stopping connection: ' + err));
      }
    }
    
    public addTransferChartDataListener = () => {
      this.hubConnection.on('transferchartdata', (data: any) => {
        this.data = data;
        console.log(data);
      });
    }

    // public getAllTeams(): Observable<Team[]> {
    //   this.isLoading = true;
    //   return this.http.get<Team[]>(serviceVariables.baseApiUrl + '/api/Team')
    //         .pipe(
    //           tap(teams => {
    //             this.errorSubject.next('');
    //           }),
    //           catchError(error => {
    //             this.errorSubject.next(this.customErrorHandlerService.handleError(error));
    //             this.isLoading = false;
    //             return of([]);
    //           })
    //         );
    // }

}