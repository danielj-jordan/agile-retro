import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { Team } from '../../models/team';
import { TeamMember } from '../../models/teammember';

@Component({
  selector: 'app-teamedit',
  templateUrl: './teamedit.component.html',
  styleUrls: ['./teamedit.component.css']
})
export class TeamEditComponent implements OnInit {
  team: Team;
  newUser: string;
  newRole: string;

  constructor(    
    private route: ActivatedRoute,
    private router: Router,
    private notesService: NotesService) { }

  ngOnInit() {
    this.team = new Team();
    this.team.name = '';
    this.newRole="member";
    this.newUser="";


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

  addPerson(): void{
    
    console.log('addPerson' + this.newRole);

    var newTeamMember= new TeamMember();
    newTeamMember.role=this.newRole;
    newTeamMember.userName=this.newUser;

    this.team.members.push(newTeamMember);

    this.newRole="member";
    this.newUser="";

  }

  navigateToList(): void {
    this.router.navigateByUrl('/list');
  }

  save(): void{
    this.team.meetings=null;
    this.notesService.saveTeam(this.team).subscribe();
    console.log(this.team);
    this.navigateToList();
  }

  getDisplayStringForRole(role: string):string{

    if(role=="manager"){
      return "Team Owner/Manager/Scrum Master";
    }

    if(role=="stakeholder"){
      return "Team Stakeholder";
    }

    return "Team Member";

  }

}
