import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmForgotPasswordComponent } from './reset-password.component';

describe('ConfirmForgotPasswordComponent', () => {
  let component: ConfirmForgotPasswordComponent;
  let fixture: ComponentFixture<ConfirmForgotPasswordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConfirmForgotPasswordComponent]
    });
    fixture = TestBed.createComponent(ConfirmForgotPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
