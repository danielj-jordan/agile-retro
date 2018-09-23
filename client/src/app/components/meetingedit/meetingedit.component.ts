import { Component, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import {NotesService} from '../../services/notes.service'
import {Team} from '../../models/team';
import {Meeting} from '../../models/meeting';
import { Category } from '../../models/category';

@Component({
  selector: 'app-meetingedit.component',
  templateUrl: './meetingedit.component.html',
  styleUrls: ['./meetingedit.component.css']
})
export class MeetingEditComponent implements OnInit {

  constructor(    
    private route: ActivatedRoute,
    private notesService: NotesService) { 
    }

    public team: Team;
    public meeting: Meeting;

  ngOnInit() {

          this.meeting=new Meeting();
          this.meeting.name='';

          this.team= new Team();
          this.team.name='';


          //get the teamid id
          let teamId= this.route.snapshot.paramMap.get('teamId');

          let meetingId=this.route.snapshot.paramMap.get('meetingId');

          //get team info
          this.notesService.getTeam(teamId).subscribe(
            data=>{
              this.team=data;
            }
          );
          

          if(meetingId!=null){
            //editing existing meeting
            this.notesService.getMeeting(meetingId).subscribe(
              data=>{
                this.meeting=data;
                console.log(this.meeting);
              });
          }
          else{
            this.meeting=new Meeting();
            this.meeting.teamId=teamId;
          }
          

  }

  save(): void{
    this.notesService.saveMeeting(this.meeting).subscribe(
      data=>{
        
      }
    );
  }

  add(): void{
    //add the category to the list


  }

  nextCategoryNumber(): number{
    //find the maximum category number and return a higher value
    let next=1;
    for (let cat of this.meeting.categories)
    {
      if(cat.categoryNum>=next)next=cat.categoryNum+1;
    }
    return next;
  }

}
