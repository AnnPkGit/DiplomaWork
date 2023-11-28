import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, interval } from 'rxjs';
import { ToastItem } from '../profile-page/profile.component';

@Injectable({
  providedIn: 'root',
})

export class NotificationService {
  private updateSubject: BehaviorSubject<ReactionNotification | undefined> = new BehaviorSubject<ReactionNotification | undefined>(undefined);

  private reactionNotification: ReactionNotification | any;

    constructor(private http: HttpClient) {
        // Run the update function every minute (adjust as needed)
        interval(3000).subscribe(() => this.update());
      }
    
      private update(): void {
        if(!localStorage.getItem("latestNotificationDateReceived")) {
          // Perform a GET request and emit the received data
          this.http.get<ReactionNotification>('/api/v1/notification/by/current/account').subscribe((data) => {
            this.reactionNotification = data;
            this.updateSubject.next(data);
          });
        }
      }
    
      get onUpdate(): Observable<any> {
        return this.updateSubject.asObservable();
      }
      
      public getNotifications() : ReactionNotification {
        return this.reactionNotification;
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