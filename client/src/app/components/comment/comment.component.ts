import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import {FormsModule} from '@angular/forms'
import {ParamMap, ActivatedRoute} from '@angular/router';
import {NotesService} from '../../services/notes.service'
import {Comment} from '../../models/comment';
import {Category} from '../../models/category';
import {CommentEditComponent} from "../commentedit/commentedit.component"
declare var $:any;
declare var jQuery:any;


@Component({
    selector: 'comment',
    templateUrl: './comment.component.html',
    styleUrls: ['./comment.component.css'],
    providers : [NotesService]
})
export class CommentComponent implements  OnInit, AfterViewInit{
  @ViewChild(CommentEditComponent)
  private editor: CommentEditComponent;

  ngAfterViewInit(){
      console.log('afterviewinit');
    }

  constructor(
    private route: ActivatedRoute,
    private notesService: NotesService,
  ){}
    
    
    public comments: Comment[]=[];
    public categories: Category[]=[];
    public count: Number;
    public selectedNote: Comment | null=null;
    
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
            this.comments=data;
          }
        );
        this.count=this.comments.length;
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
        this.notesService.saveComment(this.sessionId,this.selectedNote);        
      }
    }

    deleteNote(id:string)
    {

      console.log('deleting note: ' + id);

        var newNotes: Comment[]=[];
        for(var i: number=0; i< this.comments.length; i++) {
          console.log(this.comments[i].Id);
          if(this.comments[i].Id!=id){
              newNotes.push(this.comments[i]);
            }
            else {
              console.log('deleted');
              this.notesService.deleteNote(id);
            }

        }
        this.comments=newNotes;
    }


    onSelect(note: Comment): void{
      console.log('selected note: ' + note.text + note.Id);
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

   
      for(var i: number=0; i< this.comments.length; i++) {
        if(this.comments[i].Id==id){
            this.selectedNote=this.comments[i];
           // $('#modalEditNote').modal();
            this.editor.setNote(this.comments[i]);
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
        this.deleteNote(this.selectedNote.Id);
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

    onNoteMouseEnter(note: Comment):void{
      console.log('mouse enter');
      var id:string = "#note" + note.Id;
      console.log(id);
      jQuery(id).children(".card-footer").show();
      
     // console.log(jQuery(note));
    }

    onNoteMouseLeave(note: Comment):void{
      console.log('mouse leave');
      var id:string = "#note" + note.Id;
      console.log(id);
      jQuery(id).children(".card-footer").hide();
      
     // console.log(jQuery(note));
    }

    domId(note: Comment):string{
      return 'note' + note.Id;
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


