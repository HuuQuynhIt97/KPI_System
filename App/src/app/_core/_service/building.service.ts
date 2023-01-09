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
export class BuildingService {
  baseUrl = environment.apiUrlEC;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient , public env: EnvService) { }
  delete(id) { return this.http.delete(`${this.env.apiUrlEC}Building/Delete/${id}`); }
  rename(edit) { return this.http.put(`${this.env.apiUrlEC}Building/Update`, edit); }

  getBuildingsAsTreeView() {
    return this.http.get<Array<HierarchyNode<IBuilding>>>(`${this.env.apiUrlEC}Building/GetAllAsTreeView`);
  }
  getBuildings() {
    return this.http.get<Array<IBuilding>>(`${this.env.apiUrlEC}Building/GetBuildings`);
  }
  getAllBuildingType() {
    return this.http.get(`${this.env.apiUrlEC}Building/getAllBuildingType`);
  }
  getBuildingsForSetting() {
    return this.http.get(`${this.env.apiUrlEC}Building/GetBuildingsForSetting`);
  }
  addOrUpdateLunchTime(item: any) {
    return this.http.post(`${this.env.apiUrlEC}Building/AddOrUpdateLunchTime`, item);
  }
  createMainBuilding(Building) { return this.http.post(`${this.env.apiUrlEC}Building/CreateMainBuilding`, Building); }
  createSubBuilding(Building) { return this.http.post(`${this.env.apiUrlEC}Building/CreateSubBuilding`, Building); }
}
