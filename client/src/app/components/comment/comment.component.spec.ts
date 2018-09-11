import { assert } from 'chai';
import { CommentComponent } from './comment.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { NotesService } from '../../services/notes.service';
import { NotesServiceMock } from '../../services/notes.service.mock';
import {CommentEditComponent} from '../commentedit/commentedit.component';
import {Comment} from '../../models/comment';
import {Category} from '../../models/category';
import {RouterTestingModule} from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import {Observable} from 'rxjs/Rx';
import { doesNotThrow } from 'assert';





describe('comment component', () => {

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

    jasmine.clock().install();
    //jasmine.DEFAULT_TIMEOUT_INTERVAL = 20000;

   notesComponet=TestBed.get(CommentComponent);
   noteService=TestBed.get(NotesService); 
   //notesComponet.cancelTimer();

   

       
    });

    afterEach(()=>{
        jasmine.clock().uninstall();
    });
  

    it('comments should be zero or higher',() => {
     //   jasmine.clock().tick(100);
        notesComponet.ngOnInit();
        
        console.log('notescompoent:' + notesComponet.comments.length);
        expect(notesComponet.comments.length).toBeGreaterThanOrEqual(0);

    });



});
