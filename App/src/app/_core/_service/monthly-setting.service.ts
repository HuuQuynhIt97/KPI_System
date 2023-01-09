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
export class MonthlySettingService {

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
    return this.http.get(`${this.env.apiUrl}SettingMonth/getall`);
  }
  add(model) {
    return this.http.post(`${this.env.apiUrl}SettingMonth/add`, model);
  }
  update(model) {
    return this.http.put(`${this.env.apiUrl}SettingMonth/update`, model);
  }
  delete(id) {
    return this.http.delete(`${this.env.apiUrl}SettingMonth/delete/${id}`);
  }

}
