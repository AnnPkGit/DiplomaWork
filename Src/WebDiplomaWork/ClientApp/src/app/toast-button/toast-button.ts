import { Component, EventEmitter, Output } from '@angular/core';
import { ToastItem } from '../profile-page/profile.component';

@Component({
  selector: 'toast-button',
  templateUrl: './toast-button.html',
})
export class ToastBtn {
    createToast: boolean = false;

    @Output() onToastCreation: EventEmitter<ToastItem> = new EventEmitter();
    
    onBooleanEmitted(value: boolean) {
        this.createToast = value;
    }

    openModal() {
        this.createToast = true;
    }

    addToast($event : ToastItem): void {
        console.log($event);
        this.onToastCreation.emit($event);
    }
}