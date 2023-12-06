import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | undefined;

  startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('your-signalr-hub-url') // Replace with your SignalR hub URL
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  stopConnection = () => {
    this.hubConnection?.stop();
  };

  // Add methods to handle SignalR events or send messages
}