import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject, interval, merge } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { UserFollower } from '../profile-page/profile.component';

@Injectable({
  providedIn: 'root',
})
export class FollowRecommendationsService {
  private recommendationsSubject = new Subject<UserFollower[]>();
  private refreshIntervalMillis = 3 * 60 * 1000;
  private currentData: UserFollower[] = [];

  constructor(private http: HttpClient) {
    // Combine an immediate HTTP request with the periodic interval
    merge(
      this.http.get<UserFollower[]>('/api/v1/follow/recommendations'),
      interval(this.refreshIntervalMillis).pipe(
        switchMap(() => this.http.get<UserFollower[]>('/api/v1/follow/recommendations'))
      )
    ).subscribe(
      (data) => {
        this.currentData = data;
        this.recommendationsSubject.next(data);
      },
      (error) => {
        console.error('Error fetching recommendations:', error);
      }
    );
  }

  getRecommendations() : UserFollower[] {
    return this.currentData;
  }

  getRecommendationsObservable(): Observable<UserFollower[]> {
    return this.recommendationsSubject.asObservable();
  }

  Follow(id: number) {
    const updatedData = this.currentData.map((item) => {
        if (item.id === id) {
          return { ...item, youFollow: true };
        }
        return item;
    });
  
    this.currentData = updatedData;

    this.recommendationsSubject.next(this.currentData);

    const body = {
      AccountId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post("/api/v1/follow", body, { headers }).subscribe(
      () => {
      }
      ,
      (error) => {
      }
      );
  }

  UnFollow(id: number) {
    const updatedData = this.currentData.map((item) => {
        if (item.id === id) {
          return { ...item, youFollow: false };
        }
        return item;
    });
  
    this.currentData = updatedData;

    this.recommendationsSubject.next(this.currentData);

    const body = {
      FollowingId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.delete("/api/v1/follow", { body: body, headers }).subscribe(
      () => {
      }
      ,
      (error) => {
      }
      );
  }
}