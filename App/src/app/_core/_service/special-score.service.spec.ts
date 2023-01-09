/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SpecialScoreService } from './special-score.service';

describe('Service: SpecialScore', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SpecialScoreService]
    });
  });

  it('should ...', inject([SpecialScoreService], (service: SpecialScoreService) => {
    expect(service).toBeTruthy();
  }));
});
