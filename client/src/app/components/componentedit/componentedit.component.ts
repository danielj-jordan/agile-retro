import { Component, OnInit } from '@angular/core';
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
import {NotesService} from '../../services/notes.service'
declare var $:any;
declare var jQuery:any;


@Component({
  selector: 'app-componentedit',
  templateUrl: './componentedit.component.html',
  styleUrls: ['./componentedit.component.css'],
  providers:[NotesService],
})
export class ComponentEdit implements OnInit {
  
  private note: Note;
  public categories: Category[];
  public sessionId: string;
  private notesService: NotesService;


  constructor(notesService:NotesService){
    this.notesService=notesService;

    
  }


  ngOnInit(): void {
    
  /*
    this.notesService.getCategories().subscribe(
      data=>{
        this.categories=data;
      });
  */
  }
  

  public newNote(categoryNum: number)
  {
    this.note=new NoteEdit(categoryNum);
  }

  public setNote(note: Note){
    this.note=note;
  }

  public getNote (): Note{
    return this.note;
  }

  public show(): void{
    console.log('showing' + this.note.text);
    $('#text').val(this.note.text);
    $('#modalEditNote').modal();
  }

  public onSave(): void{
    this.note.text= $('#text').val();
    this.notesService.saveNote(this.sessionId, this.note).subscribe(data=>{this.note=data;});
    $('#modalEditNote').hide();
  }

  public onCancel():void{

    $('#modalEditNote').hide();
  }


}


class NoteEdit implements Note{
  commentId: string="";
  sessionId: string ="";
  categoryId: number;
  text: string='';
  updateUser: string='';

  constructor(categoryId: number){
    this.categoryId=categoryId;

  }

}