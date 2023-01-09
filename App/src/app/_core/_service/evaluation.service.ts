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
export class EvaluationService {

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
    return this.http.get(`${this.env.apiUrl}Campaign/getall`);
  }

  getSelfAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}Evaluation/getSelfAppraisal/${userID}`);
  }

  getFirstLevelAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}Evaluation/getFirstLevelAppraisal/${userID}`);
  }

  getSecondLevelAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}Evaluation/getSecondLevelAppraisal/${userID}`);
  }

  getFLFeedback(userID) {
    return this.http.get(`${this.env.apiUrl}Evaluation/getFLFeedback/${userID}`);
  }

  getGM(userID) {
    return this.http.get(`${this.env.apiUrl}Evaluation/getGM/${userID}`);
  }

  add(model) {
    return this.http.post(`${this.env.apiUrl}Campaign/add`, model);
  }
  update(model) {
    return this.http.put(`${this.env.apiUrl}Campaign/update`, model);
  }
  delete(id) {
    return this.http.delete(`${this.env.apiUrl}Campaign/delete/${id}`);
  }

  generateEvaluation(campaignID) {
    return this.http.post(`${this.env.apiUrl}Campaign/GenerateEvaluation/${campaignID}`, {});
  }

  generateAttitudeSubmit(campaignID) {
    return this.http.post(`${this.env.apiUrl}Evaluation/GenerateAttitudeSubmit/${campaignID}`, {});
  }

}
