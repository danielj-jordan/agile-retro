import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { FormsModule } from '@angular/forms';
import { ParamMap, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { Comment } from '../../models/comment';
import { Category } from '../../models/category';
import { Meeting } from '../../models/meeting';
import { CommentEditComponent } from '../commentedit/commentedit.component';
declare var $: any;
declare var jQuery: any;


@Component({
  selector: 'app-commentlist',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css'],
  providers: [NotesService]
})
export class CommentComponent implements OnInit, AfterViewInit {
  @ViewChild(CommentEditComponent)
  private editor: CommentEditComponent;

  public comments: Comment[] = [];
  public categories: Category[] = [];
  public selectedNote: Comment | null = null;

  private sessionId: string;
  public meeting: Meeting = null;


  ngAfterViewInit() {
    console.log('afterviewinit');
  }

  constructor(
    private route: ActivatedRoute,
    private notesService: NotesService,
  ) { }


  ngOnInit(): void {

    // get the session id
    this.sessionId = this.route.snapshot.paramMap.get('id');
    console.log('sessionid: ' + this.sessionId);

    // get the categories for this meeting
    this.notesService.getCategories(this.sessionId).subscribe(
      data => {
        this.categories = data;
        console.log(this.categories);
      }
    );
    this.selectedNote = null;

    // get the meeting name
    this.notesService.getMeeting(this.sessionId).subscribe(
      data => {
        this.meeting = data;
        console.log('meeting');
        console.log(this.meeting);
      }
    );

    // this one always runs
    this.getComments();
    console.log(this.comments);

    // this one doesn't run if timer is stopped as in the jasmine test
    Observable.timer(3000, 3000).subscribe(
      t => {
        this.getComments();
      }
    );
  }

  findNoteById(id: string){
    for (let i = 0; i < this.comments.length; i++) {
      if (this.comments[i].commentId === id) {
        return this.comments[i];
      }
    }
  }

  isCategoriesFew(): boolean{
    if(this.categories.length<4){
      return true;
    }
    return false;
  }
  
  getComments(): void {
    // get the notes for this session
    this.notesService.getNotes(this.sessionId).subscribe(
      data => {
        this.comments = data;
      }
    );
  }

  saveSelectedNote(): void {

    if (this.selectedNote) {
      console.log('saving: ' + this.selectedNote.text);
      this.notesService.saveComment(this.sessionId, this.selectedNote);
    }
  }

  deleteNote(id: string) {

    console.log('deleting note: ' + id);

    const newNotes: Comment[] = [];
    for (let i = 0; i < this.comments.length; i++) {
      console.log(this.comments[i].commentId);
      if (this.comments[i].commentId !== id) {
        newNotes.push(this.comments[i]);
      } else {
        console.log('deleted');
        this.notesService.deleteNote(id);
      }
    }
    this.comments = newNotes;
  }

  onVoteUp(id: string){
    console.log("votting up")
    let note = this.findNoteById(id);
    if(!note.thisUserVoted)
    {
      note.voteCount++;
      note.thisUserVoted=true;
    }
    this.notesService.voteUp(id);
  }

  onVoteDown(id: string){
    console.log("votting up")
    let note = this.findNoteById(id);
    if(note.thisUserVoted)
    {
      note.voteCount--;
      note.thisUserVoted=false;
    }
    this.notesService.voteDown(id);
  }

  onSelect(note: Comment): void {
    console.log('selected note: ' + note.text + note.commentId);
    this.selectedNote = note;
    $('#modalEditNote').modal();
  }

  onNewNote(categoryNum: number): void {
    console.log('creating a new note ' + categoryNum + '- ' + this.sessionId);
    this.editor.categories = this.categories;
    this.editor.sessionId = this.sessionId;

    this.editor.newComment(categoryNum);
    this.editor.show();
  }

  onDeleteId(id: string) {
    this.deleteNote(id);
  }

  onEditId(id: null) {
    console.log('editing note: ' + id);


    for (let i = 0; i < this.comments.length; i++) {
      if (this.comments[i].commentId === id) {
        this.selectedNote = this.comments[i];
        this.editor.setComment(this.comments[i]);
        this.editor.categories = this.categories;
        this.editor.sessionId = this.sessionId;
        this.editor.show();
        break;
      }

    }

  }

  onDeleteSelected(): void {
    if (this.selectedNote) {
      this.deleteNote(this.selectedNote.commentId);
    } else {
      console.log('no selected note to delete');
    }
    this.onClearSelected();
  }

  onClearSelected(): void {
    console.log('clearing the selected note');
    this.saveSelectedNote();
    this.selectedNote = null;
  }

  /*
  onNoteMouseEnter(note: Comment): void {
    const id: string = '#' + this.domId(note);
    jQuery(id).children('.card-footer').show();
  }

  onNoteMouseLeave(note: Comment): void {
    const id: string = '#' + this.domId(note);
    jQuery(id).children('.card-footer').hide();
  }
  */

  domId(comment: Comment): string {
    return 'comment' + comment.commentId;
  }

  CategoryDomId(category: Category): string {
    return 'category' + category.categoryNum;
  }
}

