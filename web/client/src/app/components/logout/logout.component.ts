import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalstorageService } from '../../services/localstorage.service';
import { GoogleAuthService} from '../../services/google-auth.service';



@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements AfterViewInit {

  constructor(private router: Router,
    private storage: LocalstorageService,
    private googleAuth: GoogleAuthService) { }

  ngAfterViewInit() {    
    console.log("logging out");
    this.storage.deleteAll();
    this.googleAuth.signOut();
    this.router.navigateByUrl('');
  }
}
