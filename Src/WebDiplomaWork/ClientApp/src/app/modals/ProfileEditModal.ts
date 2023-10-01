import { Component, EventEmitter, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-profile-edit-modal',
  templateUrl: './profile-edit-modal.html'
})
export class ProfileEditModal {
    @Output() booleanEmitter: EventEmitter<boolean> = new EventEmitter();
    close!: () => void;

    Close(event: Event) {
        //this.close();
        this.booleanEmitter.emit(false);
    }

    StopPropagation(event: Event) {
      event.stopPropagation();
    }

    myForm: FormGroup;

    constructor(private fb: FormBuilder) {
      this.myForm = this.fb.group({
        name: ['', [Validators.required, Validators.maxLength(10)]],
        bio: ['', [Validators.maxLength(50)]],
      });

      let date = new Date().getFullYear();
      for (var i = date; i > date - 106; i--) {
        this.years.push(i);
      }
      this.UpdateDaysByMounth(28);
    }
  
    onSubmit() {
      // Handle form submission here
    }
    

    private mounths = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
    private days: number[] = [];
    private years: number[] = [];
  
    public selectedMounth: number = 0;
    public selectedDay: number = 0;
    public selectedYear: number = 0;

    public GetMounths() : number[] {
      return this.mounths;
    }
  
    public GetDays(): number[] {
      return this.days;
    }
  
    public GetYears(): number[] {
      return this.years;
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
}