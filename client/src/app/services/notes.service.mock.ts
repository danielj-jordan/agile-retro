import { Inject, Injectable } from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Note} from './notes';
import {Category} from './category';
import {Team} from '../models/team';
import {Retrospective} from '../models/retrospective';


@Injectable()
export class NotesServiceMock {

    getUserTeams(userEmail: string): Observable<Team[]>{
        let teams: Team[] =[]  

        return Observable.of(teams);
    }

    getTeamMeetings(teamId: string): Observable<Retrospective[]>{
        let retrospectives: Retrospective[] =[]  

        return Observable.of(retrospectives);
    }


    public getNotes(sessionId: string):Observable<Note[]> {
        let notes: Note[] =[]
        notes.push({commentId:"1", sessionId:" + sessionId + ", categoryId:1, text:"this is a test", updateUser:"bob"});
        notes.push({commentId:"2", sessionId:" + sessionId + ", categoryId:1, text:"this is also a test", updateUser:"bob"});

        console.log('notes count: ' + notes.length);
        return Observable.of(notes);
    }

    getCategories(sessionId: string): Observable<Category[]>{
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

    saveNote(sessionId: string, note:Note): Note{
 
        note.sessionId=sessionId;
        if(note.commentId.length==0)note.categoryId=100;
        return note;

    }

    deleteNote(commentId: number): void{  
          return ;
    }





}