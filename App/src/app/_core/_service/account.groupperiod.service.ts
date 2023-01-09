import { EnvService } from './env.service';
import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { AccountGroupPeriod } from '../_model/account.group.period';
@Injectable({
  providedIn: 'root'
})
export class AccountGroupPeriodService extends CURDService<AccountGroupPeriod> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService , env: EnvService)
  {
    super(http,"AccountGroupPeriod", utilitiesService ,env);
  }

}
