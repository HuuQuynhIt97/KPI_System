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
export class RoleService {
  baseUrl = environment.apiUrlEC;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient, public env: EnvService) { }
  getAll() {
    return this.http
      .get<IRole[]>(`${this.env.apiUrl}Role/GetAll`);
  }
  mappingUserRole(userRole: IUserRole) {
    return this.http
      .post(`${this.env.apiUrl}UserRole/MappingUserRole`, userRole);
  }
  mapUserRole(userID: number, roleID: number) {
    return this.http.put(`${this.env.apiUrl}Role/MapUserRole/${userID}/${roleID}`, {} );
  }
  lock(userRole: IUserRole) {
    return this.http.put(`${this.env.apiUrl}UserRole/Lock`, userRole);
  }
  isLock(userRole: IUserRole) {
    return this.http.put(`${this.env.apiUrl}UserRole/IsLock`, userRole);
  }
  getRoleByUserID(userid: number) {
    return this.http.get<any>(`${this.env.apiUrl}Role/GetRoleByUserID/${userid}`);
  }
  create(model) {
    return this.http.post(this.env.apiUrl + 'Role/Add', model);
  }
  update(model) {
    return this.http.put(this.env.apiUrl + 'Role/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.env.apiUrl + 'Role/Delete/' + id);
  }
  getScreenFunctionAndAction(roleIDs: any, lang) {
    return this.http.post(`${this.env.apiUrl}Permission/GetScreenFunctionAndAction`, {
      roleIDs,
      lang
    }).pipe(map((data: any[]) => {
      const data2 = data;
      for (const item of data2) {
        const checkedNodes = [];
        for (const subItem of item.fields.dataSource) {
          for (const child of subItem.childrens) {
            child.isChecked = child.status;
            if (child.status) {
              checkedNodes.push(child.id);
            }
          }
          const flag = subItem.childrens.every(v => v.status === true);
          if (flag) {
            checkedNodes.push(subItem.id);
            subItem.isChecked = flag;
          }
        }
      }
      return data2;
    }));
  }
}
