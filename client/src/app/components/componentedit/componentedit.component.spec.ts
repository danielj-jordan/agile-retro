import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentEdit} from './componentedit.component';

describe('ComponenteditComponent', () => {
  let component: ComponentEdit;
  let fixture: ComponentFixture<ComponentEdit>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComponentEdit ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
