import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Property } from 'src/app/model/Property';
import { HousingService } from 'src/app/services/housing.service';

@Injectable({
  providedIn: 'root'
})
export class PropertyDetailResolverService implements Resolve<Property> {

  constructor(private router: Router,private housingService: HousingService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
  Property | Observable<Property> | Promise<Property> {
    const propId = route.params['id'];
    return this.housingService.getProperty(+propId).pipe();

    // Error can be handled in this resolver and error message can be returned : https://stackoverflow.com/questions/43898934/how-to-handle-error-in-a-resolver;
  }
}
