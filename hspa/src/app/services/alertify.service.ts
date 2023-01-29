import { Injectable } from '@angular/core';
import * as awsomealert from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  success(message:string){
    awsomealert.success(message);
  }

  warning(message:string){
    awsomealert.warning(message);
  }

  error(message:string){
    awsomealert.error(message);
  }
}
