import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import {AuthService} from "../../shared/services/auth.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  private months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  private days: number[] = [];
  private years: number[] = [];

  loginInput: string = '';
  emailInput: string = '';

  public selectedMonth: number = 0;
  public selectedDay: number = 0;
  public selectedYear: number = 0;

  passwordInput: string = '';
  confirmPasswordInput: string = '';

  private step = 1;

  constructor(
    private router: LocalRouter,
    private http: HttpClient,
    private authService: AuthService)
  {
    let date = new Date().getFullYear();
    for (var i = date; i > date - 106; i--) {
      this.years.push(i);
    }
    this.UpdateDaysByMonth(28);
  }

  public onChangeEmailInput(newValue: string) {
    this.emailInput = newValue
  }

  public onChangeLoginInput(newValue: string) {
    this.loginInput = newValue;
  }

  onChangeConfirmPasswordInput(newValue: string) {
    this.confirmPasswordInput = newValue;
  }

  onChangePasswordInput(newValue: string) {
    this.passwordInput = newValue;
  }

  public GetMonths() : number[] {
    return this.months;
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

  onSelectedMonth(value: string) {
    this.selectedMonth = Number(value);
    var daysInThisMonth = this.GetDaysInMonth(this.selectedMonth);
    this.UpdateDaysByMonth(daysInThisMonth);

    if (this.selectedDay > daysInThisMonth) {
      this.selectedDay = 1;
    }
  }

  onSelectedDay(value: string) {
    this.selectedDay = Number(value);
  }

  onSelectedYear(value: string) {
    this.selectedYear = Number(value);
  }

  private GetDaysInMonth(month: number) : number {
   return new Date(new Date().getFullYear(), month, 0).getDate();
  }

  private UpdateDaysByMonth(days : number) {
    this.days = [];
    for (var i = 1; i < days + 1; i++) {
      this.days.push(i);
    }
  }

  public getNextStep() {
    this.step = 2;
  }

  public GetPreviousStep() {
    this.step = 1;
  }

  public Register(){
    this.Register1()
    this.router.goToHome()
  }

  private Register1() {
    this.RegisterUser().add(() => {
      this.authService.Authorize({
        Email: this.emailInput,
        Password: this.passwordInput
      }).add(() => {
        const accessToken = this.authService.GetTokens().accessToken;
        this.RegisterAccount(accessToken);
      })
    });
  }

  private RegisterAccount(accessToken: string) {
    const birthdate = new Date(this.selectedYear, this.selectedMonth - 1, this.selectedDay);

    const body = {
      Login: this.loginInput,
      BirthDate: birthdate.toISOString()
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${accessToken}`
    });
    return this.http.post('api/v1/account', body, { headers }).subscribe(
      (value: any) => {
        console.log('Registration successful');
      },
      (error) => {
        console.error('Error during account registration:', error);
      }
    );
  }
  private RegisterUser() {
    const body = {
      Email: this.emailInput,
      Password: this.passwordInput,
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post('api/v1/user', body, { headers }).subscribe(
      () => {
      },
      (error) => {
        console.error('Error during user registration:', error);
      }
    );
  }
}
