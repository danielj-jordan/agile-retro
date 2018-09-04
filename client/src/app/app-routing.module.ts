import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotesComponent } from './components/notes/notes.component';
import { LoginComponent } from './components/login/login.component';
import { RetrospectivelistComponent } from './components/retrospectivelist/retrospectivelist.component';


const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch:'full' },
  { path: 'notes/:id', component: NotesComponent}, 
  { path: 'login', component: LoginComponent },
  { path: 'list',  component: RetrospectivelistComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [ RouterModule ]


})


export class AppRoutingModule { }
