import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserResponse } from 'src/app/identification/signIn/signIn.component';
import { ToastItem } from 'src/app/profile-page/profile.component';

@Component({
  selector: 'app-toast-creator',
  templateUrl: './toastCreator.component.html',
})
export class ToastCreatorComponent implements OnInit{

  public user: UserResponse | undefined;

  input : string = '';

  constructor(private http: HttpClient) {

  }

  ngOnInit(): void { 
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }

  autoGrow() {
    var textarea = document.getElementById('text-area');
    textarea = textarea ?? new HTMLElement();
    const div = textarea.parentElement ?? new HTMLElement();
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
    div.style.height = textarea.style.height;
  }

  PostContent() {
    var body = {
        Context: this.input,
        ToastMediaItemIds: []
      };
    var url = "/api/v1/toast";

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post<ToastItem>(url, body, { headers }).subscribe(
      (res: ToastItem) => {
        if(res) {
          this.input = '';
        }
      },
      (error) => {
      }
    );
  }
}