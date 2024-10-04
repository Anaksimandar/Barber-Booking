import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditReservationModalComponent } from './edit-reservation-modal.component';

describe('EditReservationModalComponent', () => {
  let component: EditReservationModalComponent;
  let fixture: ComponentFixture<EditReservationModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditReservationModalComponent]
    });
    fixture = TestBed.createComponent(EditReservationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
