import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import {FormsModule} from '@angular/forms'
import {ParamMap, ActivatedRoute} from '@angular/router';
import {NotesService} from '../../services/notes.service'
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
import {ComponentEdit} from "../componentedit/componentedit.component"
declare var $:any;
declare var jQuery:any;


@Component({
    selector: 'notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css'],
    providers : [NotesService]
})
export class NotesComponent implements  OnInit, AfterViewInit{
  @ViewChild(ComponentEdit)
  private editor: ComponentEdit;

  ngAfterViewInit(){
      console.log('afterviewinit');
    }

  constructor(
    private route: ActivatedRoute,
    private notesService: NotesService,
  ){}
    
    
    public notes: Note[]=[];
    public categories: Category[]=[];
    public count: Number;
    public selectedNote: Note | null=null;
    
    private sessionId: string;
   
    
    


    ngOnInit(): void {
     
      //get the session id
      this.sessionId= this.route.snapshot.paramMap.get('id');
      console.log('sessionid: ' + this.sessionId);

      /*
      this.route.paramMap.switchMap((params: ParamMap) => {
        return params.get('id');
      }).subscribe(data=>
        {
          this.sessionId= data
          console.log('sessionid: ' + this.sessionId);
        });
*/

        

        

        //get the notes for this session
        this.notesService.getNotes(this.sessionId).subscribe(
          data => {
            this.notes=data;
          }
        );
        this.count=this.notes.length;
        this.notesService.getCategories(this.sessionId).subscribe(
          data=>{
            this.categories=data;
            console.log(this.categories);
          });
        this.selectedNote=null;
      
        
  


    }


  
  


    saveSelectedNote(): void{
   
      if(this.selectedNote)
      {
        console.log('saving: ' + this.selectedNote.text);
        this.notesService.saveNote(this.sessionId,this.selectedNote);        
      }
    }

    deleteNote(id:string)
    {

      console.log('deleting note: ' + id);

        var newNotes: Note[]=[];
        for(var i: number=0; i< this.notes.length; i++) {
          console.log(this.notes[i].commentId);
          if(this.notes[i].commentId!=id){
              newNotes.push(this.notes[i]);
            }
            else {
              console.log('deleted');
              this.notesService.deleteNote(id);
            }

        }
        this.notes=newNotes;
    }


    onSelect(note: Note): void{
      console.log('selected note: ' + note.text + note.commentId);
      this.selectedNote=note;
      $('#modalEditNote').modal();
    }

    onNewNote(categoryNum: number): void {
      console.log("creating a new note " + categoryNum + '- ' + this.sessionId)
      this.editor.categories=this.categories;
      this.editor.sessionId=this.sessionId;


      this.editor.newNote(categoryNum);     
      this.editor.show();
   
      
    }

    onDeleteId(id: string)
    {
      this.deleteNote(id); 
    }

    onEditId(id:null)
    {
      console.log('editing note: ' + id);

   
      for(var i: number=0; i< this.notes.length; i++) {
        if(this.notes[i].commentId==id){
            this.selectedNote=this.notes[i];
           // $('#modalEditNote').modal();
            this.editor.setNote(this.notes[i]);
            this.editor.categories=this.categories;
            this.editor.sessionId=this.sessionId;
            this.editor.show();
            console.log('found');
            break;
          }
   
        }


       
    }



    onDeleteSelected(): void{
      if(this.selectedNote)
      {
        this.deleteNote(this.selectedNote.commentId);
      }
      else{
        console.log('no selected note to delete');
      }
        this.onClearSelected();
    

    }

    onClearSelected(): void{
      console.log('clearing the selected note');
      this.saveSelectedNote();
      this.selectedNote=null;

    }

    onNoteMouseEnter(note):void{
      console.log('mouse enter');
      var id:string = "#note" + note.commentId;
      console.log(id);
      jQuery(id).children(".card-footer").show();
      
     // console.log(jQuery(note));
    }

    onNoteMouseLeave(note):void{
      console.log('mouse leave');
      var id:string = "#note" + note.commentId;
      console.log(id);
      jQuery(id).children(".card-footer").hide();
      
     // console.log(jQuery(note));
    }

    domId(note: Note):string{
      return 'note' + note.commentId;
    }

    CategoryDomId(category: Category):string{
      return "category" + category.categoryId;
    }


}

/*
class NoteEdit implements Note{
  commentId: number=0;
  categoryId: number;
  text: string='';
  updateUser: string='';

  constructor(categoryId: number){
    this.categoryId=categoryId;

  }

}
*/


