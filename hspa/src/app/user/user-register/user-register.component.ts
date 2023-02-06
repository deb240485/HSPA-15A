import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { IUser } from '../../model/IUser';
import { AlertifyService } from '../../services/alertify.service';
import { AuthenticationService } from '../../services/authentication.service';


@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit{

  public registerationForm!: FormGroup;

  user!: IUser;

  userSubmitted: boolean = false;

  constructor(private fb: FormBuilder , private authService: AuthenticationService, private alertify: AlertifyService) {

  }

  ngOnInit(): void {
    // this.registerationForm = new FormGroup({
    //   userName: new FormControl(null,Validators.required),
    //   userEmail: new FormControl(null,[Validators.required,Validators.email]),
    //   password: new FormControl(null,[Validators.required,Validators.minLength(6)]),
    //   confirmPassword: new FormControl(null,[Validators.required]),
    //   mobile: new FormControl(null,[Validators.required,Validators.maxLength(10)])
    // },this.passwordMatchingValidator);

    this.createRegistrationForm();
  }

  createRegistrationForm(){
    this.registerationForm = this.fb.group({
      userName: [null,Validators.required],
      userEmail: [null,[Validators.required,Validators.email]],
      password: [null,[Validators.required,Validators.minLength(6)]],
      confirmPassword: [null,Validators.required],
      mobile: [null,[Validators.required,Validators.maxLength(10)]]
    },{validators: this.passwordMatchingValidator});
  }

  passwordMatchingValidator(fc: AbstractControl): ValidationErrors | null {
    return fc.get('password')?.value === fc.get('confirmPassword')?.value ? null :
      { notmatched: true };
  };

  userData() : IUser {
    return this.user = {
      userName: this.userName.value,
      userEmail: this.userEmail.value,
      password: this.password.value,
      mobile : this.mobile.value
    }
  }

  //Getter method for all form controls

  get userName(){
    return this.registerationForm.get('userName') as FormControl;
  }

  get userEmail(){
    return this.registerationForm.get('userEmail') as FormControl;
  }

  get password(){
    return this.registerationForm.get('password') as FormControl;
  }

  get confirmPassword(){
    return this.registerationForm.get('confirmPassword') as FormControl;
  }

  get mobile(){
    return this.registerationForm.get('mobile') as FormControl;
  }

  onSubmit(){

    console.log(this.registerationForm.value);
    this.userSubmitted = true;
    if(this.registerationForm.valid){
    // this.user = Object.assign(this.user,this.registerationForm.value);
    this.authService.registerUser(this.userData()).subscribe(()=>{
      this.onReset();
      this.alertify.success("Congrats, You are successfully registered");
    },error => {
      this.alertify.error(error.error);
    });
  }
}

  onReset(){
    this.userSubmitted = false;
    this.registerationForm.reset();
  }
}

