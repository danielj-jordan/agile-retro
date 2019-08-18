import { Inject, Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Comment} from '../models/comment';
import {Category} from '../models/category';
import {Team} from '../models/team';
import { Meeting } from '../models/meeting';
import {UserLogin} from '../models/userlogin';
import {LocalstorageService} from './localstorage.service';
import { Options } from 'selenium-webdriver/edge';
import { UserToken } from '../models/usertoken';
import {environment} from '../../environments/environment';
import { Invitation } from '../models/invitation';

@Injectable()
export class NotesService {

   
    private baseUrl:string;

    constructor( 
        private http: HttpClient,
        private storage: LocalstorageService){
      this.baseUrl="api"; 
    }


    addAuthHeader(headers: HttpHeaders):HttpHeaders {
        return headers.set('Authorization', 'Bearer ' + this.storage.userToken);
    }

    isLoggedIn()
    {
        this.http.get(this.baseUrl + '/auth/isloggedin')
        .subscribe(resp => console.log(resp));
    }

    loginGoogle(googleToken: UserToken):Observable<UserToken>{
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};
        console.log("calling loginGoogle: " + googleToken);

        return this.http.post<UserToken>(this.baseUrl + '/auth/logingoogle', googleToken, options );
    }

    login( user: UserLogin): Observable<UserToken>{
        
        console.log('logging in: ' + user.LoginName);
        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};

        return this.http.post<UserToken>(this.baseUrl + '/auth/generatetoken', user, options );
    }

    loginDemo(): Observable<UserToken>{
        
        console.log('logging in demo user');
        
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var options = { headers : headers};

        return this.http.post<UserToken>(this.baseUrl + '/auth/demo', options );
    }

    getCategories(meetingId: string) : Observable<Category[]>{
        console.log('getting categories');
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
        return this.http.get<Category[]>(this.baseUrl + '/notes/categories/' + meetingId, options);
    }

    getMeeting(meetingId: string): Observable<Meeting>{
        console.log('getting single meeting');
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
        return this.http.get<Meeting>(this.baseUrl + '/meeting/meeting/' + meetingId, options);   
    }

    getNotes(meetingId: string):Observable<Comment[]> {    
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};   
        return this.http.get<Comment[]>(this.baseUrl + '/notes/notes/' + meetingId, options);
    }

    getTeam(teamId: string): Observable<Team>{
        console.log('getting single team');
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
        return this.http.get<Team>(this.baseUrl + '/team/team/' + teamId, options);   
    }

    getTeamMeetings(teamId: string): Observable<Meeting[]>{
        console.log('retrospectives for team: ' + teamId);
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
        return this.http.get<Meeting[]>(this.baseUrl + '/meeting/meetings/' + teamId, options);
    }

    getUserTeams(): Observable<Team[]>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};

        console.log(options);
        return this.http.get<Team[]>(this.baseUrl + '/team/teams/' , options);
    }

    saveComment(meetingId: string, note:Comment): Observable<Comment>{
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};

        if(note.commentId.length==0){
            return this.http.post<Comment>(this.baseUrl + '/notes/NewNote/' + meetingId, note, options );
        }
       
            return this.http.put<Comment>(this.baseUrl + '/notes/note/' + meetingId, note, options );
    }


    saveMeeting(meeting: Meeting): Observable<Meeting>{
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
    
        console.log('saving meeting');
        console.log (meeting);

         return this.http.post<Meeting>(this.baseUrl + '/meeting/meeting', meeting, options );

    }

    saveTeam(team: Team): Observable<Team>{
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
        
        console.log('saving meeting');
        console.log (team);

         return this.http.post<Team>(this.baseUrl + '/team/team', team, options );
    }

    deleteNote(commentId: string): void{
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};
       

        this.http.delete(this.baseUrl + '/notes/deletenote/'+ commentId,
          options
          ).subscribe();
          
          return ;
    }

    voteUp(commentId: string): void{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers}; 
       
        this.http.put(this.baseUrl + '/notes/voteup/'+ commentId,null,
          options
          ).subscribe();
          
          return ;
    }

    voteDown(commentId: string): void{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};   
       
        this.http.put(this.baseUrl + '/notes/votedown/'+ commentId,null,
          options
          ).subscribe();
          
          return ;
    }

    addInvite(teamId: string, invitation: Invitation): Observable<Team>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};   
       
        return this.http.post<Team>(this.baseUrl + '/team/invite/'+ teamId,invitation,
          options);
    }

    removeInvite(teamId: string, invite: Invitation): Observable<Team>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};   
       
        return this.http.post<Team>(this.baseUrl + '/team/uninvite/'+ teamId,invite,
          options);
    }

    getMyInvitations(): Observable<Team[]>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};

        console.log(options);
        return this.http.get<Team[]>(this.baseUrl + '/team/myinvitations/' , options);
    }

    acceptnvitations(teamId: string): Observable<Team[]>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};

        console.log(options);
        return this.http.post<Team[]>(this.baseUrl + '/team/acceptinvitation/' + teamId, null, options);
    }

    createTeam(): Observable<Team[]>{
        var headers= new HttpHeaders();
        headers= this.addAuthHeader(headers);
        var options = { headers : headers};

        console.log(options);
        return this.http.post<Team[]>(this.baseUrl + '/team/createteam' ,null, options);
    }
}
