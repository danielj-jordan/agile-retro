import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import { RetrospectivelistComponent } from './retrospectivelist.component';
import { Router } from '@angular/router';
import {FormsModule} from '@angular/forms'
import { LocalstorageService } from '../../services/localstorage.service';
import {NotesService} from '../../services/notes.service'
import { NotesServiceMock } from '../../services/notes.service.mock';


describe('RetrospectivelistComponent', () => {
  let component: RetrospectivelistComponent;
  let fixture: ComponentFixture<RetrospectivelistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, RouterTestingModule],
      declarations: [ RetrospectivelistComponent  ],
      providers: [ LocalstorageService, {provide: NotesService, useValue: new  NotesServiceMock() }]
    })
    .compileComponents();
   // TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
  }));

  beforeEach(() => {
  
    fixture = TestBed.createComponent(RetrospectivelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
