import { Inject, Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Comment} from '../models/comment';
import {Category} from '../models/category';
import {Team} from '../models/team';
import { Meeting } from '../models/meeting';
 

@Injectable()
export class NotesService {

    private http:HttpClient;
    private baseUrl:string;

    constructor( http: HttpClient){
      this.http=http;
      this.baseUrl='api';
    }


    getNotes(sessionId: string):Observable<Comment[]> {       
        return this.http.get<Comment[]>(this.baseUrl + '/notes/notes/' + sessionId);
    }

    getUserTeams(userEmail: string): Observable<Team[]>{
        console.log('teams' + userEmail);
        return this.http.get<Team[]>(this.baseUrl + '/team/teams/' + userEmail);
    }

    getTeamMeetings(teamId: string): Observable<Meeting[]>{
        console.log('retrospectives for team: ' + teamId);
        return this.http.get<Meeting[]>(this.baseUrl + '/meeting/meetings/' + teamId);
    }

    getMeeting(meetingId: string): Observable<Meeting>{
        console.log('getting single meeting');
        return this.http.get<Meeting>(this.baseUrl + '/meeting/meeting/' + meetingId);   
    }

    /*
    getRetrospectives(teamId: string) : Observable<[]>{

        return this.http.get<Category[]>(this.baseUrl + '/notes/categories');
    }*/

    getCategories(sessionId: string) : Observable<Category[]>{
        console.log('getting categories');
        return this.http.get<Category[]>(this.baseUrl + '/notes/categories/' + sessionId);
        /*
        var categories:Category[]=[];


        this.http.get(this.baseUrl + '/categories').subscribe(result =>{
            console.log(result);
            categories=result.json() as Category[];
            console.log('getting categories');
            console.log('count2:' + categories.length);
            return categories as Category[];
          }, error=>console.log('categories error'));

          console.log('count:' + categories.length);
        return categories;*/
    }

    saveComment(sessionId: string, note:Comment): Observable<Comment>{
        
        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};

        if(note.commentId.length==0){
            return this.http.post<Comment>(this.baseUrl + '/notes/NewNote/' + sessionId, note, options );
        }
       
            return this.http.put<Comment>(this.baseUrl + '/notes/note/' + sessionId, note, options );
       

    }

    deleteNote(commentId: string): void{

        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};
       

        this.http.delete(this.baseUrl + '/notes/DeleteNote/'+ commentId,
          options
          ).subscribe();
          
          return ;
        

    }



}
