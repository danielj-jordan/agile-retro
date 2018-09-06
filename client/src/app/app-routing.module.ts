import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommentComponent } from './components/comment/comment.component';
import { LoginComponent } from './components/login/login.component';
import { MeetingListComponent } from './components/meetinglist/meetinglist.component';


const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch:'full' },
  { path: 'notes/:id', component: CommentComponent}, 
  { path: 'login', component: LoginComponent },
  { path: 'list',  component: MeetingListComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [ RouterModule ]


})


export class AppRoutingModule { }
