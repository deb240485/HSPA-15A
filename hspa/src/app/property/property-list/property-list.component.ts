import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HousingService } from 'src/app/services/housing.service';
import { IPropertyBase } from '../../model/IPropertyBase';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css']
})

export class PropertyListComponent implements OnInit {
  SellRent = 1;
  properties!: Array<IPropertyBase>;
  todayProp = new Date();
  City = '';
  searchCity:any;
  SortbyParam = '';
  SortDirection = 'asc';

  constructor(private route:ActivatedRoute,private housingService: HousingService) {

  }

  ngOnInit(): void {

    if(this.route.snapshot.url.toString()){
      this.SellRent = 2; // Means we are on rent-property URL else we are on base URL.
    }

    this.housingService.getAllProperties(this.SellRent).subscribe(
      data => {
        this.properties = data;
        console.log(data);
      },error =>{
        console.log('httperror:');
        console.log(error);
      }
    );
  }

  onCityFilter(){
    this.searchCity = this.City;
  }

  onCityFilterClear() {
    this.searchCity = '';
    this.City = '';
  }

  onSortDirection() {
    if (this.SortDirection === 'desc') {
      this.SortDirection = 'asc';
    } else {
      this.SortDirection = 'desc';
    }
  }
}
