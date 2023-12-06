import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { ToastItem } from '../profile-page/profile.component';

@Injectable({
  providedIn: 'root',
})

export class NotificationService {
  private hubConnection: signalR.HubConnection | undefined;
  private reactionNotificationSubject: Subject<ReactionNotification> = new Subject<ReactionNotification>();

  reactionNotification$ = this.reactionNotificationSubject.asObservable();

  constructor() {
    this.startSignlaRConnection();
  }

  stopConnection = () => {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  };

  startSignlaRConnection(): void {
    // this.hubConnection = new signalR.HubConnectionBuilder()
    //   .withUrl('http://localhost:5031/sync/notification', {
    //     skipNegotiation: true,
    //     transport: signalR.HttpTransportType.WebSockets
    //   })
    //   .build();
    // this.hubConnection
    //   .start()
    //   .then(() => console.log('Connection started'))
    //   .catch(err => console.log('Error while starting connection: ' + err));

      // this.hubConnection.on('Sync', x => x(data))
  }
}

export interface ReactionNotification {
  items: {
    reaction: {
      author: {
        id: number;
        login: string;
        birthDate: string | null;
        name: string;
        avatar: string | null;
        bio: string | null;
      };
      toastWithContent: ToastItem
    };
    id: number;
    toAccount: {
      id: number;
      login: string;
      birthDate: string | null;
      name: string;
      avatar: string | null;
      bio: string | null;
    };
    type: string;
    created: string;
    viewed: string | null;
  }[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}