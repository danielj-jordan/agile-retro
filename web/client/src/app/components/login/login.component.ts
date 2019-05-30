import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'
import { User } from '../../models/user';
import { LocalstorageService } from '../../services/localstorage.service';
import { NotesService } from '../../services/notes.service';
import { UserLogin } from '../../models/userlogin';
import { environment } from '../../../environments/environment';
import { UserToken } from '../../models/usertoken';
declare const gapi: any;


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {

  constructor(private router: Router,
    private storage: LocalstorageService,
    private notesService: NotesService) {

  }
  public model = new User();

  public isDemo: boolean;

  public isLoginAllowed: boolean;


  ngOnInit() {
    this.isDemo = environment.demoEnabled;
    this.isLoginAllowed = environment.signupEnabled;
    this.storage.user = null;
    this.storage.userToken = null;


  }

  ngAfterViewInit() {

    // this.notesService.isLoggedIn();
    this.googleInit();

  }


  private clientId: string = '266959264581-v4il3u57njfbhreg38tj013u9ahbf8t5.apps.googleusercontent.com';

  private scope = [
    'profile',
    'email'
  ].join(' ');

  public auth2: any;

  public googleInit() {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: this.clientId,
        cookiepolicy: 'single_host_origin',
        scope: this.scope
      });

      console.log(this.auth2.isSignedIn.get());
      if (this.auth2.isSignedIn.get()) {
        console.log("google signed in")
      }

      this.auth2.isSignedIn.listen( signedIn =>
        {
            if (signedIn) {
              var googleUSer = this.auth2.currentUser.get();
              var profile = googleUSer.getBasicProfile();
              var token = googleUSer.getAuthResponse().id_token;
              console.log('Token || ' + token);
              console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
              console.log('Name: ' + profile.getName());
              console.log('Image URL: ' + profile.getImageUrl());
              console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

              var userToken = new UserToken();
              userToken.token=token;

              this.notesService.loginGoogle(userToken)
              .subscribe(
                data => {
                  console.log('login Google');
                  console.log(data);
                  this.storage.userToken = data.token;
          
                  // save to local storage
                  this.storage.user = this.model;
          
                  // redirect to teams retrospectivelist page
                  this.router.navigateByUrl('/retrospective/list');
                });
          
            }
        });
    });


    //console.log($("#signinGoogle"));
    // this.attachSignin($("#signinGoogle"));
  }

  public onGoogleSignIn(signedIn: boolean) {
    if (signedIn) {
      var googleUSer = this.auth2.currentUser.get();
      var profile = googleUSer.getBasicProfile();
      console.log('Token || ' + googleUSer.getAuthResponse().id_token);
      console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
      console.log('Name: ' + profile.getName());
      console.log('Image URL: ' + profile.getImageUrl());
      console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
    }
  }

  public attachSignin(element: object) {
    console.log(element);
    this.auth2.attachClickHandler(element, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        console.log('Token || ' + googleUser.getAuthResponse().id_token);
        console.log('ID: ' + profile.getId());


        this.notesService.loginGoogle(googleUser.getAuthResponse().id_token);

      }, function (error) {
        console.log(JSON.stringify(error, undefined, 2));
      });
  }



  public LoginDemo(): void {

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


  public Login(): void {
    console.log('logging in user: ' + this.model.loginName);

    if (!this.isLoginAllowed) {
      return;
    }


    //verify login -- assume valid for now
    let login = new UserLogin();
    login.LoginName = this.model.loginName;


    this.notesService.login(login).subscribe(
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
