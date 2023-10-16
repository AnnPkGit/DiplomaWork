import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
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
    SettingsPageComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AutosizeModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: AuthComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'sign-in', component: SignInComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'auth', component: AuthComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'toast/:id', component: PostPAgeComponent },
      { path: 'profile/:id', component: ProfilePageComponent },
      { path: 'notifications', component: NotifactionsPageComponent },
      { path: 'settings', component: SettingsPageComponent },

    ])
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule { }
