/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PeopleCommitteeService } from './people-committee.service';

describe('Service: PeopleCommittee', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PeopleCommitteeService]
    });
  });

  it('should ...', inject([PeopleCommitteeService], (service: PeopleCommitteeService) => {
    expect(service).toBeTruthy();
  }));
});
