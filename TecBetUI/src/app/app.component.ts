import { Router } from '@angular/router';
import { Component, OnInit, HostListener, ViewEncapsulation } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './models/login.model';
import { LoginService } from './services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  credentials: LoginModel = {userName:'', password:'', role: '', id: '', balance: 0, email: ''};
  title = 'Soccer-Database';
  scrolled = 0;
  showDropdown = false;
  timer: any;

  constructor(private router: Router, private jwtHelper: JwtHelperService, private loginService: LoginService)
  {

    this.startTimer();

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    }); 

    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString)
    {
    storedCredentials = JSON.parse(storedCredentialsString);
    let role = storedCredentials.role;
    let displayName = storedCredentials.username;
    this.credentials.role = role;
    this.credentials.userName = displayName; 
    }

  }

  isRegisterPage = (): boolean => {
    if (this.router.url.includes('/register')) 
  {  
     return true; 
  }
return false;
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
    this.router.navigateByUrl("/");
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll($event: any) {
    const numb = window.scrollY;
    if (numb >= 50){
      this.scrolled = 1;
      this.showDropdown = false;
    }
    else {
      this.scrolled = 0;
    }
  }

  startTimer() {
    this.timer = setTimeout(() => {
      this.showDropdown = false;
    }, 4000); // 4 seconds
  }

  resetTimer() {
    clearTimeout(this.timer);
    this.startTimer();
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  onItemClick(item: string) {
    // Handle the item click event here.
    // For example, you could emit an event or perform some action based on the clicked item.
    console.log(`Item "${item}" clicked.`);
    
    // Hide the dropdown box.
    this.showDropdown = false;
    this.resetTimer();
  }

   //DropDownButton items definition
    items = [
    {
        text: 'Dashboard',
    },
    {
        text: 'Log Out',
    }];
}
