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
export class PeopleCommitteeService {
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
  getAll(lang, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetAll/${lang}/${campaignID}`);
  }
  getAllHQReport(lang, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}HQReport/GetAllHQReport/${lang}/${campaignID}`);
  }
  getKpi(appraiseeID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetKpi/${appraiseeID}`);
  }
  getPeopleCommittee(appraiseeID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetPeopleCommittee/${appraiseeID}`);
  }
  getKpiScore(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetAllKpiScore/${appraiseeID}/${campaignID}`);
  }
  getAttitudeScore(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetAllAttitudeScore/${appraiseeID}/${campaignID}`);
  }
  getSumAttitudeScore(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetSumAttitudeScore/${appraiseeID}/${campaignID}`);
  }
  getSumNewAttitudeScore(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetSumNewAttitudeScore/${appraiseeID}/${campaignID}`);
  }
  getDetailNewAttitudeEvaluation(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetDetailNewAttitudeEvaluation/${appraiseeID}/${campaignID}`);
  }
  getSpecialScoreDetail(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetSpecialScoreDetail/${appraiseeID}/${campaignID}`);
  }
  getScoreL2(appraiseeID, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetScoreL2/${appraiseeID}/${campaignID}`);
  }
  getCommitteeScore(appraiseeID, userLogin, campaignID): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetCommitteeScore/${appraiseeID}/${userLogin}/${campaignID}`);
  }
  getFrozen(campaignID): Observable<boolean> {
    return this.http.get<boolean>(`${this.env.apiUrl}PeopleCommittee/GetFrozen/${campaignID}`);
  }
  updateKpiScore(kpiScore) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateKpiScore`, kpiScore);
  }
  updateAttitudeScore(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateAttitudeScore`, model);
  }
  updateSpecialScore(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateSpecialScore`, model);
  }
  updateSpecialContribution(model) {
    return this.http.post(`${this.env.apiUrl}PeopleCommittee/UpdateSpecialContribution`, model);
  }
  updateKpiScoreL2(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateKpiScoreL2`, model);
  }
  updateCommitteeScore(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateCommitteeScore`, model);
  }
  updateCommitteeSequence(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateCommitteeSequence`, model);
  }
  updateNewAttitudeEvaluation(model) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/UpdateNewAttitudeEvaluation`, model);
  }
  lockUpdate(campaignID) {
    return this.http.put(`${this.env.apiUrl}PeopleCommittee/LockUpdate/${campaignID}`, {});
  }
  getKPIDefaultPerson(campaignID,userID) {
    return this.http.get(`${this.env.apiUrl}PeopleCommittee/getKPIDefaultPerson/${campaignID}/${userID}`);
  }
  getKPIDefaultMuti(campaignID,userID) {
    return this.http.get(`${this.env.apiUrl}PeopleCommittee/getKPIDefaultMuti/${campaignID}/${userID}`);
  }
  getKPIStringPerson(campaignID,userID) {
    return this.http.get(`${this.env.apiUrl}PeopleCommittee/getKPIStringPerson/${campaignID}/${userID}`);
  }
  getKPIStringMuti(campaignID,userID) {
    return this.http.get(`${this.env.apiUrl}PeopleCommittee/getKPIStringMuti/${campaignID}/${userID}`);
  }
}
