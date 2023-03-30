import { ThisReceiver } from "@angular/compiler";
import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";

@Injectable({ providedIn: 'root' })
export class SignalrService {
    constructor(
    ){}

    hubConnection : signalR.HubConnection | any;

    startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withAutomaticReconnect()
        .withUrl('https://localhost:5001/Matches', {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();

        this.hubConnection
        .start()
        .then(() => {
            console.log('Hub Connection Started! sgnalR');
        })
        .catch((err: any) => console.log('Error while starting connection: ' + err))
    }

    // UpdateMatches = () => {
    //     this.hubConnection.on('GET', (response) => {
    //         this.data = response;
    //     })
    // }

    askServer() {
        this.hubConnection.invoke("askServer", "hey")
            .catch((err: any) => console.error(err));
    }

    askServerListener() {
        this.hubConnection.on("askServerResponse", (someText: any) => {
            console.log(someText);
        })
    }

    matchUpdatedResponse() {
        this.hubConnection.on("matchUpdatedResponse", (someText: any) => {
            console.log(someText);
        })
    }

    newWindowLoaded() {
        this.hubConnection.on("NewWindowLoaded", (value: any) => {
            var newCountSpan = document.getElementById("totalViewsCounter");
            if (newCountSpan) {
                newCountSpan.innerText = value.toString();
            }
        })
    }


    // newWindowLoadedOnClient(){
    //     this.newWindowLoaded().send
    // }
}