/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AttitudeScoreService } from './attitude-score.service';

describe('Service: AttitudeScore', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AttitudeScoreService]
    });
  });

  it('should ...', inject([AttitudeScoreService], (service: AttitudeScoreService) => {
    expect(service).toBeTruthy();
  }));
});
