import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { LocalstorageService } from '../../services/localstorage.service';
import { User } from '../../models/user';
import { Team } from '../../models/team';
import { strictEqual } from 'assert';


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
  public inviteTeams: Team[];

  ngOnInit() {
    this.getTeamData();
    this.getInviteData();
  }

  createTeam(): void {
    this.noteService.createTeam().subscribe(
      data => {
        this.getTeamData();
      });
  }

  inviteCount(): number {
    if (this.inviteTeams == undefined) return 0;
    return this.inviteTeams.length;
  }

  isDemoUser(): boolean {
    if (this.storage.user.isDemoUser) {
      return true;
    }
    return false;
  }

  ownedTeamCount(): number {
    if (this.storage.user == null ||
      this.teams == undefined) {
      return 0;
    }
    let activeUserId = this.storage.user.userId;
    let count = 0;
    for (let team of this.teams) {
      if (team.members != null) {
        for (let member of team.members) {
          if (member.userId != null &&
            member.userId == activeUserId &&
            member.role == 'Owner') {
            count++;
          }
        }
      }
    }
    return count;
  }

  acceptInvitation(teamId: string): void {
    this.noteService.acceptnvitations(teamId).subscribe(
      data => {
        this.getTeamData();
        this.getInviteData();
      }
    )
  }

  getInviteData(): void {
    this.noteService.getMyInvitations().subscribe(
      data => {
        this.inviteTeams = data;
      }
    )
  }

  canEditTeam(teamId: string): boolean {
    if (this.storage.user == null ||
      this.teams == undefined) {
      return false;
    }
    let activeUserId = this.storage.user.userId;
    let count = 0;
    for (let team of this.teams) {
      if (team.members != null) {
        for (let member of team.members) {
          if (member.userId != null &&
            member.userId == activeUserId &&
            (member.role == 'Owner' ||
              member.role == 'ScrumMaster')) {
            return true;
          }
        }
      }
    }
    return false;
  }

  getTeamData(): void {
    this.noteService.getUserTeams().subscribe(
      data => {
        console.log(data);
        this.teams = data;
        console.log('user teams: ' + this.teams);
        //get the retrospective meeting sessions for each team
        for (let team of this.teams) {
          this.noteService.getTeamMeetings(team.teamId).subscribe(
            data => {
              team.meetings = data;

              //assemble summary of team
              team.message = `This team has ${team.members.length} member`;
              if (team.members.length != 1) {
                team.message += 's';
              }
              team.message += ` and ${data.length} retrospective meeting`;
              if (data.length != 1) {
                team.message += 's.';
              } else {
                team.message += '.';
              }

              for (let meeting of data) {
                meeting.message = `This retrospective meeting has ${meeting.categories.length} `;
                if (meeting.categories.length != 0) {
                  meeting.message += 'categories'
                } else {
                  meeting.message += 'category'
                }

                this.noteService.getNotes(meeting.id).subscribe(
                  meetingData => {
                    meeting.message += ` and ${meetingData.length}`;
                    if (meetingData.length != 1) {
                      meeting.message += ' comments';
                    } else {
                      meeting.message += ' comment';
                    }
                  }
                )
              }
            }
          );
        }
      }
    );
  }
}
