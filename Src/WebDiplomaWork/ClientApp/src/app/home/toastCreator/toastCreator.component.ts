import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserResponse } from 'src/app/identification/signIn/signIn.component';

@Component({
  selector: 'app-toast-creator',
  templateUrl: './toastCreator.component.html',
})
export class ToastCreatorComponent implements OnInit{

  public user: UserResponse | undefined;
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
}
