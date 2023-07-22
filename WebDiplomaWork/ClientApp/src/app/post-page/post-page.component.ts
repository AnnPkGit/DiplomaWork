import { Component } from '@angular/core';
import { PostModel } from '../shared/models/postModel';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPAgeComponent {
  post: PostModel = {};

  constructor(private route: ActivatedRoute) {
    this.post = {
      avatar: '/Images/sampoAvatar.png',
      authorName: 'Sampoooo',
      authorLogin: '@SampooooKoskii',
      likesNumber: 2201,
      commentsNumber: 58,
      imgs: ['/Images/sampo1.png','/Images/sampo2.png','/Images/sampo3.png', '/Images/sampo4.png'],
    };

    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;
    console.log(lastSegment); 
  }
}
