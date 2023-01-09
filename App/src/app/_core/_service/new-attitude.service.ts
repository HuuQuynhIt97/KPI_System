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
export class NewAttitudeService {
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

  // getAll(lang, campaignID): Observable<any[]> {
  //   return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetAll/${lang}/${campaignID}`);
  // }

  getAll(campaignID: any, scoreTo: any, scoreFrom: any) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetAll/${campaignID}/${scoreTo}/${scoreFrom}`);
  }

  updatePoint(id: any, point: string) {
    return this.http.put(`${this.env.apiUrl}NewAttitudeScore/UpdatePoint/${id}/${point}`, {});
  }

  updateComment(attAttchment) {
    return this.http.put(`${this.env.apiUrl}NewAttitudeScore/UpdateComment`, attAttchment);
  }

  getAttEvaluation(campaignID: any, scoreTo: any, scoreFrom: any) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/getAttEvaluation/${campaignID}/${scoreTo}/${scoreFrom}`);
  }

  updateAttEvaluation(attEvalution) {
    return this.http.put(`${this.env.apiUrl}NewAttitudeScore/UpdateAttEvaluation`, attEvalution);
  }

  getNewAttitudeSubmit(campaignID: any, scoreTo: any) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetNewAttitudeSubmit/${campaignID}/${scoreTo}`);
  }

  checkSubmitNewAtt(campaignID, scoreTo, scoreFrom, type) {
    return this.http.put(`${this.env.apiUrl}NewAttitudeScore/CheckSubmitNewAtt/${campaignID}/${scoreTo}/${scoreFrom}/${type}`, {});
  }

  generateNewAttitudeScore(campaignID, scoreTo, scoreFrom) {
    return this.http.put(`${this.env.apiUrl}NewAttitudeScore/GenerateNewAttitudeScore/${campaignID}/${scoreTo}/${scoreFrom}`, {});
  }

  getSelfAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetSelfAppraisal/${userID}`);
  }

  getFirstLevelAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetFirstLevelAppraisal/${userID}`);
  }

  getSecondLevelAppraisal(userID) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetSecondLevelAppraisal/${userID}`);
  }

  getFLFeedback(userID) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetFLFeedback/${userID}`);
  }

  getDetailNewAttitude(campaignID, scoreTo) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetDetailNewAttitude/${campaignID}/${scoreTo}`);
  }

  getDetailNewAttitudeEvaluation(campaignID, scoreTo) {
    return this.http.get(`${this.env.apiUrl}NewAttitudeScore/GetDetailNewAttitudeEvaluation/${campaignID}/${scoreTo}`);
  }


}
