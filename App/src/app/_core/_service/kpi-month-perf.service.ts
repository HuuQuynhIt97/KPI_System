import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { FilterRequest } from '../_model/filterRequest';
import { UtilitiesService } from './utilities.service';


@Injectable({
  providedIn: 'root'
})
export class KpiMonthPerfService {

  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    private http: HttpClient,
    private utilitiesService: UtilitiesService,
    public env: EnvService
    ) { }

  downLoadFile(file_name) {
    return this.http.get(`${this.env.apiUrl}UploadFile/DownloadFileMeeting/${file_name}`,{responseType: 'blob'});
  }
  downloadFileMeeting(url: string){
    return this.http.get(url,{
      responseType: 'blob',
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
      })
    })
  }

  download(url: string, file: any){
    return this.http.post(url,file,{
      responseType: 'blob',
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
      })
    })
  }
  getAll(userId,dateTime) {
    return this.http.get(`${this.env.apiUrl}KPIMonthPerf/GetAllKPI/${userId}/${dateTime}`);
  }
 


}
