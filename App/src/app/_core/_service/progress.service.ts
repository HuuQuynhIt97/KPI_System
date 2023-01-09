import { CURDService } from './CURD.service';
import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Progress } from '../_model/progress';
import { UtilitiesService } from './utilities.service';
import { EnvService } from './env.service';
@Injectable({
  providedIn: 'root'
})
export class ProgressService extends CURDService<Progress> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService, env: EnvService)
  {
    super(http,"Progress", utilitiesService , env);
  }

}
