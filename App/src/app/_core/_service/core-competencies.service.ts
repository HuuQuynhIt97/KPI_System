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
export class CoreCompetenciesService {
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

  getAllCoreCompetencies(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetencies/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesScoreEquals2(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesScoreEquals2/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesScoreThan2(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesScoreThan2/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesScoreThan3(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesScoreThan3/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesAverage(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesAverage/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesPercentile(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesPercentile/${lang}/${campaignID}`);
  }

  getAllCoreCompetenciesAttitudeBehavior(lang, campaignID) {
    return this.http.get(`${this.env.apiUrl}CoreCompetencies/GetAllCoreCompetenciesAttitudeBehavior/${lang}/${campaignID}`);
  }

  exportExcelCoreComp(lang, campaignID) {
    return this.http.post(this.baseUrl + `CoreCompetencies/ExportExcelCoreCompetencies/${lang}/${campaignID}`,{}, { responseType: 'blob' });
  }
}
