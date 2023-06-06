import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  private mounths = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  private days: number[] = [];
  private years: number[] = [];

  public selectedMounth: number = 0;
  public selectedDay: number = 0;

  private step = 1;

  constructor(private router: LocalRouter) {
    let date = new Date().getFullYear();
    for (var i = date; i > date - 106; i--) {
      this.years.push(i);
    }
    this.UpdateDaysByMounth(28);
  }

  public GetMounths() : number[] {
    return this.mounths;
  }

  public GetDays(): number[] {
    return this.days;
  }

  public GetYears(): number[] {
    return this.years;
  }

  public GetStep(): number {
    return this.step;
  }

  RedirectToAuthPage() {
    this.router.goToAuth();
  }

  onSelectedMounth(value: string) {
    this.selectedMounth = Number(value);
    var daysInThisMounth = this.GetDaysInMounth(this.selectedMounth);
    this.UpdateDaysByMounth(daysInThisMounth);

    if (this.selectedDay > daysInThisMounth) {
      this.selectedDay = 1;
    }
  }

  onSelectedDay(value: string) {
    this.selectedDay = Number(value);
  }

  private GetDaysInMounth(mounth: number) : number {
   return new Date(new Date().getFullYear(), mounth, 0).getDate();
  }

  private UpdateDaysByMounth(days : number) {
    this.days = [];
    for (var i = 1; i < days + 1; i++) {
      this.days.push(i);
    }
  }

  public getNextStep() {
    this.step = 2;
  }

  public GetPriviousStep() {
    this.step = 1;
  }
}
