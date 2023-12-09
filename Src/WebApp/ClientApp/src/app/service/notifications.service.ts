import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject, interval } from 'rxjs';
import { ToastItem, UserFollower } from '../profile-page/profile.component';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})

export class NotificationService {
  private anyNotViewedNots = new Subject<boolean>();
  private anyNotViewedNotsBool: boolean = false;
  private hubConnection: signalR.HubConnection | undefined;
  private reactionNotificationSubject: Subject<ReactionNotification[]> = new Subject<ReactionNotification[]>();
  private data: ReactionNotification[] = [];
  intervalTime = 4000; 
  mostRecentDate: string | null | undefined;

  constructor() {
    this.startSignlaRConnection();
  }

  stopConnection = () => {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  };

  public startSignlaRConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl( `/sync/notification`, {
        transport: signalR.HttpTransportType.ServerSentEvents
      })
      .build();
    this.hubConnection
      .start()
      .then(() => {})
      .catch(err => console.log('Error while starting connection: ' + err));

      this.hubConnection.on('Receive', (data: ReactionNotification[]) => {

        if(data.toString() == 'OK') {
          return;
        }
        
        this.data = [...data, ...this.data];
        this.reactionNotificationSubject.next(this.data);
        
        if(this.data.some(item => item.viewed == null)) {
          this.anyNotViewedNotsBool = true;
          this.anyNotViewedNots.next(this.anyNotViewedNotsBool);
        }
        else {
          this.anyNotViewedNotsBool = false;
          this.anyNotViewedNots.next(this.anyNotViewedNotsBool);
        }
      });

      interval(this.intervalTime)
      .subscribe(() => {
        var date = this.data.reduce((maxDate: string | null, notification: ReactionNotification) => {
          const notificationDate = new Date(notification.created);
          if (maxDate === null || notificationDate > new Date(maxDate)) {
            return notification.created;
          } else {
            return maxDate;
          }
        }, null);
        
        if( date?.toString() != this.mostRecentDate) {
          this.mostRecentDate = date?.toString();
        }

        const query: GetCurrentAccountNotificationsByTimeQuery = {
          time: this.mostRecentDate
        };

        if(this.hubConnection?.state != signalR.HubConnectionState.Connected) {
          return;
        }

        this.hubConnection?.invoke('Sync', query)
          .catch((error) => console.error('Error while invoking hub method:', error));
      });
  }

  getData(): Observable<ReactionNotification[]> {
    return this.reactionNotificationSubject.asObservable();
  }

  getNotsStatus(): Observable<boolean> {
    return this.anyNotViewedNots.asObservable();
  }

  getSimpleData() : ReactionNotification[] {
    return this.data;
  }

  getCurrentNotStatus() : boolean {
    return this.anyNotViewedNotsBool;
  }

  viewNot(id: number) {
    const command: ViewNotificationsCommand = {
      BaseNotificationIds: [id]
    };

    var updatedData = this.data.filter((element) => element.id !== id);
    
    if(updatedData .some(item => item.viewed == null)) {
      this.anyNotViewedNotsBool = true;
      this.anyNotViewedNots.next(this.anyNotViewedNotsBool);
    }
    else {
      this.anyNotViewedNotsBool = false;
      this.anyNotViewedNots.next(this.anyNotViewedNotsBool);
    }

    if(this.hubConnection?.state != signalR.HubConnectionState.Connected) {
      return;
    }

    this.hubConnection?.invoke('ViewNotifications', command)
          .catch((error) => console.error('Error while invoking hub method:', error));
  }
}

class ViewNotificationsCommand {
  BaseNotificationIds: number[] = [];
}

class  GetCurrentAccountNotificationsByTimeQuery {
  time: string | null | undefined;
}

export interface ReactionNotification {
  id: number;
  reaction: {
    id: number;
    author: UserFollower;
    toastWithContent: ToastItem
    // ... other properties
  };
  reply: ToastItem,
  quote: ToastItem,
  reToast: ToastItem,
  follower: UserFollower;
  created: string;
  lastModified: string;
  mediaItems: any[]; // Define the proper type for mediaItems
  quotedToast: ToastItem;
  quotesCount: number;
  reToastsCount: number;
  reactionsCount: number;
  repliesCount: number;
  type: string;
  youReToasted: boolean;
  youReacted: boolean;
  toAccount: {
    id: number;
    login: string;
    birthDate: string | null;
    name: string;
    avatar: string | null;
    bio: string | null;
    // ... other properties
  };
  viewed: string;
}
