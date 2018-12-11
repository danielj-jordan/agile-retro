import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms'
import {User} from '../../models/user';
import { LocalstorageService } from '../../services/localstorage.service';
import {NotesService} from '../../services/notes.service';
import { UserLogin } from '../../models/userlogin';
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router,
              private storage: LocalstorageService,
              private notesService: NotesService) { 

              }
  public  model = new User();

  public isDemo: boolean;


  ngOnInit() {
    this.isDemo=environment.demoEnabled;


  }

  

  public LoginDemo():void{

    this.notesService.loginDemo().subscribe(
      data=>{
        console.log('test');
        console.log(data);
        this.storage.userToken=data.token;

        // save to local storage
        this.storage.user=this.model;

        // redirect to teams retrospectivelist page
        this.router.navigateByUrl('/list');
      });

  }


  public Login():void{
    console.log('logging in user: ' + this.model.loginName);  
    
    
    //verify login -- assume valid for now
    let login = new UserLogin();
    login.LoginName=this.model.loginName;


    this.notesService.login(login).subscribe(
      data=>{
        console.log('test');
        console.log(data);
        this.storage.userToken=data.token;

        

        // save to local storage
        this.storage.user=this.model;

        // redirect to teams retrospectivelist page
        this.router.navigateByUrl('/list');
      });
  }


}
