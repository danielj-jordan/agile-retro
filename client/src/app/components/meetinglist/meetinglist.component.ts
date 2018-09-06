import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { LocalstorageService } from '../../services/localstorage.service';
import {User} from '../../models/user';
import {Team} from '../../models/team';


@Component({
  selector: 'app-meetinglist',
  templateUrl: './meetinglist.component.html',
  styleUrls: ['./meetinglist.component.css']
})
export class MeetingListComponent implements OnInit {

  constructor(
    private router: Router,
    private storage: LocalstorageService,
    private noteService: NotesService) { 
  }

  public teams: Team[];

  ngOnInit() {


       console.log('getting teams');
       
       //get the teams for this user
       this.noteService.getUserTeams(this.storage.userEmail).subscribe(
         data=>{
          this.teams=data;
          console.log('user teams: ' + this.teams);
          //get the retrospective meeting sessions for each team
          for(let team of this.teams){
            this.noteService.getTeamMeetings(team.teamId).subscribe(
              data=>team.meetings=data);
          }
          console.log(this.teams);
       });
      }
}
