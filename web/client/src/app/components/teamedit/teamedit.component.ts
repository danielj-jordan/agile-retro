import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { Team } from '../../models/team';
import { TeamMember } from '../../models/teammember';
import { Invitation } from '../../models/invitation';


@Component({
  selector: 'app-teamedit',
  templateUrl: './teamedit.component.html',
  styleUrls: ['./teamedit.component.css']
})
export class TeamEditComponent implements OnInit {
  team: Team;
  newUserName: string;
  newUserRole: string;
  newUserEmail: string;

  constructor(    
    private route: ActivatedRoute,
    private router: Router,
    private notesService: NotesService) { }

  ngOnInit() {
    this.team = new Team();
    this.team.name = '';
    this.newUserRole="Member";
    this.newUserName="";
    this.newUserEmail="";


    // get the teamid id
    let teamId = this.route.snapshot.paramMap.get('teamid');

    console.log(teamId);

    // get team info
    this.notesService.getTeam(teamId).subscribe(
      data => {
        this.team = data;
        console.log(this.team);
      }
    );
  }

  addInvite(): void {
    var invite = new Invitation();
    invite.email=this.newUserEmail;
    invite.role=this.newUserRole;
    invite.name=this.newUserName;

    this.notesService.addInvite(this.team.teamId, invite).subscribe(
      data => {
        this.team=data;
      });
  }

  removeInvite(email: string):void{
    console.log("removing: " + email);
    var invite = new Invitation();
    invite.email=email;

    this.notesService.removeInvite(this.team.teamId, invite).subscribe(
      data => {
        this.team=data;
      });
  }

  navigateToList(): void {
    this.router.navigateByUrl('/retrospective/list');
  }

  save(): void{
    this.team.meetings=null;
    this.notesService.saveTeam(this.team).subscribe();
    console.log(this.team);
    this.navigateToList();
  }

  getDisplayStringForRole(role: string):string{

    if(role=="Owner"){
      return "Team Owner/Scrum Master";
    }

    if(role=="Stakeholder"){
      return "Team Stakeholder";
    }

    return "Team Member";

  }

}
