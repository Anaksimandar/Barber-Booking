import { TestBed } from '@angular/core/testing';

import { DateEmiterService } from './date-emiter.service';

describe('DateEmiterService', () => {
  let service: DateEmiterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DateEmiterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
