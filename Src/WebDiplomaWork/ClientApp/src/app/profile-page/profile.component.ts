import { Component, OnInit } from '@angular/core';
import { PostModel } from '../shared/models/postModel';
import { AccountModel } from '../shared/models/accountModel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfilePageComponent implements OnInit{
  posts?: PostModel[];
  account?: AccountModel;

  ngOnInit(): void {
    this.posts = [];
    this.posts?.push({
      id: '1',
      avatar: '/Images/childeAvatar.png',
      authorName: 'Childe',
      authorLogin: '@TheEleventhOfFatui',
      text: 'Comrade, how would you like to join the Fatui? ...Actually, scrap that. With your capabilities, you and I could head-to-head with the Harbingers and win. Wouldnt want to miss an opportunity like that.',
      imgs: ['/Images/childe.png'],
      likesNumber: 132,
      commentsNumber: 3
    });
    this.posts?.push({
      id: '2',
      avatar: '/Images/yae.png',
      authorName: 'Yae Miko',
      authorLogin: '@FoxLady',
      text: `Even supernatural beings eventually pass away, including gods. It's a terrible shame when their tales are lost to time along with them, never to be remembered again. The problem is, only those with first-class writing and composition skills are qualified to commit these stories to paper. If there is no qualified writing talent in the world, we must cultivate it... Yes, home-grown writers are what we need, capable of adequately capturing these stories with the written word.`,
      likesNumber: 2201,
      commentsNumber: 58
    })

    this.account = {
      userName: 'Tory',
      userLogin: 'Tory',
      followersCount: 1,
      followsCount: 2,
      joinDate: '03.09.2023',
      bio: 'Hi my name is Tory!!!'
    }
  }

  Reload() {
    window.location.reload();
  }
}
