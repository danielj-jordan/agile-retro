import { Inject, Injectable } from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Note} from './notes';
import {Category} from './category';


@Injectable()
export class NotesServiceMock {


    public getNotes():Observable<Note[]> {
        let notes: Note[] =[]
        notes.push({commentId:1, categoryId:1, text:"this is a test", updateUser:"bob"});
        notes.push({commentId:2, categoryId:1, text:"this is also a test", updateUser:"bob"});

        console.log('notes count: ' + notes.length);
        return Observable.of(notes);
    }

    getCategories(): Observable<Category[]>{
        let categories$:Observable<Category[]>;
        let categories: Category[]=[];


        categories.push({categoryId:1, name:"test"});
        categories.push({categoryId:2, name:"another test"});
        categories$= Observable.create( (observer:any) => 
        {
            observer.next(categories);
        })
        return categories$;
    }

    saveNotes(note:Note): Note{
 
          if(note.commentId==0)note.categoryId=100;
          return note;

    }

    deleteNote(commentId: number): void{  
          return ;
    }





}