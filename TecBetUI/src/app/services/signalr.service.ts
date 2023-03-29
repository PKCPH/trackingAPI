import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";

@Injectable({ providedIn: 'root' })
export class SignalrService {
    constructor(
    ){}

    hubConnection : signalR.HubConnection | any;

    startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl('https://localhost:5001/test', {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();

        this.hubConnection
        .start()
        .then(() => {
            console.log('Hub Connection Started!');
        })
        .catch((err: any) => console.log('Error while starting connection: ' + err))
    }


    askServer() {
        this.hubConnection.invoke("askServer", "hey")
            .catch((err: any) => console.error(err));
    }

    askServerListener() {
        this.hubConnection.on("askServerResponse", (someText: any) => {
            console.log(someText);
        })
    }
}