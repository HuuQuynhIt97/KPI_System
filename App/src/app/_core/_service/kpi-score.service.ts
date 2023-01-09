import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class KpiScoreService {

  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    private http: HttpClient,
    public env: EnvService
    ) { }

  getAll() {
    return this.http.get(`${this.env.apiUrl}KPIScore/getall`);
  }

  getKPIScoreDetail(campaignID,userID, type) {
    return this.http.get(`${this.env.apiUrl}KPIScore/GetKPIScoreDetail/${campaignID}/${userID}/${type}`);
  }
  
  getKPIScoreDetailL2L1(campaignID,userID, type) {
    return this.http.get(`${this.env.apiUrl}KPIScore/GetKPIScoreL2L1Detail/${campaignID}/${userID}/${type}`);
  }
  getKPIScoreDetailGM(campaignID,scoreFrom,scoreTo, type) {
    return this.http.get(`${this.env.apiUrl}KPIScore/GetKPIScoreGMDetail/${campaignID}/${scoreFrom}/${scoreTo}/${type}`);
  }
  getKPIScoreDetailL1L0(campaignID,scoreFrom,scoreTo, type) {
    return this.http.get(`${this.env.apiUrl}KPIScore/GetKPIScoreL1L0Detail/${campaignID}/${scoreFrom}/${scoreTo}/${type}`);
  }
  add(model) {
    return this.http.post(`${this.env.apiUrl}KPIScore/add`, model);
  }

  submitKPI(model) {
    return this.http.post(`${this.env.apiUrl}KPIScore/SubmitKPI`, model);
  }
  update(model) {
    return this.http.put(`${this.env.apiUrl}KPIScore/update`, model);
  }
  delete(id) {
    return this.http.delete(`${this.env.apiUrl}KPIScore/delete/${id}`);
  }

}
