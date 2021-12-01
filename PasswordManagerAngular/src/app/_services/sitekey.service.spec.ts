import { TestBed } from '@angular/core/testing';

import { SitekeyService } from './sitekey.service';

describe('SitekeyService', () => {
  let service: SitekeyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SitekeyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
