import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'
import { User } from '../../models/user';
import { LocalstorageService } from '../../services/localstorage.service';
import { NotesService } from '../../services/notes.service';
import { GoogleAuthService } from '../../services/google-auth.service';
import { UserLogin } from '../../models/userlogin';
import { environment } from '../../../environments/environment';
import { UserToken } from '../../models/usertoken';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {

  constructor(private router: Router,
    private storage: LocalstorageService,
    private notesService: NotesService,
    private googleAuth: GoogleAuthService) {
    this.isDemo = environment.demoEnabled;
    this.isLoginAllowed = environment.signupEnabled;
    this.storage.user = null;
    this.storage.userToken = null;
  }

  public model = new User();

  public isDemo: boolean;

  public isLoginAllowed: boolean;

  ngOnInit() {
  }

  ngAfterViewInit() {
  }

  public loginGoogle(): void {
    this.googleAuth.signIn();
  }

  public loginDemo(): void {

    this.notesService.loginDemo().subscribe(
      data => {
        console.log('test');
        console.log(data);
        this.storage.userToken = data.token;

        // save to local storage
        this.storage.user = this.model;

        // redirect to teams retrospectivelist page
        this.router.navigateByUrl('/retrospective/list');
      });

  }

}
