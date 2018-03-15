import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoteComponent2Component } from './note-component2.component';

describe('NoteComponent2Component', () => {
  let component: NoteComponent2Component;
  let fixture: ComponentFixture<NoteComponent2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoteComponent2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoteComponent2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
