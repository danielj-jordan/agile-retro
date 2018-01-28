import { Component,Inject } from '@angular/core';
import {Http} from '@angular/http'
import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'

@Component({
    selector: 'notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css']
})
export class NotesComponent {

    public notes: Note[];
    public categories: Category[];
    public count: number;
    public selectedNote: Note;

    constructor( http: Http, @Inject('BASE_URL') baseUrl: string){

        http.get(baseUrl + 'api/notes/notes').subscribe(result => {
            this.notes= result.json() as Note[];
            this.count=this.notes.length;
            console.log('getting notes');
        }, error=> console.log('notes error'));


        http.get(baseUrl + 'api/notes/categories').subscribe(result =>{
          this.categories=result.json() as Category[];
          console.log('getting categories');
        }, error=>console.log('categories error'));

        console.log('calling constructor');
    }


    onSelect(note: Note): void{
      console.log('selected note: note.text');
      this.selectedNote=note;
    }
}

interface Category{
  categoryId: number;
  name: string;
}

interface Note{
    commentId: number;
    categoryId: number;
    text: string;
    updateUser: string;


}
