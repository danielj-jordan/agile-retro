import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import { MeetingEditComponent } from './meetingedit.component';
import { FormsModule } from '@angular/forms';
import {NotesService} from '../../services/notes.service';
import { NotesServiceMock} from '../../services/notes.service.mock';


describe('MeetingEditComponent', () => {
  let component: MeetingEditComponent;
  let fixture: ComponentFixture<MeetingEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MeetingEditComponent ],
      imports: [ RouterTestingModule, FormsModule],
      providers: [  {provide: NotesService, useClass: NotesServiceMock}]

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
