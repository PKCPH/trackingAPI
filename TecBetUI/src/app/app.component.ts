import { Router } from '@angular/router';
import { Component, OnInit, HostListener } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './models/login.model';
import { LoginService } from './services/login.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  credentials: LoginModel = {username:'', password:'', role: '', id: ''};
  title = 'Soccer-Database';
  scrolled = 0;

  constructor(private router: Router, private jwtHelper: JwtHelperService, private loginService: LoginService)
  {

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    }); 

    let storedCredentials = localStorage.getItem("credentials");
    if (storedCredentials) {
    this.credentials.role = JSON.parse(storedCredentials);
    }

  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    this.credentials.role = "";
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll($event: any) {
    const numb = window.scrollY;
    if (numb >= 50){
      this.scrolled = 1;
    }
    else {
      this.scrolled = 0;
    }
  }
}
