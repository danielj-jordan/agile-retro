import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import { MeetingListComponent } from './meetinglist.component';
import { Router } from '@angular/router';
import {FormsModule} from '@angular/forms'
import { LocalstorageService } from '../../services/localstorage.service';
import {NotesService} from '../../services/notes.service'
import { NotesServiceMock } from '../../services/notes.service.mock';


describe('MeetingListComponent', () => {
  let component: MeetingListComponent;
  let fixture: ComponentFixture<MeetingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, RouterTestingModule],
      declarations: [ MeetingListComponent  ],
      providers: [ LocalstorageService, {provide: NotesService, useValue: new  NotesServiceMock() }]
    })
    .compileComponents();
   // TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
  }));

  beforeEach(() => {
  
    fixture = TestBed.createComponent(MeetingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
