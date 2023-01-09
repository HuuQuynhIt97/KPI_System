/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MonthlySettingService } from './monthly-setting.service';

describe('Service: MonthlySetting', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MonthlySettingService]
    });
  });

  it('should ...', inject([MonthlySettingService], (service: MonthlySettingService) => {
    expect(service).toBeTruthy();
  }));
});
