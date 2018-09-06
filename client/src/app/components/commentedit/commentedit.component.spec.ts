import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentEditComponent} from './commentedit.component';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { Component } from '@angular/core';

describe('CommentEditComponent', () => {
  let component: CommentEditComponent;
  let fixture: ComponentFixture<CommentEditComponent>;
  //let noteService: NotesService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentEditComponent ],
     // providers: [ ComponentEdit, {provide: NotesService, useClass: NotesServiceMock}],
      providers: [{provide: NotesService, useValue: new NotesServiceMock() }]
  }).compileComponents();

    //component= TestBed.get(ComponentEdit);
    //noteService=TestBed.get(NotesService); 
    TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
   
     
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
