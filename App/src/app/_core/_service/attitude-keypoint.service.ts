import { EnvService } from './env.service';
import { Injectable } from '@angular/core'
import { BehaviorSubject, Observable } from 'rxjs'
import { map } from 'rxjs/operators'
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'

import { environment } from '../../../environments/environment'

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class AttitudeKeypointService {
  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }

  getAllByAttitudeScore(attitudeHeadingID: any, campaignID: any) {
    return this.http.get<any[]>(`${this.env.apiUrl}AttitudeKeypoint/GetAllByAttitudeScore/${attitudeHeadingID}/${campaignID}`);
  }

  addAttitudeKeypoint(ttitudeKeypoint) {
    return this.http.post(`${this.env.apiUrl}AttitudeKeypoint/Add`, ttitudeKeypoint);
  }
}
