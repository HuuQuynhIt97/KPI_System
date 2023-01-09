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
export class AttitudeScoreService {

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

  download(url: string){
    return this.http.get(url,{responseType: 'blob'})
  }
  getPoint(from,to) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getPoint/${from}/${to}`);
  }
  getAll(campaignID,userFrom,userTo, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getall/${campaignID}/${userFrom}/${userTo}/${type}`);
  }

  getAllScoreStation() {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/GetAllScoreStation`);
  }
  getKPISelfScoreDefault(campaignID,userID, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/GetKPISelfScoreDefault/${campaignID}/${userID}/${type}`);
  }
  getKPISelfScoreString(campaignID,userID, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/GetKPISelfScoreString/${campaignID}/${userID}/${type}`);
  }
  getDetail(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetail/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailPassion(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailPassion/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailAccountbility(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailAccountbility/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailAttention(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailAttention/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailContinuous(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailContinuous/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailEffective(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailEffective/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getDetailResilience(campaignID,userFl,userL0,userL1,userL2, type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/getDetailResilience/${campaignID}/${userFl}/${userL0}/${userL1}/${userL2}/${type}`);
  }

  getListCheckBehavior(campaignID, userFrom, userTo,type) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/GetListCheckBehavior/${campaignID}/${userFrom}/${userTo}/${type}`);
  }
  saveScore(model) {
    return this.http.post(`${this.env.apiUrl}AttitudeScore/SaveScore/`, model);
  }

  submitAttitude(model) {
    return this.http.post(`${this.env.apiUrl}AttitudeScore/SubmitAttitude/`, model);
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

  getAllByCampaign(campaignId) {
    return this.http.get(`${this.env.apiUrl}AttitudeScore/GetAllByCampaign/${campaignId}`);
  }
  addAttitudeScore(attitudeScore) {
    return this.http.post(`${this.env.apiUrl}AttitudeScore/Add`, attitudeScore);
  }
  updateAttitudeScore(attitudeScore) {
    return this.http.put(`${this.env.apiUrl}AttitudeScore/Update`, attitudeScore);
  }

  updateAttitudeSubmit(campaignId, submitTo, currentKPI, currentAtt) {
    return this.http.put(`${this.env.apiUrl}AttitudeScore/UpdateAttitudeSubmit/${campaignId}/${submitTo}/${currentKPI}/${currentAtt}`, {});
  }

  deleteAttitudeScore(id) { return this.http.delete(`${this.env.apiUrl}AttitudeScore/Delete/${id}`); }
}
