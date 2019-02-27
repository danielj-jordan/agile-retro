import { Injectable } from '@angular/core';
import {User} from '../models/user';

@Injectable()
export class LocalstorageService {

  constructor() { }


  set user(activeUser: User){
    sessionStorage.setItem("user", JSON.stringify(activeUser));
  }

  get user(): User{
    return JSON.parse(sessionStorage.getItem("user"));
  }

  set userToken(token: string){
    sessionStorage.setItem("token", token);
  }

  get userToken():string {
    return sessionStorage.getItem("token");
  }

  

}