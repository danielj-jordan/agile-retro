import { assert } from 'chai';
import { NotesComponent } from './notes.component';
import { TestBed, async, inject, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {ComponentEdit} from '../componentedit/componentedit.component';
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';





describe('notes component fixture', () => {
    let notesComponent: NotesComponent;
    let noteService: NotesService;
    let fixture: ComponentFixture<NotesComponent>;
    let component: NotesComponent;
    
    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [NotesComponent, ComponentEdit],
            imports:[FormsModule],
            providers: [{provide: NotesService, useValue: new NotesServiceMock() }]
        

    }).compileComponents();

    TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
    fixture = TestBed.createComponent(NotesComponent);
    fixture.debugElement.injector.get(NotesService);
    notesComponent = fixture.componentInstance;
 
    
    
          
    });

    it('header says notes', async(() =>{
        fixture.detectChanges();

        const banner: HTMLElement=fixture.nativeElement.querySelector("h3");
        expect(banner.textContent).toEqual("Notes");

    }));

    it('because the service is mocked, there are two categories', async (() =>{
        fixture.detectChanges();

        const category1 = fixture.nativeElement.querySelector("#category1");
        const category2 = fixture.nativeElement.querySelector("#category2");
        const category3 = fixture.nativeElement.querySelector("#category3");

        expect(category1).toBeDefined();
        expect(category2).toBeDefined();
        expect(category3).toBeNull();
    }));

    it('because the service is mocked, there are two comment in category 1', async (() =>{
        fixture.detectChanges();

        //todo: still need to check for category

        const note1 = fixture.nativeElement.querySelector("#note1");
        const note2 = fixture.nativeElement.querySelector("#note2");
        const note3 = fixture.nativeElement.querySelector("#note3");

        expect(note1).toBeDefined();
        expect(note2).toBeDefined();
        expect(note3).toBeNull();
    
    }));


    it('move mouse over note to show footer with edit and delete buttons', async (() =>{

      fixture.detectChanges();
      const nativeElement: HTMLElement= fixture.nativeElement.querySelector('#note1');
      var note= fixture.nativeElement.querySelector('#note1');

      expect(note.querySelector(".card-footer").style.display).toBe('none');
       
      note.dispatchEvent(new MouseEvent('mouseenter', {
        view: window,
        bubbles: true,
        cancelable: true
      })); 

      fixture.detectChanges();
      expect(note.querySelector(".card-footer").style.display).toBe('');        
    }));

  

    it('clicking the delete button removes it from the collection', async(() => {
        
        spyOn(notesComponent,"onDeleteId");
        fixture.detectChanges();
        const note1 = fixture.nativeElement.querySelector("#note1");
        const note1DeleteButton= note1.querySelector(".retro-btn-delete");
        fixture.detectChanges();
        note1DeleteButton.dispatchEvent(new Event('click'));
        fixture.detectChanges();
    
        expect(notesComponent.onDeleteId).toHaveBeenCalled();
    }));
    
    it('clicking the edit button displays the edit note component', async(() => {
        spyOn(notesComponent,"onEditId");
        fixture.detectChanges();
        const note1 = fixture.nativeElement.querySelector("#note1");
        const note1EditButton= note1.querySelector(".retro-btn-edit");
        fixture.detectChanges();
        note1EditButton.dispatchEvent(new Event('click'));
        fixture.detectChanges();
    
        expect(notesComponent.onEditId).toHaveBeenCalled();


    }));


});
