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
  public mainphotoUrl!: string;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];

  constructor(private route: ActivatedRoute, private router: Router , private housingService: HousingService) {

  }

  ngOnInit() {
    this.propertyId = +this.route.snapshot.params['id'];

    this.route.data.subscribe(
      (data) => {
        this.property = data['prp'];
        console.log(this.property.photos);
      }
    );


    this.property.age = this.housingService.getPropertyAge(this.property.estPossessionOn);

    this.galleryOptions = [
      {
        width: '100%',
        height: '465px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true
      }
    ];

    this.galleryImages = this.getPropertyPhotos();
  }

  changePrimaryPhoto(mainphotoUrl: string){
    this.mainphotoUrl = mainphotoUrl;
  }

  getPropertyPhotos(): NgxGalleryImage[] {
    const photoUrls: NgxGalleryImage[] = [];
    for (const photo of this.property.photos!) {
      if(photo.isPrimary){
        this.mainphotoUrl = photo.imageUrl;
      }else{
        photoUrls.push({
          small: photo.imageUrl,
          medium: photo.imageUrl,
          big: photo.imageUrl
        });
      }
    }
    return photoUrls;
  }
}


