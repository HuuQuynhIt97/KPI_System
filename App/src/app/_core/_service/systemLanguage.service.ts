import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { IRole, IUserRole } from '../_model/role';
import { PaginatedResult } from '../_model/pagination';

@Injectable({
  providedIn: 'root'
})
export class SystemLanguageService {

  baseUrl = environment.apiUrlEC;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }
  getAll() {
    return this.http.get<any>(`${this.env.apiUrl}SystemLanguage/GetAll`, {});
  }
  create(model) {
    return this.http.post(this.env.apiUrl + 'SystemLanguage/create', model);
  }
  update(model) {
    return this.http.put(this.env.apiUrl + 'SystemLanguage/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.env.apiUrl + 'SystemLanguage/Delete/' + id);
  }
  getLanguages(lang) {
    return this.http.get<any>(`${this.env.apiUrl}SystemLanguage/GetLanguages/${lang}`, {});
  }

}
