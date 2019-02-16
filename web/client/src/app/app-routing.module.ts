import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommentComponent } from './components/comment/comment.component';
import { LoginComponent } from './components/login/login.component';
import { MeetingListComponent } from './components/meetinglist/meetinglist.component';
import {MeetingEditComponent } from './components/meetingedit/meetingedit.component';
import { TeamEditComponent } from './components/teamedit/teamedit.component';
import { MenuComponent } from './components/menu/menu.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch:'full' },
  { path: 'login', component: LoginComponent },
  { path: 'retrospective',
    component: MenuComponent,
    children: [
      { path: 'comment/:id', component: CommentComponent}, 
      { path: 'meetingedit/:teamid/:meetingid', component: MeetingEditComponent},
      { path: 'meetingedit/:teamid', component: MeetingEditComponent},
      { path: 'teamedit/:teamid', component: TeamEditComponent},
      { path: 'teamedit', component: TeamEditComponent},
      { path: 'list',  component: MeetingListComponent}
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation:'reload'
    }),
  ],
  exports: [ RouterModule ]


})


export class AppRoutingModule { }
