import { Component, OnInit } from '@angular/core';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { PostModel } from '../shared/models/postModel';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  constructor(private router: LocalRouter) {}
  posts?: PostModel[];

  ngOnInit(): void {
    console.log('a');
    this.posts = [];
    this.posts?.push({
      avatar: '/Images/childeAvatar.png',
      authorName: 'Childe',
      authorLogin: '@TheEleventhOfFatui',
      text: 'Comrade, how would you like to join the Fatui? ...Actually, scrap that. With your capabilities, you and I could head-to-head with the Harbingers and win. Wouldnt want to miss an opportunity like that.',
      imgs: ['/Images/childe.png'],
      likesNumber: 132,
      commentsNumber: 3
    });
    this.posts?.push({
      avatar: '/Images/yae.png',
      authorName: 'Yae Miko',
      authorLogin: '@FoxLady',
      text: `Even supernatural beings eventually pass away, including gods. It's a terrible shame when their tales are lost to time along with them, never to be remembered again. The problem is, only those with first-class writing and composition skills are qualified to commit these stories to paper. If there is no qualified writing talent in the world, we must cultivate it... Yes, home-grown writers are what we need, capable of adequately capturing these stories with the written word.`,
      likesNumber: 2201,
      commentsNumber: 58
    })
    this.posts?.push({
      avatar: '/Images/yae.png',
      authorName: 'Yae Miko',
      authorLogin: '@FoxLady',
      text: `Though I'm an avid reader of novels written by others, I've yet to pick up the pen myself. Writing a novel is quite an undertaking, after all. But, if there comes a day when you reach the end of your adventure and want somebody to record it — and if I happen to have the time when that day comes... We'll see.`,
      likesNumber: 2201,
      commentsNumber: 58,
      imgs: ['/Images/inazuma1.png','/Images/inazuma2.png','/Images/inazuma3.png'],
    })
    this.posts?.push({
      avatar: '/Images/sampoAvatar.png',
      authorName: 'Sampoooo',
      authorLogin: '@SampooooKoskii',
      likesNumber: 2201,
      commentsNumber: 58,
      imgs: ['/Images/sampo1.png','/Images/sampo2.png','/Images/sampo3.png', '/Images/sampo4.png'],
    })
  }

  Reload() {
    window.location.reload();
  }
}
