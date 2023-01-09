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
export class KpinewService {

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

  getKPIByOcID(ocID) {
    return this.http.get(`${this.env.apiUrl}KPINew/GetKPIByOcID/${ocID}`, {});
  }
  getTree(lang){
    return this.http.get(`${this.env.apiUrl}KPINew/GetAllAsTreeView/${lang}`, {});
  }
  getTree2nd3rd(lang, userId){
    return this.http.get(`${this.env.apiUrl}KPINew/GetAllAsTreeView2nd3rd/${lang}/${userId}`, {});
  }
  getPolicyByOcID(ocID) {
    return this.http.get(`${this.env.apiUrl}KPINew/GetPolicyByOcID/${ocID}`, {});
  }
  IsDisPlayTodoUpdate(ID) {
    return this.http.post(`${this.baseUrl}KPINew/IsDisPlayTodoUpdate/${ID}`, {});
  }
  getAllType(lang) {
    return this.http.get(`${this.env.apiUrl}KPINew/getAllType/${lang}`, {});
  }
  getListPic() {
    return this.http.get(`${this.env.apiUrl}KPINew/GetListPic`, {});
  }
  add(model) {
    return this.http.post(`${this.env.apiUrl}KPINew/Add`, model);
  }
  update(model) {
    return this.http.put(`${this.env.apiUrl}KPINew/update`, model);
  }
  updateSequence(model) {
    return this.http.put(`${this.env.apiUrl}KPINew/updateSequence`, model);
  }
  delete(id) {
    return this.http.delete(`${this.env.apiUrl}KPINew/delete/${id}`);
  }
  exportExcelKpiNew(lang) {
    return this.http.post(this.baseUrl + `KPINew/ExportExcelKpiNew`, lang , { responseType: 'blob' });
  }
}
