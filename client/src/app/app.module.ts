
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './/app-routing.module';

import { NotesComponent } from './components/notes/notes.component';
import { NotesService }  from './services/notes.service';
import { ComponentEdit } from './components/componentedit/componentedit.component';


@NgModule({
    imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  declarations: [
    AppComponent,
    NotesComponent,
    ComponentEdit,

  ],
  providers: [NotesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
