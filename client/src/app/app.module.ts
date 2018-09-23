
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './/app-routing.module';

import { CommentComponent } from './components/comment/comment.component';
import { NotesService }  from './services/notes.service';
import { CommentEditComponent } from './components/commentedit/commentedit.component';
import { LoginComponent } from './components/login/login.component';
import { MeetingListComponent } from './components/meetinglist/meetinglist.component';
import { LocalstorageService } from './services/localstorage.service';
import { Router } from '@angular/router';
import { MeetingEditComponent } from './components/meetingedit/meetingedit.component';


@NgModule({
    imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  declarations: [
    AppComponent,
    CommentComponent,
    CommentEditComponent,
    LoginComponent,
    MeetingListComponent,
    MeetingEditComponent,

  ],
  providers: [NotesService, LocalstorageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
