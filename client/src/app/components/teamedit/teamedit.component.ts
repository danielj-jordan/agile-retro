import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { Team } from '../../models/team';

@Component({
  selector: 'app-teamedit',
  templateUrl: './teamedit.component.html',
  styleUrls: ['./teamedit.component.css']
})
export class TeamEditComponent implements OnInit {
  team: Team;

  constructor(    
    private route: ActivatedRoute,
    private router: Router,
    private notesService: NotesService) { }

  ngOnInit() {
    this.team = new Team();
    this.team.name = '';


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

  navigateToList(): void {
    this.router.navigateByUrl('/list');
  }

  save(): void{
    this.team.meetings=null;
    this.notesService.saveTeam(this.team).subscribe();
    console.log(this.team);
    this.navigateToList();
  }

}
