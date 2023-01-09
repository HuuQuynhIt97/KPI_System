/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { KpiScoreService } from './kpi-score.service';

describe('Service: KpiScore', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [KpiScoreService]
    });
  });

  it('should ...', inject([KpiScoreService], (service: KpiScoreService) => {
    expect(service).toBeTruthy();
  }));
});
