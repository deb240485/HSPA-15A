import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions,NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { Property } from 'src/app/model/Property';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.css']
})
export class PropertyDetailComponent implements OnInit {

  public propertyId!: number;
  property = new Property();

  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];

  constructor(private route: ActivatedRoute, private router: Router , private housingService: HousingService) {

  }

  ngOnInit() {
    this.propertyId = +this.route.snapshot.params['id'];

    this.route.data.subscribe(
      (data) => {
        this.property = data['prp'];
      }
    );

    // this.route.params.subscribe(
    //   (params)=> {
    //     this.propertyId = +params['id'];
    //     this.housingService.getProperty(this.propertyId).subscribe(
    //       (data: Property) => {
    //         this.property = data;
    //       }, error => this.router.navigate(['/'])
    //     );
    //   }
    // );


    this.galleryOptions = [
      {
        width: '100%',
        height: '465px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true
      }
    ];

    this.galleryImages = [
      {
        small: 'assets/images/prop-1_internal-1.jpg',
        medium: 'assets/images/prop-1_internal-1.jpg',
        big: 'assets/images/prop-1_internal-1.jpg'
      },
      {
        small: 'assets/images/prop-1_internal-2.jpg',
        medium: 'assets/images/prop-1_internal-2.jpg',
        big: 'assets/images/prop-1_internal-2.jpg'
      },
      {
        small: 'assets/images/prop-1_internal-3.jpg',
        medium: 'assets/images/prop-1_internal-3.jpg',
        big: 'assets/images/prop-1_internal-3.jpg'
      },
      {
        small: 'assets/images/prop-1_internal-4.jpg',
        medium: 'assets/images/prop-1_internal-4.jpg',
        big: 'assets/images/prop-1_internal-4.jpg'
      },
      {
        small: 'assets/images/prop-1_internal-5.jpg',
        medium: 'assets/images/prop-1_internal-5.jpg',
        big: 'assets/images/prop-1_internal-5.jpg'
      }
    ];

  }
}
