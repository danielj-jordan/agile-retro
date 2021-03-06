import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommentComponent } from './components/comment/comment.component';
import { LoginComponent } from './components/login/login.component';
import { MeetingListComponent } from './components/meetinglist/meetinglist.component';
import { MeetingEditComponent } from './components/meetingedit/meetingedit.component';
import { TeamEditComponent } from './components/teamedit/teamedit.component';
import { MenuComponent } from './components/menu/menu.component';
import { PrivacyPolicyComponent } from './components/privacypolicy/privacypolicy.component';
import { LogoutComponent } from './components/logout/logout.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'privacypolicy', component: PrivacyPolicyComponent },
  { path: 'logout', component: LogoutComponent },
  {
    path: 'retrospective',
    component: MenuComponent,
    children: [
      { path: 'comment/:id', component: CommentComponent },
      { path: 'meetingedit/:teamid/:meetingid', component: MeetingEditComponent },
      { path: 'meetingedit/:teamid', component: MeetingEditComponent },
      { path: 'teamedit/:teamid', component: TeamEditComponent },
      { path: 'teamedit', component: TeamEditComponent },
      { path: 'list', component: MeetingListComponent },
      { path: 'privacypolicy', component: PrivacyPolicyComponent }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload'
    }),
  ],
  exports: [RouterModule]


})


export class AppRoutingModule { }
