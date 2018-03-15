import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotesComponent } from './components/notes/notes.component';
import { NoteComponent2Component } from './note-component2/note-component2.component';

const routes: Routes = [
  { path: 'notes', component: NotesComponent },
  { path: 'notes2', component: NoteComponent2Component }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [ RouterModule ]


})


export class AppRoutingModule { }
