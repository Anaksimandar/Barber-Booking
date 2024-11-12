import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent {
  isAuthenticated: boolean = false;
  constructor(private accountService: AccountService, private router:Router) {
    this.isAuthenticated = accountService.isAuthenticated();
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }

}
