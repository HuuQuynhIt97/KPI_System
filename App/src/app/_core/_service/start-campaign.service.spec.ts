/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StartCampaignService } from './start-campaign.service';

describe('Service: StartCampaign', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StartCampaignService]
    });
  });

  it('should ...', inject([StartCampaignService], (service: StartCampaignService) => {
    expect(service).toBeTruthy();
  }));
});
