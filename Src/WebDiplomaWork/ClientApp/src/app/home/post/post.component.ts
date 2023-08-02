import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent {
  @Input()
  customTitle: string | undefined;
}
