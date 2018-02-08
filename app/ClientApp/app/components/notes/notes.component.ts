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
    public count: Number;
    public selectedNote: Note | null;

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
      console.log('selected note: ' + note.text + note.commentId);
      this.selectedNote=note;
    }

    onNewNote(categoryId: number): void {
      console.log("creating a new note " + categoryId)
      let note =  new NoteEdit(categoryId);
      this.notes.push(note);
      this.selectedNote=note;
    }

    onDelete(note: Note): void{
      if(this.selectedNote)
      {
        console.log('deleting note: ' + this.selectedNote.commentId);
      //  this.selectedNote=null;

        var newNotes: Note[]=[];
        for(var i: number=0; i< this.notes.length; i++) {
          console.log(this.notes[i].commentId);
          if(this.notes[i].commentId!=this.selectedNote.commentId){
              newNotes.push(this.notes[i]);
            }
            else {
              console.log('deleted');
            }

        }
        this.notes=newNotes;
        this.onClearSelected();
    }

    }

    onClearSelected(): void{
      console.log('clearing the selected note');
      this.selectedNote=null;

    }


}

class NoteEdit implements Note{
  commentId: number;
  categoryId: number;
  text: string;
  updateUser: string;

  constructor(categoryId: number){
    this.categoryId=categoryId;

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
