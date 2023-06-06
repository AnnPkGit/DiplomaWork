import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
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
    ImgOpened
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: AuthComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'sign-in', component: SignInComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'auth', component: AuthComponent },
    ])
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule { }
