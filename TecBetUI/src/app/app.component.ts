import { Router, NavigationEnd } from '@angular/router';
import { Component, HostListener, ViewEncapsulation } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './models/login.model';
import { LoginService } from './services/login.service';
import { Subscription, interval, switchMap } from 'rxjs';
import { AuthguardService } from './services/authguard.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './components/main-login/login/login.component';
import * as jwt from 'jsonwebtoken';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  credentials: LoginModel | any;
  title = 'Soccer-Database';
  scrolled = 0;
  showDropdown = false;
  timer: any;
  idleTimer: any;
  updateSubscription: Subscription | any;
  updating = false;
  userAuthenticated = false;
  // isLoggedin = false;

  constructor(private router: Router, private jwtHelper: JwtHelperService, private loginService: LoginService, 
    private authService: AuthguardService, private modalService: NgbModal)
  {
    //If there is credentials filled, make it do the checkAuthGuarD() function !!!

    //Here loginservice is used to update the credentials everytime component is loaded (all the time cos navbar)
    //Timer is strictly for hiding the dropdown menu if not being used

    this.startIdleTimer();

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    });  

    this.updateUserInfo();

    window.addEventListener('userLoggedIn', this.updateUserInfo.bind(this));
  }


  openLogin() {
		this.modalService.open(LoginComponent, {centered: true, windowClass: 'modal-login'});
	}

  updateUserInfo() {
    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString)
    {
      this.credentials =  JSON.parse(storedCredentialsString);
      this.updateSubscription = interval(1500).pipe(
        switchMap(() => this.authService.getUser(this.credentials.userName))
      ).subscribe({
        next: (response) => {
        this.credentials.id = response.id
        this.credentials.balance = response.balance,
        this.credentials.userName = response.userName,
        this.credentials.role = response.role,
        this.credentials.password = ""
        // console.log(this.credentials);
        },
        error: (response) => {
          console.log(response);
        }
      }); 
    }
  }

  //Basic boolean function that checks if youre on a certain page

  isRegisterPage = (): boolean => {
    if (this.router.url.includes('/register')) 
  {  
     return true; 
  }
return false;
  }

  isTestingPage = (): boolean => {
    if (this.router.url.includes('/horserace')) 
  {  
     return true; 
  }
return false;
  }

  //Checks for token 

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    // if (token){
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    localStorage.removeItem("refreshToken");
    return false;
  }

  //Clears localstorage and credential role

  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    localStorage.removeItem("refreshToken");
    this.credentials.role = "";
    this.router.navigateByUrl("/");
    this.updateSubscription.unsubscribe();
    this.userAuthenticated = false;
  }

  //Listener on scroll, any scrolling on the page will trigger this function and set a number to either 1 or 0 (1 for it being scrolled)

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
 
    }, 5000); // 4 seconds
  }

  startIdleTimer() {
    this.idleTimer = setTimeout(() => {
      this.logOut();
    }, 900000); // 900 seconds
  }

 

  toggleDropdown() {
    this.startTimer();
    this.showDropdown = !this.showDropdown;
  }

  onItemClick(item: string) {
    // Handle the item click event here.
    // For example, you could emit an event or perform some action based on the clicked item.
    // console.log(`Item "${item}" clicked.`);
    
    // Hide the dropdown box.
    this.showDropdown = false;
  }

   //DropDownButton items definition
    items = [
    {
        text: 'Dashboard',
    },
    {
      text: 'My Bets'
    },
    {
      text: 'Log Out',
    }
  ];
}
