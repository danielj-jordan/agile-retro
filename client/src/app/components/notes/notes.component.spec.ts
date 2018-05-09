import { assert } from 'chai';
import { NotesComponent } from './notes.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {ComponentEdit} from '../componentedit/componentedit.component';
import {Note} from '../../services/notes';
import {Category} from '../../services/category';
//import { ControlContainer } from '@angular/forms/src/directives/control_container';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
//import { HttpModule } from '@angular/http/src/http_module';
//import {Http} from '@angular/http';
//import {MockBackend} from '@angular/http/testing'




describe('notes component', () => {

    let notesComponet: NotesComponent;
    let noteService: NotesService;



    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [NotesComponent, ComponentEdit],
            imports:[FormsModule ],
            providers:[NotesComponent,
             {provide: NotesService, useClass: NotesServiceMock},
        ]

    }).compileComponents();

   notesComponet=TestBed.get(NotesComponent);
   noteService=TestBed.get(NotesService); 

    

       
    });
  

    it('notes should be zero or higher', async(() => {
        notesComponet.ngOnInit();
        console.log('notescompoent:' + notesComponet.notes.length);
        expect(notesComponet.notes.length).toBeGreaterThanOrEqual(0);
    }));

 





});
