import { EnvService } from './env.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PaginatedResult } from '../_model/pagination';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class BuildingUserService {
  baseUrl = environment.apiUrlEC;
  authUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }
  getBuildingsAsTreeView() {
    return this.http.get(`${this.env.apiUrl}Building/GetAllAsTreeView`);
  }
  mappingUserWithBuilding(obj) {
    return this.http.post(`${this.env.apiUrl}BuildingUser/MappingUserWithBuilding`, obj);
  }
  removeBuildingUser(obj) {
    return this.http.post(`${this.env.apiUrl}BuildingUser/RemoveBuildingUser`, obj);
  }
  getAllUsers(page?, pageSize? ): Observable<PaginatedResult<object[]>> {
    const paginatedResult: PaginatedResult<object[]> = new PaginatedResult<
    object[]
    >();
    return this.http
      .get<object[]>(`${this.env.apiUrl}Users/GetAllUsers/${environment.systemCode}/${page}/${pageSize}`, {
        observe: 'response'
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
              );
            }
          return paginatedResult;
        })
      );
  }
  deleteUser(id) { return this.http.delete(`${this.env.apiUrl}Users/Delete/${id}`); }
  updateUser(update) { return this.http.post(`${this.env.apiUrl}Users/Update`, update); }
  createUser(create) { return this.http.post(`${this.env.apiUrl}Users/Create`, create); }
  getAllBuildingUsers() {
    return this.http.get(`${this.env.apiUrl}BuildingUser/getAllBuildingUsers`);
    }
  getBuildingUserByBuildingID(buildingID) {
    return this.http.get(`${this.env.apiUrl}BuildingUser/GetBuildingUserByBuildingID/${buildingID}`);
    }
}
