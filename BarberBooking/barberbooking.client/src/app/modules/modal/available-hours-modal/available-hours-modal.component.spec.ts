import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableHoursModalComponent } from './available-hours-modal.component';

describe('AvailableHoursModalComponent', () => {
  let component: AvailableHoursModalComponent;
  let fixture: ComponentFixture<AvailableHoursModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AvailableHoursModalComponent]
    });
    fixture = TestBed.createComponent(AvailableHoursModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
