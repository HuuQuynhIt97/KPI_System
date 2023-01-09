import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';
import { PeriodType } from 'src/app/_core/_model/period-type';
import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class PeriodTypeService extends CURDService<PeriodType> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"PeriodType", utilitiesService, env);
  }
}
