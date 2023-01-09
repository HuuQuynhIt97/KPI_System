import { ObjectiveRequest } from './../_model/objective';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Objective } from '../_model/objective';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class ObjectiveService extends CURDService<Objective> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"Objective", utilitiesService , env);
  }
  post(model: ObjectiveRequest): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.env.apiUrl}${this.entity}/add`, model)
      .pipe(catchError(this.handleError));
  }
  put(model: ObjectiveRequest): Observable<OperationResult> {
    return this.http
      .put<OperationResult>(`${this.env.apiUrl}${this.entity}/update`, model)
      .pipe(catchError(this.handleError));
  }
  getAllKPIObjectiveByAccountId(): Observable<Objective[]> {
    return this.http
      .get<Objective[]>(`${this.env.apiUrl}${this.entity}/GetAllKPIObjectiveByAccountId`, {})
      .pipe(catchError(this.handleError));
  }

}
