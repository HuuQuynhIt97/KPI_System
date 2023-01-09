import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { IRole, IUserRole } from '../_model/role';

@Injectable({
  providedIn: 'root'
})
export class TrackingProcessService {

  baseUrl = environment.apiUrlEC;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }
  trackingProcess(currentTime, userId) {
    return this.http.get(`${this.env.apiUrl}Report/TrackingProcess/${currentTime}/${userId}`);
  }

  trackingAppraisalProgress(userId,campaignID) {
    return this.http.get(`${this.env.apiUrl}Report/TrackingAppaisalProgress/${userId}/${campaignID}`);
  }

  trackingProcessKPI(currentTime, userId) {
    return this.http.get(`${this.env.apiUrl}Report/TrackingProcessKPI/${currentTime}/${userId}`);
  }
 

}
