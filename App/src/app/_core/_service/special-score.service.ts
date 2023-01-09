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
export class SpecialScoreService {

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
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/getall`);
  }

  getSpecialType(lang) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/GetSpecialType/${lang}`);
  }

  getSpecialCompact(lang) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/GetSpecialCompact/${lang}`);
  }

  getSpecialRatio() {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/getSpecialRatio`);
  }

  getSpecialScore() {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/getSpecialScore`);
  }


  getSpecialScoreDetail(campaignID,scoreFrom,scoreTo,scoreType) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/getSpecialScoreDetail/${campaignID}/${scoreFrom}/${scoreTo}/${scoreType}`);
  }

  getMultiType(campaignID,scoreTo,scoreType) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/GetMultiType/${campaignID}/${scoreTo}/${scoreType}`);
  }

  getMultiImpact(campaignID,scoreTo,scoreType) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/GetMultiImpact/${campaignID}/${scoreTo}/${scoreType}`);
  }
  getSpecialL1ScoreDetail(campaignID,userID, type) {
    return this.http.get(`${this.env.apiUrl}SpecialContributionScore/getSpecialL1ScoreDetail/${campaignID}/${userID}/${type}`);
  }
  add(model) {
    return this.http.post(`${this.env.apiUrl}SpecialContributionScore/add`, model);
  }
  update(model) {
    return this.http.put(`${this.env.apiUrl}SpecialContributionScore/update`, model);
  }
  delete(id) {
    return this.http.delete(`${this.env.apiUrl}SpecialContributionScore/delete/${id}`);
  }

}
