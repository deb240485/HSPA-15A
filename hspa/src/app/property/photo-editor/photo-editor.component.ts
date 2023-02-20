import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { IPhoto } from 'src/app/model/IPhoto';
import { Property } from 'src/app/model/Property';
import { HousingService } from 'src/app/services/housing.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() property!: Property;
  @Output() mainPhotoChangedEvent = new EventEmitter<string>();

  uploader!: FileUploader;
  baseUrl = environment.baseUrl;
  maxAllowedFileSize = 10*1024*1024;

  constructor(private housingService: HousingService) {

  }

  initializeFileUploader(){
    this.uploader  = new FileUploader({
        url: this.baseUrl + '/property/photo/' + this.property.id ,
        authToken: 'Bearer '+ localStorage.getItem('token'),
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: true,
        maxFileSize: this.maxAllowedFileSize
    });

    this.uploader.onAfterAddingFile = (file) => {
        file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.property.photos?.push(photo);
      }
    };
  }

  mainPhotoChanged(url: string){
    this.mainPhotoChangedEvent.emit(url);
  }

  ngOnInit(): void {
      this.initializeFileUploader();
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

  deletePhoto(propertyId: number, photo: IPhoto){
    this.housingService.deletePhoto(propertyId,photo.publicId).subscribe(()=>{
        this.property.photos = this.property.photos?.filter(
          p => p.publicId !== photo.publicId);
    });
  }

}
