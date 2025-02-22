import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    if (!this.accountService.isAuthenticated())
    {
      this.accountService.logout();
    }
  }



  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/login");
  }

}
