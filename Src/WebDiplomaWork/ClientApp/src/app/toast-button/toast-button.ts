import { Component } from '@angular/core';

@Component({
  selector: 'toast-button',
  templateUrl: './toast-button.html',
})
export class ToastBtn {
    createToast: boolean = false;

    onBooleanEmitted(value: boolean) {
        this.createToast = value;
    }

    openModal() {
        this.createToast = true;
    }
}