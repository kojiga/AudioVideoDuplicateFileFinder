import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class UploadServiceService {

  private baseUrl = 'https://localhost:7211/api/values/';

  constructor(private http: HttpClient) { }

  getDuplicateFiles(directoryPath: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}duplicates?directoryPath=${directoryPath}`);
  }

  deleteDuplicateFiles(directoryPath: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}deleteduplicates?directoryPath=${directoryPath}`);
  }
}
