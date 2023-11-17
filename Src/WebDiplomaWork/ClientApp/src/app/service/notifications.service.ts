import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, interval } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class NotificationService {
    private updateSubject: BehaviorSubject<void> = new BehaviorSubject<void>(undefined);

    constructor(private http: HttpClient) {
        // Run the update function every minute (adjust as needed)
        interval(60000).subscribe(() => this.update());
      }
    
      private update(): void {
        // Perform a GET request and emit the received data
        // this.http.get<any>('/your-api-endpoint').subscribe((data) => {
        //   this.updateSubject.next(data);
        // });
      }
    
      get onUpdate(): Observable<any> {
        return this.updateSubject.asObservable();
      }
}