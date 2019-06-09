import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {NotesServiceMock} from '../../services/notes.service.mock';
import {NotesService} from '../../services/notes.service';
import { LoginComponent } from './login.component';
import { LocalstorageService } from '../../services/localstorage.service';
import { GoogleAuthService } from '../..//services/google-auth.service';
import { GoogleAuthServiceMock } from '../../services/google-auth.service.mock';


describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginComponent ],
      imports: [FormsModule, RouterTestingModule.withRoutes([])],
      providers: [LocalstorageService, 
        { provide: NotesService, useValue: new NotesServiceMock()} , 
        {provide: GoogleAuthService, useValue: new GoogleAuthServiceMock()
      }]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
