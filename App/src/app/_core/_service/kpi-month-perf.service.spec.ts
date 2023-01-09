/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { KpiMonthPerfService } from './kpi-month-perf.service';

describe('Service: KpiMonthPerf', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [KpiMonthPerfService]
    });
  });

  it('should ...', inject([KpiMonthPerfService], (service: KpiMonthPerfService) => {
    expect(service).toBeTruthy();
  }));
});
