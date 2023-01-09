import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { HttpClient } from '@angular/common/http'
import { catchError } from 'rxjs/operators'

import { CURDService } from './CURD.service'
import { Account } from '../_model/account'
import { UtilitiesService } from './utilities.service'
import { OperationResult } from '../_model/operation.result'
import { EnvService } from './env.service'

@Injectable({
  providedIn: 'root'
})
export class Account2Service extends CURDService<Account> {

  constructor(
      http: HttpClient,
      utilitiesService: UtilitiesService,
      env: EnvService
    )
  {
    super(http,"Account", utilitiesService,env);
  }
  lock(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/lock?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  updateL0(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/updateL0?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  updateGhr(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/updateGhr?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  updateGm(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/updateGm?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  updateGmScore(id): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/updateGmScore?id=${id}`, {}).pipe(
      catchError(this.handleError)
    );
  }
  getAccounts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}Account/GetAccounts`);
  }
  getAllByLang(lang): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}Account/GetAllByLang/${lang}`);
  }
  getAllByL0(lang): Observable<any[]> {
    return this.http.get<any[]>(`${this.env.apiUrl}PeopleCommittee/GetAll/${lang}`);
  }
  changePassword(request): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.env.apiUrl}Account/changePassword`, request).pipe(
      catchError(this.handleError)
    );
  }
}
