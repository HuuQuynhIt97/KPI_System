import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { PeriodReportTime } from '../_model/period.report.time';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class PeriodReportTimeService extends CURDService<PeriodReportTime> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"PeriodReportTime", utilitiesService , env);
  }

}
