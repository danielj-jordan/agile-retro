import { Inject, Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Note} from './notes';
import {Category} from './category';


@Injectable()
export class NotesService {

    private http:HttpClient;
    private baseUrl:string;

    constructor( http: HttpClient){
      this.http=http;
      this.baseUrl='api/notes';
       
      console.log('calling constructor');
    }


    getNotes():Observable<Note[]> {
        
       
        return this.http.get<Note[]>(this.baseUrl + '/notes');
        
        /*.subscribe(result => {
            notes= result.json() as Note[];
            console.log('getting notes' + notes.length);
            return notes as Note[];
        }, error=> console.log('notes error'));

        return notes;*/

    }

    getCategories() : Observable<Category[]>{

        return this.http.get<Category[]>(this.baseUrl + '/categories');
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
            return this.http.post<Note>(this.baseUrl + '/NewNote', note, options );
        }
       
            return this.http.put<Note>(this.baseUrl + '/note', note, options );
       

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
