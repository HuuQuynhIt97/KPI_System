/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SystemLanguageService } from './systemLanguage.service';

describe('Service: SystemLanguage', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SystemLanguageService]
    });
  });

  it('should ...', inject([SystemLanguageService], (service: SystemLanguageService) => {
    expect(service).toBeTruthy();
  }));
});
