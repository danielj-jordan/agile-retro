import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms'
import {User} from '../../models/user';
import { LocalstorageService } from '../../services/localstorage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router,
              private storage: LocalstorageService) { }
    public  model = new User();


  ngOnInit() {


  }


  public Login():void{
    console.log('logging in user: ' + this.model.LoginName);  
    
    
    //verify login -- assume valid for now
    
    //save to local storage
    this.storage.userEmail=this.model.LoginName;

    //redirect to teams retrospectivelist page
    this.router.navigateByUrl('/list');



  }


}
