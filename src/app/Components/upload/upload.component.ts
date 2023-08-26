import { Component } from '@angular/core';
import { UploadServiceService } from 'src/app/Services/upload-service.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {
  directoryPath = ''; // Add the desired directory path
  duplicateFiles: any[] = [];

  constructor(private apiService: UploadServiceService) { }

  getDuplicates() {
    this.apiService.getDuplicateFiles(this.directoryPath).subscribe(
      data => {
        this.duplicateFiles = data;
      },
      error => {
        console.error(error);
      }
    );
  }

  deleteDuplicates() {
    this.apiService.deleteDuplicateFiles(this.directoryPath).subscribe(
      () => {
        alert('Duplicate files deleted.');
      },
      error => {
        console.error(error);
      }
    );
  }
}
