import { Component, OnInit } from '@angular/core';
import { MessageModel } from '../shared/models/messageModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ChatModel } from '../shared/models/ChatModel';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
})
export class MessagesComponent implements OnInit{
  messages: MessageModel[] | any;
  messageInput: string = '';
  addedPictures: string[] = [];
  chats: ChatModel[] = [];
  selectedChatId: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.messages = [ 
      {
        authorId: 'notMe',
        type: 'text',
        content: 'Morning. You look awfully drowsy, didnt you sleep well? Tsk-tsk, come on then, what is it? What mischief were you up to last night, hmm?',
        time: '12:41 May 2023'
      },
      {
        authorId: 'myId',
        type: 'img',
        content: 'https://pbs.twimg.com/media/FyNHpx6X0AsiK8A?format=jpg&name=large',
        time: '12:41 May 2023'
      },
      {
        authorId: 'notMe',
        type: 'text',
        content: 'Humans are just fascinating creatures, thats why Im so fond of them. They live such short lives, but for the time theyre around, they shine as bright as the midday sun. There are some whose light never grows dim, even over great stretches of time... These are the lives that make for real page-turners.',
        time: '12:41 May 2023'
      },
      {
        authorId: 'notMe',
        type: 'text',
        content: 'The moon is simply magnificent tonight. We mustnt squander it. Come, join me for a moonlight stroll — I wont take no for an answer.',
        time: '12:41 May 2023'
      },
      {
        authorId: 'notMe',
        type: 'img',
        content: 'https://pbs.twimg.com/media/FyMHvm5XsAIRAWX?format=jpg&name=large',
        time: '12:41 May 2023'
      },
      {
        authorId: 'myId',
        type: 'text',
        content: ' These Divine Priestesses seem to be getting cuter every generation. I wonder how she looks performing the Dance of Divine Peace... Perhaps even little schools of fish swish around her while she spins. If shes ever looking for a more experienced head shrine maiden to learn from, Id be quite happy to share a few tips with her... Hehe, as long as she asked me nicely, of course.',
        time: '12:41 May 2023'
      }
    ]

    this.chats = [{
      Id: '1',
      Avatar: '',
      Login: 'Zongli',
      Name: 'Name'
    },
    {
      Id: '2',
      Avatar: '',
      Login: 'Zongli2',
      Name: 'Name2'
    }]
  }

  sendMessage() {
    this.messages.push(      
      {
        authorId: 'myId',
        type: 'text',
        content: this.messageInput,
        time: new Date().toLocaleTimeString()
      });
      
    this.messageInput = '';
  }

  handleFileInput(event : any) {
    const file:File = event.target.files[0];

    console.log(file);
    if (file) {
        const formData = new FormData();

        formData.append("pic", file);

        this.http.post("/user/postpic", formData).subscribe(
          (response: any) => {
            this.addedPictures.push('/Images/' +  response['fileName']);
          },
          (error: any) => {
            console.error('Error:', error);
          }
        );
    }
  }

  removeAttachment() {
    this.addedPictures = [];
  }

  selectChat(Id : string) {
    this.selectedChatId = Id;
  }

  isChatSelected(Id : string) {
    return this.selectedChatId == Id;
  }
}
