import { assert } from 'chai';
import { CommentComponent } from './comment.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {CommentEditComponent} from '../commentedit/commentedit.component';
import {Comment} from '../../models/comment';
import {Category} from '../../models/category';
//import { ControlContainer } from '@angular/forms/src/directives/control_container';
import {RouterTestingModule} from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
//import { HttpModule } from '@angular/http/src/http_module';
//import {Http} from '@angular/http';
//import {MockBackend} from '@angular/http/testing'




describe('notes component', () => {

    let notesComponet: CommentComponent;
    let noteService: NotesService;



    beforeEach(() => {
        TestBed.configureTestingModule({ 
            declarations: [CommentComponent, CommentEditComponent],
            imports:[FormsModule, RouterTestingModule],
            providers:[CommentComponent,
             {provide: NotesService, useClass: NotesServiceMock},
        ]

    }).compileComponents();

   notesComponet=TestBed.get(CommentComponent);
   noteService=TestBed.get(NotesService); 

    

       
    });
  

    it('notes should be zero or higher', async(() => {
        notesComponet.ngOnInit();
        console.log('notescompoent:' + notesComponet.comments.length);
        expect(notesComponet.comments.length).toBeGreaterThanOrEqual(0);
    }));



});
