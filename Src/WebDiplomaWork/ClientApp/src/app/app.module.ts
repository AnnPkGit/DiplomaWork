import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AuthComponent } from './identification/auth/auth.component';
import { SignInComponent } from './identification/signIn/signIn.component';
import { RegisterComponent } from './identification/register/register.component';
import { PostComponent } from './home/post/post.component';
import { NavBarComponent } from './home/navBar/navBar';
import { ToastCreatorComponent } from './home/toastCreator/toastCreator.component';
import { RecProfileComponent } from './home/profileRec/recProfile.component';
import { AuthGuard } from './shared/Guards/auth.guard';
import { ImgOpened } from './home/imgOpened/imageOpened.component';
import { MessagesComponent } from './messages/messages.component';
import { PostPAgeComponent } from './post-page/post-page.component';
import { ProfileBarComponent } from './home/profileBar/profile-bar.component';
import { ToastItComponent } from './toast-it-page/toast-it.component';
import { ProfilePageComponent } from './profile-page/profile.component';
import { ProfileEditModal } from './modals/ProfileEditModal';
import { AutosizeModule } from 'ngx-autosize';
import { NotifactionsPageComponent } from './notification-page/notification-page';
import { SettingsPageComponent } from './settings-page/settings-page.component';
import { ToastModalComponent } from './toast-modal/toast-modal';
import { ToastBtn } from './toast-button/toast-button';
import { ExploreComponent } from './Explore/explore-component';
import { FollowExplorerComponent } from './follow-explorer/follow-explorer';
import { ExploreProfComponent } from './explore-prof-component/explore-prof-component';
import { AuthInterceptor } from './HttpInterceptor/auth-interceptor.service';
import { NotificationService } from './service/notifications.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AuthComponent,
    SignInComponent,
    RegisterComponent,
    PostComponent,
    NavBarComponent,
    ToastCreatorComponent,
    RecProfileComponent,
    ImgOpened,
    MessagesComponent,
    PostPAgeComponent,
    ProfileBarComponent,
    ToastItComponent,
    ProfilePageComponent,
    ProfileEditModal,
    NotifactionsPageComponent,
    SettingsPageComponent,
    ToastModalComponent,
    ExploreComponent,
    ToastBtn,
    FollowExplorerComponent,
    ExploreProfComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AutosizeModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: AuthComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full'},
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'sign-in', component: SignInComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'auth', component: AuthComponent },
      { path: 'messages', component: ExploreComponent },
      { path: 'toast/:id', component: PostPAgeComponent },
      { path: 'profile/:id', component: ProfilePageComponent },
      { path: 'notifications', component: NotifactionsPageComponent },
      { path: 'settings', component: SettingsPageComponent },
    ])
  ],
  providers: [
      AuthGuard, 
      NotificationService,
      {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
