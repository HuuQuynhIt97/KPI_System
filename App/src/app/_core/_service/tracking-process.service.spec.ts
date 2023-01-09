/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TrackingProcessService } from './tracking-process.service';

describe('Service: TrackingProcess', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TrackingProcessService]
    });
  });

  it('should ...', inject([TrackingProcessService], (service: TrackingProcessService) => {
    expect(service).toBeTruthy();
  }));
});
