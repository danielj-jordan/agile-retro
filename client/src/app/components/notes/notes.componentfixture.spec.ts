import { assert } from 'chai';
import { NotesComponent } from './notes.component';
import { TestBed, async, inject, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';





describe('notes component fixture', () => {
    let notesComponet: NotesComponent;
    let noteService: NotesService;
    let fixture: ComponentFixture<NotesComponent>;
    let component: NotesComponent;
    
    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [NotesComponent],
            imports:[FormsModule],
            providers: [{provide: NotesService, useValue: new NotesServiceMock() }]
        

    }).compileComponents();

    TestBed.overrideProvider(NotesService, {useValue: new NotesServiceMock() });
    fixture = TestBed.createComponent(NotesComponent);
    fixture.debugElement.injector.get(NotesService);
    notesComponet = fixture.componentInstance;
 
    
    
          
    });

    it('header says notes', async(() =>{
        fixture.detectChanges();

        const banner: HTMLElement=fixture.nativeElement.querySelector("h1");
        expect(banner.textContent).toEqual("Notes");

    }));

    it('clicking on an existing comment sets the selected node', async (() =>{

        fixture.detectChanges();
        const nativeElement: HTMLElement= fixture.nativeElement;
        nativeElement.querySelector('div[name="note"]').dispatchEvent(new Event('click'));
        fixture.detectChanges();

      //  expect(notesComponet.selectedNote.commentId).toBeGreaterThan(0);
        
    }));

  
/*
    it('deleting the note removes it from the local collection', async(() => {

        fixture.detectChanges();
        let beforeCount:number= notesComponet.count.valueOf();

        const nativeElement: HTMLElement= fixture.nativeElement;
       // const debugElement :DebugElement=fixture.debugElement;
       
        //debugElement.query(By.css('div[name="note"]')).

        //onsole.log(nativeElement.querySelector('div[name="note"]').innerHTML);
        nativeElement.querySelector('div[name="note"]').dispatchEvent(new Event('click'));
        fixture.detectChanges();
                
        const  deleteButton = fixture.debugElement.nativeElement.querySelector('.btn-secondary');
        
        deleteButton.click();
        fixture.detectChanges();
      
        expect(notesComponet.count).toBeLessThan(beforeCount);
       // expect(notesComponet.onDelete).toHaveBeenCalled();



    }));
    */



});
