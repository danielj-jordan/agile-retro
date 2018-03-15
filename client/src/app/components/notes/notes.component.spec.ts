/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { NotesComponent } from './notes.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import { ControlContainer } from '@angular/forms/src/directives/control_container';
//import { HttpModule } from '@angular/http/src/http_module';
//import {Http} from '@angular/http';
//import {MockBackend} from '@angular/http/testing'



let notesComponet: NotesComponent;
//let noteService: NotesService;


describe('notes component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [NotesComponent],
            imports:[],
            providers:[
            NotesComponent, {provide: NotesService, useClass: NotesServiceMock},
            //{provide: Http, useClass: MockBackend},
            //{provide: 'BASE_URL', useValue: 'http://localhost:5000'}
        ]

    });
    notesComponet=TestBed.get(NotesComponent);
   
    //TestBed.initTestEnvironment()
    
    
 
   
    

       
    });

    it('notes should be zero or higher', async(() => {
        console.log('notescompoent:' + notesComponet.notes.length);
        expect(notesComponet.notes.length).toBeGreaterThanOrEqual(0);
    }));

    it('clicking on a note sets selected note', async(() => {
    
    }));

    it('closing the note edit box, clears the selected note', async(() => {
       
    }));

    it('deleting the note removes it from the local collection', async(() => {

        let fixture: ComponentFixture<NotesComponent>;
        //TestBed.compileComponents();
        TestBed.overrideProvider(NotesService, {useValue: NotesServiceMock});
        TestBed.compileComponents();
        fixture=TestBed.createComponent(NotesComponent);
        fixture.detectChanges();

      //  let beforeCount:number= notesComponet.count.valueOf();
        
       const  deleteButton = fixture.nativeElement.querySelector('.btn-secondary');
        
        deleteButton.click();
       // fixture.detectChanges();
        
       // expect(notesComponet.count).toBeLessThan(beforeCount);
      //  expect(notesComponet.onDelete).toHaveBeenCalled();



    }));



});
