import { HttpClient, HttpHeaders } from '@angular/common/http';
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
  private createdUserId: number = 0;

  loginInput: string = '';
  emailInput: string = '';
  nameInput: string = '';

  public selectedMounth: number = 0;
  public selectedDay: number = 0;
  public selectedYear: number = 0;

  passwordInput: string = '';
  confirmPasswordInput: string = '';

  private step = 1;

  private userSuccessfulyCreated: boolean = false;

  constructor(private router: LocalRouter, private http: HttpClient) {
    let date = new Date().getFullYear();
    for (var i = date; i > date - 106; i--) {
      this.years.push(i);
    }
    this.UpdateDaysByMounth(28);
  }

  public onChangeEmailInput(newValue: string) {
    this.emailInput = newValue
  }

  public onChangeLoginInput(newValue: string) {
    this.loginInput = newValue;
  }

  onChangeNameInput(newValue: string) {
    this.nameInput = newValue;
  }

  onChangeConfirmPasswordInput(newValue: string) {
    this.confirmPasswordInput = newValue;
  }

  onChangePasswordInput(newValue: string) {
    this.passwordInput = newValue;
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

  onSelectedYear(value: string) {
    this.selectedYear = Number(value);
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
    if(!this.userSuccessfulyCreated) {
      return;
    }
    
    this.step = 2;
  }

  public GetPriviousStep() {
    this.step = 1;
  }

  public CreateUserRequest() {
    const createUserBody = {
      Email: this.emailInput,
      Password: this.passwordInput
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post<number>('api/v1/user', createUserBody, { headers }).subscribe(
      (result: number) => {
        console.log(result);
        this.userSuccessfulyCreated = true;
        this.createdUserId = result;
        this.getNextStep();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  public Register() {
    // const birthdate = new Date(this.selectedYear, this.selectedMounth - 1, this.selectedDay);

    const createAccountBody = {
      UserId: this.createdUserId,
      Login: this.loginInput,
      Name:  this.nameInput
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post('api/v1/account', createAccountBody, { headers }).subscribe(
      () => {
        this.router.goToSignIn();
      },
      (error) => {
      }
    );
  }
}
