import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/Auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged In Successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      // complete part
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn(); // if there is a token return true and if not return false.
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out!');
    this.router.navigate(['/home']);
  }

}
