import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentEditComponent } from './commentedit.component';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { Component } from '@angular/core';
import { Comment } from '../../models/comment';

describe('CommentEditComponent', () => {
  let component: CommentEditComponent;
  let fixture: ComponentFixture<CommentEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CommentEditComponent],
      providers: [{ provide: NotesService, useValue: new NotesServiceMock() }]
    }).compileComponents();


    TestBed.overrideProvider(NotesService, { useValue: new NotesServiceMock() });

  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('can be shown', () => {

    let commentText = 'sample comment text';
    let comment = new Comment();
    comment.text = commentText;
    comment.categoryNum = 2;
    component.setComment(comment);
    component.show();

    const text = fixture.nativeElement.querySelector("#text");
    expect(text.value).toEqual(commentText);

  });


});
