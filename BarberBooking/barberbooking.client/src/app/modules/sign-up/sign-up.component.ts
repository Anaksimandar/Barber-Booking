import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NewUser } from '../../../models/new-user.model';
import { passwordMatchValidator } from '../../validators/password-match-validator';
import { NotificationService } from '../../services/notification.service';
import { RestService } from '../rest/rest-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  signUpForm!: FormGroup;

  constructor(
    private notificationService: NotificationService,
    private restService: RestService,
    private router:Router)
    { }

  ngOnInit() {
    this.initializeForm();
  }

  initializeForm() {
    this.signUpForm = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      surname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email]),
      number: new FormControl("", [Validators.required]),
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
        this.router.navigateByUrl("/login")
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
