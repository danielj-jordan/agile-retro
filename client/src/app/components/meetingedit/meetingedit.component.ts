import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NotesService } from '../../services/notes.service';
import { Team } from '../../models/team';
import { Meeting } from '../../models/meeting';
import { Category } from '../../models/category';

@Component({
  selector: 'app-meetingedit.component',
  templateUrl: './meetingedit.component.html',
  styleUrls: ['./meetingedit.component.css']
})
export class MeetingEditComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private notesService: NotesService) {
  }

  public team: Team;
  public meeting: Meeting;

  ngOnInit() {

    this.meeting = new Meeting();
    this.meeting.name = '';

    this.team = new Team();
    this.team.name = '';


    // get the teamid id
    let teamId = this.route.snapshot.paramMap.get('teamid');

    let meetingId = this.route.snapshot.paramMap.get('meetingid');

    // get team info
    this.notesService.getTeam(teamId).subscribe(
      data => {
        this.team = data;
      }
    );

    console.log('meetingId: ' + meetingId);
    console.log('teamid: ' + teamId);

    if (meetingId != null) {
      // editing existing meeting
      this.notesService.getMeeting(meetingId).subscribe(
        data => {
          this.meeting = data;
          console.log(this.meeting);
        });
    } else {
      this.initialEmptyMeeting(teamId);
    }


  }

  initialEmptyMeeting(teamId: string): void{
    this.meeting = new Meeting();
    this.meeting.teamId = teamId;
    this.meeting.categories=[];
  }

  save(): void {
    this.notesService.saveMeeting(this.meeting).subscribe(
      data => {

      }
    );
    this.navigateToList();
  }

  addCategory(): void {
    // add the category to the list
    let category = new Category();
    category.categoryNum = this.nextCategoryNumber();
    category.name = 'category';
    category.sortOrder = this.nextSortNumber();

    this.meeting.categories.push(category);

  }

  nextCategoryNumber(): number {
    // find the maximum category number and return a higher value
    let next = 1;
    
    for (let cat of this.meeting.categories) {
      if (cat.categoryNum >= next) {
        next = cat.categoryNum + 1;
      }
    }
    
    return next;
  }

  nextSortNumber(): number {
    let next = 1;

    for (let cat of this.meeting.categories) {
      if (cat.sortOrder >= next) {
        next = cat.sortOrder + 1;
      }
    }
    
    return next;
  }

  navigateToList(): void {
    this.router.navigateByUrl('/list');
  }

  categoryDomId(categoryNum: number): string {
    return 'category' + categoryNum;

  }

  categoryTracking(index: number, category: Category): number {
    return category.categoryNum;
  }


  categorySort(): void {
    this.meeting.categories.sort((a, b) => {
      if (a.sortOrder < b.sortOrder) {
        return -1;
      }
      if (a.sortOrder > b.sortOrder) {
        return 1;
      } else {
        return 0;
      }
    });
  }

  categoryMoveUp(categoryNum: number) {
    for (let i = 0; i < this.meeting.categories.length; i++) {
      if (this.meeting.categories[i].categoryNum === categoryNum) {
        if (i <= 0) {
          return;
        }
        let tempSortOrder = this.meeting.categories[i].sortOrder;
        this.meeting.categories[i].sortOrder = this.meeting.categories[i - 1].sortOrder;
        this.meeting.categories[i - 1].sortOrder = tempSortOrder;
        break;
      }
    }
    this.categorySort();
  }

  categoryMoveDown(categoryNum: number) {
    for (let i = 0; i < this.meeting.categories.length; i++) {
      if (this.meeting.categories[i].categoryNum === categoryNum) {
        if (i > this.meeting.categories.length - 1) {
          return;
        }
        let tempSortOrder = this.meeting.categories[i].sortOrder;
        this.meeting.categories[i].sortOrder = this.meeting.categories[i + 1].sortOrder;
        this.meeting.categories[i + 1].sortOrder = tempSortOrder;
        break;
      }
    }
    this.categorySort();
  }

  categoryDelete(categoryNum: number): void {
    console.log('delete');
    for (let i = 0; i < this.meeting.categories.length; i++) {
      if (this.meeting.categories[i].categoryNum === categoryNum) {
        console.log('delete' + categoryNum);
        this.meeting.categories.splice(i, 1);
      }
    }
  }



}
