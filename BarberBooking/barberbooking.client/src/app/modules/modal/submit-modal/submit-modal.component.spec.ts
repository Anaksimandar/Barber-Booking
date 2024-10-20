import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitModalComponent } from './submit-modal.component';

describe('SubmitModalComponent', () => {
  let component: SubmitModalComponent;
  let fixture: ComponentFixture<SubmitModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SubmitModalComponent]
    });
    fixture = TestBed.createComponent(SubmitModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
