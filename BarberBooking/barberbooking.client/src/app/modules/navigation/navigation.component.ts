import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent {
  constructor(public accountService: AccountService, private router:Router) {}

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/login");
  }

}
