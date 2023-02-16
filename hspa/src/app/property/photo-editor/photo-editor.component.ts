import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IPhoto } from 'src/app/model/IPhoto';
import { Property } from 'src/app/model/Property';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() property!: Property;
  @Output() mainPhotoChangedEvent = new EventEmitter<string>();

  constructor(private housingService: HousingService) {

  }

  mainPhotoChanged(url: string){
    this.mainPhotoChangedEvent.emit(url);
  }

  ngOnInit(): void {

  }

  setPrimaryPhoto(propertyId: number, photo: IPhoto){
      this.housingService.setPrimaryPhoto(propertyId,photo.publicId).subscribe(
        () => {
            this.mainPhotoChanged(photo.imageUrl);
            this.property.photos!.forEach(p => {
              if(p.isPrimary){
                p.isPrimary = false;
              }
              if(p.publicId === photo.publicId){
                p.isPrimary = true;
              }
            });
        }
      );
  }

}
