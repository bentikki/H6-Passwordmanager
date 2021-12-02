import { TestBed } from '@angular/core/testing';

import { GeneratepasswordService } from './generatepassword.service';

describe('GeneratepasswordService', () => {
  let service: GeneratepasswordService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GeneratepasswordService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
