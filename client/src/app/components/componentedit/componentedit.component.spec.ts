import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentEdit} from './componentedit.component';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { Component } from '@angular/core';

describe('ComponenteditComponent', () => {
  let component: ComponentEdit;
  let fixture: ComponentFixture<ComponentEdit>;
  //let noteService: NotesService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComponentEdit ],
     // providers: [ ComponentEdit, {provide: NotesService, useClass: NotesServiceMock}],
      providers: [{provide: NotesService, useValue: new NotesServiceMock() }]
  }).compileComponents();

    //component= TestBed.get(ComponentEdit);
    //noteService=TestBed.get(NotesService); 
    TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
   
     
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
