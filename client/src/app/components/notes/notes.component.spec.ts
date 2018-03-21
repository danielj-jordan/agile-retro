import { assert } from 'chai';
import { NotesComponent } from './notes.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
//import { ControlContainer } from '@angular/forms/src/directives/control_container';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
//import { HttpModule } from '@angular/http/src/http_module';
//import {Http} from '@angular/http';
//import {MockBackend} from '@angular/http/testing'



let notesComponet: NotesComponent;
let noteService: NotesService;
let fixture: ComponentFixture<NotesComponent>;
let component: NotesComponent;


describe('notes component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [],
            imports:[FormsModule],
            providers:[NotesComponent,
             {provide: NotesService, useClass: NotesServiceMock},
        ]

    }).compileComponents();
  // TestBed.overrideProvider(NotesService, {useValue: NotesServiceMock});

   notesComponet=TestBed.get(NotesComponent);
  // fixture = TestBed.createComponent(NotesComponent);
   //notesComponet = fixture.componentInstance;
   //fixture.detectChanges();
   noteService=TestBed.get(NotesService); 

    //notesComponet= TestBed.createComponent(NotesComponent).componentInstance;

   
    //TestBed.initTestEnvironment()
    
    
 
   
    

       
    });

    it('notes should be zero or higher', async(() => {
        notesComponet.ngOnInit();
        console.log('notescompoent:' + notesComponet.notes.length);
        expect(notesComponet.notes.length).toBeGreaterThanOrEqual(0);
    }));

    it('clicking on a note sets selected note', async(() => {
    
    }));

    it('closing the note edit box, clears the selected note', async(() => {
       
    }));

    it('deleting the note removes it from the local collection', async(() => {
        /*
       let beforeCount:number= notesComponet.count.valueOf();
        
       const  deleteButton = fixture.nativeElement.querySelector('.btn-secondary');
        
        deleteButton.click();
        fixture.detectChanges();
        
       expect(notesComponet.count).toBeLessThan(beforeCount);
       expect(notesComponet.onDelete).toHaveBeenCalled();
*/


    }));



});
