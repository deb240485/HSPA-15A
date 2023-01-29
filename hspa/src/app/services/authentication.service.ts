import { Injectable } from '@angular/core';
import { IUser } from '../model/IUser';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor() { }

  authUser(user: IUser){
    let userArray = [];
    if(localStorage.getItem('Users')){
      userArray = JSON.parse(localStorage.getItem('Users')!);
    }
    return userArray.find((u: { userName: string; password: string; }) =>u.userName === user.userName && u.password === user.password);
  }
}
