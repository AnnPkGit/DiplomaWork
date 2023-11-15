   import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ImageItem } from 'src/app/toast-modal/toast-modal';

@Component({
  selector: 'app-img-opened',
  templateUrl: './imgOpened.component.html',
})
export class ImgOpened {
  @Input() 
  imgs: string[] | any;
  @Input() 
  currentImg: string | undefined;
  @Input()
  shouldBeHidden: boolean | undefined;
  @Output() shouldBeHiddenChange = new EventEmitter<boolean>();

  hide() {
    this.shouldBeHidden = true;
    this.shouldBeHiddenChange.emit(this.shouldBeHidden);
  }

  showBackArrow() : boolean {
    var currentIndex = this.imgs?.indexOf(this.currentImg ?? '') ?? 0;
    if(currentIndex == 0){
      return false;
    }
    return true;
  }
  
  showForwardArrow() : boolean {
    var currentIndex = this.imgs?.indexOf(this.currentImg ?? '') ?? 0;
    if(currentIndex == this.imgs.length - 1){
      return false;
    }
    return true;
  }

  back(event: Event) {
    event.stopPropagation();
    var currentIndex = this.imgs?.indexOf(this.currentImg ?? '') ?? 0;
    if(currentIndex !== -1 && currentIndex !== 0) {
      this.currentImg = this.imgs[currentIndex - 1];
    }
  }

  forward(event: Event) {
    event.stopPropagation();
    var currentIndex = this.imgs?.indexOf(this.currentImg ?? '') ?? 0;
    if(currentIndex !== -1 && currentIndex !== this.imgs.length - 1) {
      this.currentImg = this.imgs[currentIndex + 1];
    }
  }
}
