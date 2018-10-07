import { Injectable } from '@angular/core';

@Injectable()
export class LocalstorageService {

  constructor() { }


  set userEmail(email: string){
    sessionStorage.setItem("user", email);
  }

  get userEmail(): string{
    return sessionStorage.getItem("user");
  }

  set userToken(token: string){
    sessionStorage.setItem("token", token);
  }

  get userToken():string {
    return sessionStorage.getItem("token");
  }
}
