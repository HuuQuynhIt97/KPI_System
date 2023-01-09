import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_model/pagination';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { FilterRequest } from '../_model/filterRequest';
import { UtilitiesService } from './utilities.service';

@Injectable({
  providedIn: 'root'
})
export class MeetingService {

  baseUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(
    private http: HttpClient,
    private utilitiesService: UtilitiesService,
    public env: EnvService
    ) { }

  downLoadFile(file_name) {
    return this.http.get(`${this.env.apiUrl}UploadFile/DownloadFileMeeting/${file_name}`,{responseType: 'blob'});
  }
  downloadFileMeeting(url: string){
    return this.http.get(url,{
      responseType: 'blob',
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
      })
    })
  }

  download(url: string, file: any){
    return this.http.post(url,file,{
      responseType: 'blob',
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
      })
    })
  }

  checkFile(url: string, file: any){
    return this.http.post<boolean>(url,file)
  }

  getOCs() {
    return this.http.get(`${this.env.apiUrl}Ocs/GetListTree`);
  }
  getAllKpi(userId) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetAllKPI/${userId}`);
  }
  getAllKpiCHM(userId) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetAllKPICHM/${userId}`);
  }
  getAllLevel() {
    return this.http.get(`${this.env.apiUrl}Meeting/getAllLevel/`);
  }
  getAllKpiWithFilterQuery(request: FilterRequest) {
    const query = this.utilitiesService.serialize(request);
    return this.http.get<any[]>(`${this.env.apiUrl}Meeting/GetAllKPIWithFilterQuery?${query}`);
  }
  getChart(kpiId) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetChart/${kpiId}`);
  }
  getChartWithTime(kpiId, dateTime) {
    return this.http.get(`${this.env.apiUrl}Meeting/GetChartWithDateTime/${kpiId}/${dateTime}`);
    // return this.http.get<any>(`${this.env.apiUrl}Meeting/GetChartWithDateTime?kpiId=${kpiId}?currentTime=${dateTime}`, {});
  }
  addPolicy(model) {
    return this.http.post(`${this.env.apiUrl}Ocpolicy/MappingPolicyOc`, model);
  }
  updatePolicy(model) {
    return this.http.post(`${this.env.apiUrl}Ocpolicy/RemovePolicyOC`, model);
  }
  deletePolicy(id) {
    return this.http.delete(`${this.env.apiUrl}Ocpolicy/DeletePolicy/${id}`);
  }
  getAllPolicy(){
    return this.http.get(`${this.env.apiUrl}Ocpolicy/GetAllPolicy`);
  }

}
