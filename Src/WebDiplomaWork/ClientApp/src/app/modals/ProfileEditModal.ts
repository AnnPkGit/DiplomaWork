import { Component, EventEmitter, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { LocalRouter } from "../shared/localRouter/local-router.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UserResponse } from "../identification/signIn/signIn.component";
import { ImageItem } from "../toast-modal/toast-modal";

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
    public userCurrent: UserResponse = {} as UserResponse;

    constructor(private fb: FormBuilder, private localRouter: LocalRouter, private http: HttpClient) {
      this.myForm = this.fb.group({
        name: ['', [Validators.required, Validators.maxLength(10)]],
        bio: ['', [Validators.maxLength(50)]],
      });

      let date = new Date().getFullYear();
      for (var i = date; i > date - 106; i--) {
        this.years.push(i);
      }
      this.UpdateDaysByMounth(28);

      this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");

      this.myForm.patchValue({
        name: this.userCurrent?.account.name,
        bio: this.userCurrent?.account.bio
      });
    }
  
    onSubmit() {
      const formValues = this.myForm.value;

      const body = {
        Name: formValues.name,
        Bio: formValues.bio
      };
      const headers = new HttpHeaders({
        'Content-Type': 'application/json' 
      });
      this.http.patch("/api/v1/account", body, { headers }).subscribe(
        () => {
          this.booleanEmitter.emit(false);

          this.userCurrent.account.bio = formValues.bio ?? this.userCurrent.account.bio;
          this.userCurrent.account.name= formValues.name ?? this.userCurrent.account.name;

          localStorage.setItem("userInfo", JSON.stringify(this.userCurrent));
        }
        ,
        (error) => {
        }
        );
    }

    selectedFile: File = {} as File;

    onFileChange(event: any): void {
      console.log("avatarUpload")
      this.selectedFile = FileList = event.target.files[0];
      this.uploadFile();
    }

  
    uploadFile(): void {
      const formData = new FormData();
      formData.append('file', this.selectedFile);
  
      this.http.post<ImageItem>('/api/v1/MediaItem/avatar', formData).subscribe(
        (response) => {
          console.log('File uploaded successfully:', response);
          this.userCurrent.account.avatar = response;

          const body = {
            AvatarId: response.id
          };

          const headers = new HttpHeaders({
            'Content-Type': 'application/json' 
          });

          this.http.patch('/api/v1/account', body, {headers}).subscribe();
        },
        (error) => {
          console.error('Error uploading file:', error);
        }
      );
    }

    onFileChangeBanner(event: any): void {
      console.log("avatarUpload")
      this.selectedFile = FileList = event.target.files[0];
      this.uploadBanner();
    }

    uploadBanner(): void {
      const formData = new FormData();
      formData.append('file', this.selectedFile);
  
      this.http.post<ImageItem>('/api/v1/MediaItem/banner', formData).subscribe(
        (response) => {
          console.log('File uploaded successfully:', response);
          this.userCurrent.account.banner = response;

          const body = {
            BannerId: response.id
          };

          const headers = new HttpHeaders({
            'Content-Type': 'application/json' 
          });

          this.http.patch('/api/v1/account', body, {headers}).subscribe();
        },
        (error) => {
          console.error('Error uploading file:', error);
        }
      );
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

    goToAdvncedSettings() {
      this.localRouter.goToSettings();
    }
}