import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { TeamEditComponent } from './teamedit.component';

describe('TeameditComponent', () => {
  let component: TeamEditComponent;
  let fixture: ComponentFixture<TeamEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamEditComponent ],
      imports: [RouterTestingModule.withRoutes([]), FormsModule],
      providers: [
        { provide: NotesService, useClass: NotesServiceMock },
        {
          provide: ActivatedRoute, useValue: {
            snapshot:
            {
              paramMap: convertToParamMap({
                teamid: '1'
              })
            }
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
