import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { LocalstorageService } from '../services/localstorage.service';
import { NotesService } from '../services/notes.service';
import { UserLogin } from '../models/userlogin';
import { UserToken } from '../models/usertoken';
import { User } from '../models/user';

declare const gapi: any;

  
@Injectable({
  providedIn: 'root'
})
export class GoogleAuthService {



  private clientId: string = '266959264581-v4il3u57njfbhreg38tj013u9ahbf8t5.apps.googleusercontent.com';

  private scope = [
    'profile',
    'email'
  ].join(' ');

  public auth2: any;

  private isInitialized: boolean;

  constructor(private router: Router,
    private storage: LocalstorageService,
    private notesService: NotesService,
    private ngZone: NgZone) {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: this.clientId,
        cookiepolicy: 'single_host_origin',
        scope: this.scope
      });
      console.log("google auth is initialized.")
    });
  }



  public init(): void {
    if (this.isInitialized === true) return;
    this.addListener();
    console.log("google Auth services says the user is signed in with Google: " + this.auth2.isSignedIn.get());
    this.isInitialized = true;
  }

  public signIn(): void {

      console.log("signing in via google")

      this.init();

      var options = new gapi.auth2.SigninOptionsBuilder();
      // options.setFetchBasicProfile(true);
      options.setPrompt('select_account');
      options.setScope('profile').setScope('email profile');



      this.signOut();
      console.log("is signed in: " + this.auth2.isSignedIn.get());
      this.auth2.signIn(options);
      console.log("is signed in: " + this.auth2.isSignedIn.get());

  }

  private addListener(): void{

    this.auth2.isSignedIn.listen((signedIn) => {
      console.log("google signed in: " + signedIn);
      if (this.auth2.isSignedIn.get() === true || true) {
        var googleUSer = this.auth2.currentUser.get();
        var profile = googleUSer.getBasicProfile();
        var token = googleUSer.getAuthResponse().id_token;
        console.log('Token || ' + token);
        console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

        var userToken = new UserToken();
        userToken.token = token;

        this.notesService.loginGoogle(userToken)
          .subscribe(
            data => {
              console.log('login Google');
              console.log(data);
              if (data) {
                this.storage.userToken = data.token;

                // save to local storage
                let activeUser = new User();
                activeUser.userId=data.userId;
                activeUser.isDemoUser=data.isDemoUser;
                this.storage.user=activeUser;

                // redirect to teams retrospectivelist page
                this.ngZone.run(() => {
                  this.router.navigateByUrl('/retrospective/list');
                });
              }
            });
      }
    });
  }
  public signOut(): void {
    if (this.auth2) {
      this.auth2.signOut();
      this.router.navigateByUrl('/login');
    }
  }

}



