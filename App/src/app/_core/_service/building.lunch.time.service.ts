import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { HierarchyNode, IBuilding } from '../_model/building';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class BuildingLunchTimeService {
  baseUrl = environment.apiUrlEC;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }
  getBuildings() {
    return this.http.get<Array<IBuilding>>(`${this.env.apiUrl}BuildingLunchTime/GetAllBuildings`);
  }
  getPeriodMixingByBuildingID(buildingID: number) {
    return this.http.get(`${this.env.apiUrl}BuildingLunchTime/getPeriodMixingByBuildingID/${buildingID}`);
  }
  addOrUpdateLunchTime(item: any) {
    return this.http.post(`${this.env.apiUrl}BuildingLunchTime/AddOrUpdateLunchTime`, item);
  }
  updatePeriodMixing(item: any) {
    return this.http.put(`${this.env.apiUrl}BuildingLunchTime/updatePeriodMixing`, item);
  }
  addPeriodMixing(item: any) {
    return this.http.post(`${this.env.apiUrl}BuildingLunchTime/addPeriodMixing`, item);
  }
  deletePeriodMixing(id) {
    return this.http.delete(`${this.env.apiUrl}BuildingLunchTime/deletePeriodMixing/${id}`);
  }

  addLunchTimeBuilding(item: any) {
    return this.http.put(`${this.env.apiUrl}BuildingLunchTime/AddLunchTimeBuilding`, item);
  }
  updatePeriodDispatch(item: any) {
    return this.http.put(`${this.env.apiUrl}BuildingLunchTime/updatePeriodDispatch`, item);
  }
  addPeriodDispatch(item: any) {
    return this.http.post(`${this.env.apiUrl}BuildingLunchTime/addPeriodDispatch`, item);
  }
  deletePeriodDispatch(id) {
    return this.http.delete(`${this.env.apiUrl}BuildingLunchTime/deletePeriodDispatch/${id}`);
  }
  getPeriodDispatchByPeriodMixingID(periodMixingID: number) {
    return this.http.get(`${this.env.apiUrl}BuildingLunchTime/GetPeriodDispatchByPeriodMixingID/${periodMixingID}`);
  }
}
