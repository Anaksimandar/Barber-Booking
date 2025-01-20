import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NewUser } from '../../../models/new-user.model';
import { HttpClient } from '@angular/common/http';
import { passwordMatchValidator } from '../../validators/password-match-validator';
import { NotificationService } from '../../services/notification.service';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  signUpForm!: FormGroup;

  constructor(
    private notificationService: NotificationService,
    private http: HttpClient,
    private restService: RestService) { }

  ngOnInit() {
    this.initializeForm();
  }

  initializeForm() {
    this.signUpForm = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      surname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl("", [Validators.required, Validators.minLength(6)]),
    },
    { validators: passwordMatchValidator }
    )
  }

  private getValueFromFormByElementName(elementName:string):string {
    return this.signUpForm.get("elementName")?.value;

  }

  addUser(newUser: NewUser) {
    this.restService.post("sign-in", newUser).subscribe({
      next: (result) => {
        this.notificationService.showSuccess("You sign up successfully" + result);
      },
      error: (error) => {
        this.notificationService.showError(error);
      }
    })
  }

  onSubmit() {
    if (this.signUpForm.valid) {
      this.addUser(this.signUpForm.value);
      this.signUpForm.reset();
    }

  }
   
}
