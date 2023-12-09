import { Component, OnInit } from '@angular/core';
import { NotificationService, ReactionNotification } from '../service/notifications.service';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { ToastItem } from '../profile-page/profile.component';

@Component({
  selector: 'notifactions-page-home',
  templateUrl: './notifactions-page.html',
})
export class NotifactionsPageComponent implements OnInit {

  constructor(private notService: NotificationService, private localRouter: LocalRouter) {
  }

  notifications: ReactionNotification[] = [];

  ngOnInit(): void {
    this.notifications = this.notService.getSimpleData();
    this.notService.getData().subscribe((data) => {
      this.notifications = data;
    });
  }

  beautifyDate(date: Date | string | undefined): string {
    if (!date) {
        return '';
    }

    const inputDate = typeof date === 'string' ? new Date(date) : date;

    if (isNaN(inputDate.getTime())) {
        return '';
    }

    const today = new Date();

    if (inputDate.toDateString() === today.toDateString()) {
        const timeOptions: Intl.DateTimeFormatOptions = {
            hour: 'numeric',
            minute: 'numeric',
            hour12: false,
        };

        return new Intl.DateTimeFormat(undefined, timeOptions).format(inputDate);
        // Use undefined for the locale to use the user's default locale
    } else {
        const options: Intl.DateTimeFormatOptions = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            hour12: false,
        };

        return new Intl.DateTimeFormat(undefined, options).format(inputDate);
        // Use undefined for the locale to use the user's default locale
    }
  }

  GetReactionType(rec: ReactionNotification) : string {
    if(rec.follower) {
      return 'NEW FOLLOWER';
    }
    if(rec.reaction) {
      return 'REACTION';
    }
    if(rec.reToast) {
      return 'ReTOAST';
    }
    if(rec.quote) {
      return 'QUOTE';
    }
    if(rec.reply) {
      return 'REPLY';
    }
    return '-';
  }

  getUrlFoItem(item: ToastItem) : string {
    if(item?.quotedToast?.mediaItems) {
      return item?.quotedToast?.mediaItems[0]?.url ?? '';
    }
    return '';
  }

  getStyle(rec: ReactionNotification) {
    if(rec.viewed) {
      return 'notification';
    }
    return 'notification-un-seen'
  }

  view(not: ReactionNotification) {
    if(not.viewed == null) {
      not.viewed = 'yes';
      this.notService.viewNot(not.id);
    }
  }

  getNotImg(rec: ReactionNotification) {
    if(rec.follower) {
      if(rec.follower.avatar) {
        return rec.follower.avatar.url
      }
      else {
        return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNTYiIGhlaWdodD0iNTYiIHZpZXdCb3g9IjAgMCA1NiA1NiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGcgY2xpcC1wYXRoPSJ1cmwoI2NsaXAwXzUzNl8zKSI+CjxyZWN0IHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgZmlsbD0iI0FCQjlDOSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjU2IiByPSIyMCIgZmlsbD0iI0Y3RjlGQSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjIxIiByPSIxMSIgZmlsbD0iI0Y3RjlGQSIvPgo8L2c+CjxkZWZzPgo8Y2xpcFBhdGggaWQ9ImNsaXAwXzUzNl8zIj4KPHJlY3Qgd2lkdGg9IjU2IiBoZWlnaHQ9IjU2IiBmaWxsPSJ3aGl0ZSIvPgo8L2NsaXBQYXRoPgo8L2RlZnM+Cjwvc3ZnPgo=';
      }
    }

    if(rec.reaction) {
      if(rec.reaction.author.avatar) {
        return rec.reaction.author.avatar.url
      }
      else {
        return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNTYiIGhlaWdodD0iNTYiIHZpZXdCb3g9IjAgMCA1NiA1NiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGcgY2xpcC1wYXRoPSJ1cmwoI2NsaXAwXzUzNl8zKSI+CjxyZWN0IHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgZmlsbD0iI0FCQjlDOSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjU2IiByPSIyMCIgZmlsbD0iI0Y3RjlGQSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjIxIiByPSIxMSIgZmlsbD0iI0Y3RjlGQSIvPgo8L2c+CjxkZWZzPgo8Y2xpcFBhdGggaWQ9ImNsaXAwXzUzNl8zIj4KPHJlY3Qgd2lkdGg9IjU2IiBoZWlnaHQ9IjU2IiBmaWxsPSJ3aGl0ZSIvPgo8L2NsaXBQYXRoPgo8L2RlZnM+Cjwvc3ZnPgo=';
      }
    }

    if(rec.reToast) {
      if(rec.reToast.author?.avatar) {
        return rec.reToast.author?.avatar.url
      }
      else {
        return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNTYiIGhlaWdodD0iNTYiIHZpZXdCb3g9IjAgMCA1NiA1NiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGcgY2xpcC1wYXRoPSJ1cmwoI2NsaXAwXzUzNl8zKSI+CjxyZWN0IHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgZmlsbD0iI0FCQjlDOSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjU2IiByPSIyMCIgZmlsbD0iI0Y3RjlGQSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjIxIiByPSIxMSIgZmlsbD0iI0Y3RjlGQSIvPgo8L2c+CjxkZWZzPgo8Y2xpcFBhdGggaWQ9ImNsaXAwXzUzNl8zIj4KPHJlY3Qgd2lkdGg9IjU2IiBoZWlnaHQ9IjU2IiBmaWxsPSJ3aGl0ZSIvPgo8L2NsaXBQYXRoPgo8L2RlZnM+Cjwvc3ZnPgo=';
      }
    }

    if(rec.quote) {
      if(rec.quote.author?.avatar) {
        return rec.quote.author?.avatar.url
      }
      else {
        return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNTYiIGhlaWdodD0iNTYiIHZpZXdCb3g9IjAgMCA1NiA1NiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGcgY2xpcC1wYXRoPSJ1cmwoI2NsaXAwXzUzNl8zKSI+CjxyZWN0IHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgZmlsbD0iI0FCQjlDOSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjU2IiByPSIyMCIgZmlsbD0iI0Y3RjlGQSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjIxIiByPSIxMSIgZmlsbD0iI0Y3RjlGQSIvPgo8L2c+CjxkZWZzPgo8Y2xpcFBhdGggaWQ9ImNsaXAwXzUzNl8zIj4KPHJlY3Qgd2lkdGg9IjU2IiBoZWlnaHQ9IjU2IiBmaWxsPSJ3aGl0ZSIvPgo8L2NsaXBQYXRoPgo8L2RlZnM+Cjwvc3ZnPgo=';
      }
    }

    if(rec.reply) {
      if(rec.reply.author?.avatar) {
        return rec.reply.author?.avatar?.url
      }
      else {
        return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNTYiIGhlaWdodD0iNTYiIHZpZXdCb3g9IjAgMCA1NiA1NiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPGcgY2xpcC1wYXRoPSJ1cmwoI2NsaXAwXzUzNl8zKSI+CjxyZWN0IHdpZHRoPSI1NiIgaGVpZ2h0PSI1NiIgZmlsbD0iI0FCQjlDOSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjU2IiByPSIyMCIgZmlsbD0iI0Y3RjlGQSIvPgo8Y2lyY2xlIGN4PSIyOCIgY3k9IjIxIiByPSIxMSIgZmlsbD0iI0Y3RjlGQSIvPgo8L2c+CjxkZWZzPgo8Y2xpcFBhdGggaWQ9ImNsaXAwXzUzNl8zIj4KPHJlY3Qgd2lkdGg9IjU2IiBoZWlnaHQ9IjU2IiBmaWxsPSJ3aGl0ZSIvPgo8L2NsaXBQYXRoPgo8L2RlZnM+Cjwvc3ZnPgo=';
      }
    }

    return undefined;
  }

  goToProfilePage(id: number) : void {
    this.localRouter.goToProfilePage(id.toString());
  }

  truncateText(text: string): string {
    if (text.length <= 100) {
      return text; 
    } else {
      return text.substring(0, 100 - 3) + '...';
    }
  }

  truncateTextMini(text: string): string {
    if (text.length <= 20) {
      return text; 
    } else {
      return text.substring(0, 20 - 3) + '...';
    }
  }

  GoToToast(id: number) {
    this.localRouter.goToToastPage(id.toString());
  }
}