import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { MeetingEditComponent } from './meetingedit.component';
import { FormsModule } from '@angular/forms';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {IconsModule} from '../../icons/icons.module';


describe('MeetingEditComponent', () => {
  let component: MeetingEditComponent;
  let fixture: ComponentFixture<MeetingEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [MeetingEditComponent],
      imports: [RouterTestingModule.withRoutes([]), FormsModule, IconsModule],
      providers: [
        { provide: NotesService, useClass: NotesServiceMock },
        {
          provide: ActivatedRoute, useValue: {
            snapshot:
            {
              paramMap: convertToParamMap({
                teamid: '1', meetingid: '2'
              })
            }
          }
        }
      ]
    })
      .compileComponents();
  }));

  /*
    all of these tests use the NoteServiceMock which initializes with categories
    in the meeting
  */

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('sort the list of categories', () => {
    component.categorySort();
    expect(component.meeting.categories[0].sortOrder).toEqual(1);
    expect(component.meeting.categories[component.meeting.categories.length - 1].sortOrder).
      toEqual(component.meeting.categories.length);
  });

  it('move a category up in the list', () => {
    component.categoryMoveUp(2);
    expect(component.meeting.categories[0].categoryNum).toEqual(2);
  });

  it('move a category down in the list', () => {
    component.categoryMoveDown(2);
    expect(component.meeting.categories[2].categoryNum).toEqual(2);
  });

  it('delete a category from  the list', () => {
    component.categoryDelete(2);
    expect(component.meeting.categories.length).toEqual(2);
  });

  it('add a category', () => {
    component.addCategory();
    expect(component.meeting.categories.length).toEqual(4);
  });

  it('clicking on add category, calls add category', () => {
    spyOn(fixture.componentInstance, 'addCategory');

    const button = fixture.nativeElement.querySelector('#btnAddCategory');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));

    expect(fixture.componentInstance.addCategory).toHaveBeenCalled();

  });

  it('clicking on move up', () => {
    spyOn(fixture.componentInstance, 'categoryMoveUp');
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('#divCategory2>div:nth-child(1)>div:nth-child(2)>button:nth-child(1)');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));

    expect(fixture.componentInstance.categoryMoveUp).toHaveBeenCalled();
  });

  it('clicking on move down', () => {
    spyOn(fixture.componentInstance, 'categoryMoveDown');
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('#divCategory2>div:nth-child(1)>div:nth-child(2)>button:nth-child(2)');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));
    fixture.detectChanges();
    expect(fixture.componentInstance.categoryMoveDown).toHaveBeenCalled();
  });

  it('clicking on delete', () => {
    spyOn(fixture.componentInstance, 'categoryDelete');
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('#divCategory2>div:nth-child(1)>div:nth-child(2)>button:nth-child(3)');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));

    expect(fixture.componentInstance.categoryDelete).toHaveBeenCalled();
  });

  it('clicking on save', () => {
    spyOn(fixture.componentInstance, 'save');

    const button = fixture.nativeElement.querySelector('#btnSave');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));

    expect(fixture.componentInstance.save).toHaveBeenCalled();
  });

  it('clicking on cancel', () => {
    spyOn(fixture.componentInstance, 'navigateToList');

    const button = fixture.nativeElement.querySelector('#btnCancel');
    fixture.detectChanges();
    button.dispatchEvent(new Event('click'));

    expect(fixture.componentInstance.navigateToList).toHaveBeenCalled();
  });

  it('can add category to empty meeting', ()=>{
    component.initialEmptyMeeting('123');
    component.addCategory();

    expect(component.meeting.categories.length).toEqual(1);
    
  });


});
