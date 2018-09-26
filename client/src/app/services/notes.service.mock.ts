import { Inject, Injectable } from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Comment} from '../models/comment';
import {Category} from '../models/category';
import {Team} from '../models/team';
import {Meeting} from '../models/meeting';


@Injectable()
export class NotesServiceMock {

    getUserTeams(userEmail: string): Observable<Team[]>{
        let teams: Team[] =[]  

        return Observable.of(teams);
    }

    getTeamMeetings(teamId: string): Observable<Meeting[]>{
        let retrospectives: Meeting[] =[]  

        return Observable.of(retrospectives);
    }


    public getNotes(sessionId: string):Observable<Comment[]> {
        let notes: Comment[] =[]
        notes.push({commentId: "1", sessionId:" + sessionId + ", categoryNum:1, text:"this is a test", updateUser:"bob"});
        notes.push({commentId: "2", sessionId:" + sessionId + ", categoryNum:1, text:"this is also a test", updateUser:"bob"});

        console.log('notes count: ' + notes.length);
        return Observable.of(notes);
    }

    getCategories(sessionId: string): Observable<Category[]>{
        let categories$:Observable<Category[]>;
        let categories: Category[]=[];


        categories.push({categoryNum:1, name:"test", sortOrder:1});
        categories.push({categoryNum:2, name:"another test", sortOrder: 2});
        categories$= Observable.create( (observer:any) => 
        {
            observer.next(categories);
        })
        return categories$;
    }

    saveNote(sessionId: string, note:Comment): Comment{
 
        note.sessionId=sessionId;
        if(note.commentId.length==0)note.categoryNum=100;
        return note;

    }

    deleteNote(commentId: number): void{  
          return ;
    }

    getMeeting(meetingId: string): Observable<Meeting>{
        let meeting= new Meeting(); 
        meeting.id='1';
        meeting.name='test';
        meeting.teamId='2';
        meeting.categories=[];
        
        meeting.categories.push({categoryNum:1, name:"test category 1", sortOrder: 1});
        meeting.categories.push({categoryNum:2, name:"test category 2", sortOrder: 2});
        meeting.categories.push({categoryNum:3, name:"test category 3", sortOrder: 3});
        return Observable.of(meeting);
    }

    getTeam(teamId: string): Observable<Team>{
        let team = new Team();
        team.teamId='test1team';
        team.name="test team";
       // team.meetings= new Meeting[0];

        console.log('getting one team');
        return Observable.of(team); 
    }

    saveMeeting(meeting: Meeting): Observable<Meeting>{
        if(meeting.id==null)meeting.id='1';

        return Observable.of(meeting);

    }






}