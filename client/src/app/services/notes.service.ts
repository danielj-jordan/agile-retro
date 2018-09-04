import { Inject, Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Note} from './notes';
import {Category} from './category';
import {Team} from '../models/team';
import { Retrospective } from '../models/retrospective';
 

@Injectable()
export class NotesService {

    private http:HttpClient;
    private baseUrl:string;

    constructor( http: HttpClient){
      this.http=http;
      this.baseUrl='api';
    }


    getNotes(sessionId: string):Observable<Note[]> {       
        return this.http.get<Note[]>(this.baseUrl + '/notes/notes/' + sessionId);
    }

    getUserTeams(userEmail: string): Observable<Team[]>{
        console.log('teams' + userEmail);
        return this.http.get<Team[]>(this.baseUrl + '/team/teams/' + userEmail);
    }

    getTeamMeetings(teamId: string): Observable<Retrospective[]>{
        console.log('retrospectives for team: ' + teamId);
        return this.http.get<Retrospective[]>(this.baseUrl + '/session/meetings/' + teamId);
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

    saveNote(note:Note): Observable<Note>{
        
        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};

        if(note.commentId==0){
            return this.http.post<Note>(this.baseUrl + '/notes/NewNote', note, options );
        }
       
            return this.http.put<Note>(this.baseUrl + '/notes/note', note, options );
       

    }

    deleteNote(commentId: number): void{

        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};
       

        this.http.delete(this.baseUrl + '/note/'+ commentId,
          options
          ).subscribe();
          
          return ;
        

    }



}
