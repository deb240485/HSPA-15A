import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {

  constructor(private authService: AuthenticationService,
    private alertify: AlertifyService,
    private router: Router){

  }

  ngOnInit(): void {

  }

  onLogin(loginForm: NgForm){
    console.log(loginForm.value);
    this.authService.authUser(loginForm.value).subscribe(
      (response:any) => {
        console.log(response);
        const user = response;
        localStorage.setItem('userName',user.userName);
        localStorage.setItem('token',user.token);
        this.alertify.success('Login Successful');
        this.router.navigate(['/']);
      },error => {
        console.log(error);
        this.alertify.error(error.error);
      }
    );
  }
}
