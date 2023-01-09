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
export class JobTitleService {
  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }

  getAll() {
    return this.http.get(`${this.env.apiUrl}JobTitle/GetAll`);
  }

  getAllByLang(lang) {
    return this.http.get<any[]>(`${this.env.apiUrl}JobTitle/GetAllByLang/${lang}`);
  }

  addJobTitle(jobTitle) {
    return this.http.post(`${this.env.apiUrl}JobTitle/Add`, jobTitle);
  }
  updateJobTitle(jobTitle) {
    return this.http.put(`${this.env.apiUrl}JobTitle/Update`, jobTitle);
  }
  delete(id) { return this.http.delete(`${this.env.apiUrl}JobTitle/Delete/${id}`); }
}
