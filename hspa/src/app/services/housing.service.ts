import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Property } from '../model/Property';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IKeyValuePair } from '../model/IKeyValuePair';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

   baseUrl = environment.baseUrl;

  constructor(private http:HttpClient) { }

  getAllCities() : Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl +'/City');
  }

  getPropertyTypes(): Observable<IKeyValuePair[]>{
    return this.http.get<IKeyValuePair[]>(this.baseUrl+ '/PropertyType');
  }

  getFurnishingTypes(): Observable<IKeyValuePair[]>{
    return this.http.get<IKeyValuePair[]>(this.baseUrl+ '/FurnishingType');
  }

  getProperty(id: number) {
      return this.http.get<Property>(this.baseUrl+ '/property/detail/'+ id.toString());
    // return this.getAllProperties(1).pipe(
    //   map(propertiesArray => {
    //     // throw new Error('Some error');
    //     return propertiesArray.find(p => p.id === id) as Property;
    //   })
    // );
  }

  getAllProperties(SellRent?: number): Observable<Property[]>{
    return this.http.get<Property[]>(this.baseUrl + '/property/list/'+ SellRent?.toString());
  }

  addProperty(property: Property){
    return this.http.post(this.baseUrl + '/property', property);
  }

  newPropID(){
    if (localStorage.getItem('PID')){
      localStorage.setItem('PID', String(+localStorage.getItem('PID')! + 1));
      return +localStorage.getItem('PID')!;
    }else{
      localStorage.setItem('PID', '101');
      return 101;
    }
  }

  getPropertyAge(dateofEstablishment: Date): string {
    const today = new Date(); //08-02-2023 - [DD-MM-YYYY]
    const estDate = new Date(dateofEstablishment); // 01-01-2019 - [DD-MM-YYYY]
    let age = today.getFullYear() - estDate.getFullYear();
    const mnth = today.getMonth() - estDate.getMonth();

    //Current month smaller than establishment month or
    // Same month but current date smaller than establishment date

    if(mnth < 0 || (mnth === 0 && today.getDate() < estDate.getDate())){
      age--;
    }

    //Establishment date is future date
    if(today < estDate){
      return '0';
    }

    //Age is less than a year
    if(age === 0){
      return 'Less than a year';
    }

    return age.toString();
  }

}
