import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';
import { ChangePasswordModel } from '../../../models/change-password.model';
import { AuthenticatedUser } from '../../../models/authenticated-user.model';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  formGroup!: FormGroup;
  private currentUser: AuthenticatedUser | null;
  constructor (private http: HttpClient, private notification: NotificationService, private accountService:AccountService) {
    this.currentUser = null;
  }

  ngOnInit(): void {
    this.initializeForm();
    this.currentUser = this.accountService.getCurrentUserFromStorage();
  }

  changePassword() {
    const httpOptions: object = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    const changePasswordModel: ChangePasswordModel = {
      mail: this.currentUser?.email,
      oldPassword: this.formGroup.get("oldPassword")?.value,
      newPassword: this.formGroup.get("newPassword")?.value
    };
    this.http.post("http://localhost:5137/api/change-password", JSON.stringify(changePasswordModel), httpOptions).subscribe(
      result => {
        this.notification.showSuccess("Password has been changed succesfully");
      },
      error => {
        this.notification.showError(error.error);
      }
    )
    this.formGroup.reset();
  }

  initializeForm() {
    this.formGroup = new FormGroup({
      oldPassword: new FormControl("", [Validators.required, Validators.minLength(6)]),
      newPassword: new FormControl("", [Validators.required, Validators.minLength(6)])
    })
  }
}
