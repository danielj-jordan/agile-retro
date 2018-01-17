import { Component,Inject } from '@angular/core';
import {Http} from '@angular/http';   

@Component({
    selector: 'notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css']
})
export class NotesComponent {

    public notes: Note[];
    public count: number;

    constructor( http: Http, @Inject('BASE_URL') baseUrl: string){
        http.get(baseUrl + 'api/notes/notes').subscribe(result => {
            this.notes= result.json() as Note[];
            this.count=this.notes.length;
            console.log('test');
        }, error=> console.log('error'));

        
    }



/*
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
            this.forecasts = result.json() as WeatherForecast[];
        }, error => console.error(error));
    }

*/

   
    
}



interface Note{
    commentId: number;
    categoryId: number;
    text: string;
    updateUser: string;

}

