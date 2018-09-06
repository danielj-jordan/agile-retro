import { Component, OnInit } from '@angular/core';
import {Comment} from '../../models/comment';
import {Category} from '../../models/category';
import {NotesService} from '../../services/notes.service'
declare var $:any;
declare var jQuery:any;


@Component({
  selector: 'app-commentedit',
  templateUrl: './commentedit.component.html',
  styleUrls: ['./commentedit.component.css'],
  providers:[NotesService],
})
export class CommentEditComponent implements OnInit {
  
  private note: Comment;
  public categories: Category[];
  public sessionId: string;
  private notesService: NotesService;


  constructor(notesService:NotesService){
    this.notesService=notesService;

    
  }


  ngOnInit(): void {
    
  }
  

  public newNote(categoryNum: number)
  {
    this.note=new NoteEdit(categoryNum);
  }

  public setNote(note: Comment){
    this.note=note;
  }

  public getNote (): Comment{
    return this.note;
  }

  public show(): void{
    console.log('showing' + this.note.text);
    $('#text').val(this.note.text);
    $('#modalEditNote').modal();
  }

  public onSave(): void{
    this.note.text= $('#text').val();
    this.notesService.saveComment(this.sessionId, this.note).subscribe(data=>{this.note=data;});
    $('#modalEditNote').hide();
  }

  public onCancel():void{

    $('#modalEditNote').hide();
  }


}


class NoteEdit implements Comment{
  Id: string="";
  sessionId: string ="";
  categoryId: number;
  text: string='';
  updateUser: string='';

  constructor(categoryId: number){
    this.categoryId=categoryId;

  }

}