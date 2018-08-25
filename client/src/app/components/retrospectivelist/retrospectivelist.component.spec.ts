import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RetrospectivelistComponent } from './retrospectivelist.component';

describe('RetrospectivelistComponent', () => {
  let component: RetrospectivelistComponent;
  let fixture: ComponentFixture<RetrospectivelistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RetrospectivelistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RetrospectivelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
