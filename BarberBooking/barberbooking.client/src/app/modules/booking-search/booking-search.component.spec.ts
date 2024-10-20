import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingSearchComponent } from './booking-search.component';

describe('BookingSearchComponent', () => {
  let component: BookingSearchComponent;
  let fixture: ComponentFixture<BookingSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BookingSearchComponent]
    });
    fixture = TestBed.createComponent(BookingSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
