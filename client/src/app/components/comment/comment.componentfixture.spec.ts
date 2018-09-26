import { assert } from 'chai';
import { CommentComponent } from './comment.component';
import { TestBed, async, inject, ComponentFixture } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { CommentEditComponent } from '../commentedit/commentedit.component';
import { Comment } from '../../models/comment';
import { Category } from '../../models/category';
import { FormsModule } from '@angular/forms';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';


describe('comment component fixture', () => {
  let commentComponent: CommentComponent;
  let fixture: ComponentFixture<CommentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CommentComponent, CommentEditComponent],
      imports: [FormsModule, RouterTestingModule],
      providers: [{ provide: NotesService, useValue: new NotesServiceMock() }]
    }).compileComponents();

    jasmine.clock().install();
    TestBed.overrideProvider(NotesService, { useValue: new NotesServiceMock() });
    fixture = TestBed.createComponent(CommentComponent);
    fixture.debugElement.injector.get(NotesService);
    commentComponent = fixture.componentInstance;
  });

  afterEach(() => {
    jasmine.clock().uninstall();
  });

  it('meeting name is displayed says test', () => {
    fixture.detectChanges();
    const banner: HTMLElement = fixture.nativeElement.querySelector('h3');
    expect(banner.textContent).toEqual('test');
  });

   it('because the service is mocked, there are two categories', () => {
    fixture.detectChanges();

    const category1 = fixture.nativeElement.querySelector('#category1');
    const category2 = fixture.nativeElement.querySelector('#category2');
    const category3 = fixture.nativeElement.querySelector('#category3');

    expect(category1).toBeDefined();
    expect(category2).toBeDefined();
    expect(category3).toBeNull();
  });

  it('because the service is mocked, there are two comment in category 1', () => {
    fixture.detectChanges();

    const note1 = fixture.nativeElement.querySelector('#comment1');
    const note2 = fixture.nativeElement.querySelector('#comment2');
    const note3 = fixture.nativeElement.querySelector('#comment3');

    expect(note1).toBeDefined();
    expect(note2).toBeDefined();
    expect(note3).toBeNull();
  });


  it('move mouse over note to show footer with edit and delete buttons', () => {

    fixture.detectChanges();
    const nativeElement: HTMLElement = fixture.nativeElement.querySelector('#comment1');
    const note = fixture.nativeElement.querySelector('#comment1');

    expect(note.querySelector('.card-footer').style.display).toBe('none');

    note.dispatchEvent(new MouseEvent('mouseenter', {
      view: window,
      bubbles: true,
      cancelable: true
    }));

    fixture.detectChanges();
    expect(note.querySelector('.card-footer').style.display).toBe('');
  });


  it('clicking the delete button removes it from the collection', () => {
    spyOn(commentComponent, 'onDeleteId');
    fixture.detectChanges();
    const note1 = fixture.nativeElement.querySelector('#comment1');
    const note1DeleteButton = note1.querySelector('.retro-btn-delete');
    fixture.detectChanges();
    note1DeleteButton.dispatchEvent(new Event('click'));
    fixture.detectChanges();

    expect(commentComponent.onDeleteId).toHaveBeenCalled();
  });

  it('clicking the edit button displays the edit note component', () => {
    spyOn(commentComponent, 'onEditId');
    fixture.detectChanges();
    const note1 = fixture.nativeElement.querySelector('#comment1');
    const note1EditButton = note1.querySelector('.retro-btn-edit');
    fixture.detectChanges();
    note1EditButton.dispatchEvent(new Event('click'));
    fixture.detectChanges();

    expect(commentComponent.onEditId).toHaveBeenCalled();
  });

});
