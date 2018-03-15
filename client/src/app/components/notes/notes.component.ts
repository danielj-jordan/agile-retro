import { Component,Inject } from '@angular/core';
import {NgModule} from '@angular/core'
import {FormsModule} from '@angular/forms'
import {NotesService} from '../../services/notes.service'
import {Note} from '../../services/notes';
import {Category} from '../../services/category';

@Component({
    selector: 'notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css'],
    providers : [NotesService]
})
export class NotesComponent {

    public notes: Note[]=[];
    public categories: Category[]=[];
    public count: Number;
    public selectedNote: Note | null=null;
    private notesService:NotesService;
  

    constructor(notesService:NotesService){
      this.notesService=notesService;
      notesService.getNotes().subscribe(
        data => {
          this.notes=data;
        }
      );
      this.count=this.notes.length;
      notesService.getCategories().subscribe(
        data=>{
          this.categories=data;
        });
      console.log('calling constructor');
    }

    saveSelectedNote(): void{
   
      if(this.selectedNote)
      {
        console.log('saving: ' + this.selectedNote.text);
        this.notesService.saveNotes(this.selectedNote);        
      }
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

        var newNotes: Note[]=[];
        for(var i: number=0; i< this.notes.length; i++) {
          console.log(this.notes[i].commentId);
          if(this.notes[i].commentId!=this.selectedNote.commentId){
              newNotes.push(this.notes[i]);
            }
            else {
              console.log('deleted');
              this.notesService.deleteNote(this.selectedNote.commentId);
            }

        }
        this.notes=newNotes;
        this.onClearSelected();
    }

    }

    onClearSelected(): void{
      console.log('clearing the selected note');
      this.saveSelectedNote();
      this.selectedNote=null;

    }


}

class NoteEdit implements Note{
  commentId: number=0;
  categoryId: number;
  text: string='';
  updateUser: string='';

  constructor(categoryId: number){
    this.categoryId=categoryId;

  }

}


