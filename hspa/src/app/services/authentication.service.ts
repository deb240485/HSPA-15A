import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUser, IUserLogin } from '../model/IUser';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseURL = environment.baseUrl;

  constructor(private http: HttpClient) { }

  authUser(user: IUserLogin){
    return this.http.post(this.baseURL + '/Account/Login',user);
  }

  registerUser(user: IUser){
    return this.http.post(this.baseURL+'/Account/Register',user);
  }
}
