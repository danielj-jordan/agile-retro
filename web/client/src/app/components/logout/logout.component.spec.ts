import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutComponent } from './logout.component';
import { RouterTestingModule } from '@angular/router/testing';
import { LocalstorageService } from '../../services/localstorage.service';
import { GoogleAuthService } from '../../services/google-auth.service';
import { GoogleAuthServiceMock } from '../../services/google-auth.service.mock';

describe('LogoutComponent', () => {
  let component: LogoutComponent;
  let fixture: ComponentFixture<LogoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogoutComponent ],
      imports: [RouterTestingModule.withRoutes([])],
      providers: [LocalstorageService, 
        {provide: GoogleAuthService, useValue: new GoogleAuthServiceMock()}]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
