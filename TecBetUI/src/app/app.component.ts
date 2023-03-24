import { Router, NavigationEnd } from '@angular/router';
import { Component, HostListener, ViewEncapsulation } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from './models/login.model';
import { LoginService } from './services/login.service';
import { Subscription, interval, switchMap } from 'rxjs';
import { AuthguardService } from './services/authguard.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './components/main-login/login/login.component';

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
    //Here loginservice is used to update the credentials everytime component is loaded (all the time cos navbar)

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    });  

    this.updateUserInfo();

    //This will listen for events. userLoggedIn is a custom event made from the login component. When it detects that user has logged in, it'll trigger the updateUserInfo function

    window.addEventListener('userLoggedIn', this.updateUserInfo.bind(this));
    window.addEventListener('userLoggedIn', this.startIdleTimer.bind(this));
    
    this.router.events.subscribe(event => {
      if (event.constructor.name === "NavigationStart") {
        if(this.credentials.userName !== '')
        {
        this.resetIdleTimer();
      }
      }
    })
    
  }

//Login modal function
  openLogin() {
		this.modalService.open(LoginComponent, {centered: true, windowClass: 'modal-login'});
	}

  //UserInfo updates moved out of constructor, but basically does the same as before
  //Creates a temporary string to store credentials from localstorage
  //With the help of JSON.parse we can fill our credentials model with the information from localstorage
  //This is used to maintain user roles, balances and showing of navigation items
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
      this.router.navigateByUrl("/");
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    localStorage.removeItem("refreshToken");
    this.credentials.role = "";
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

  //Timer that starts when you trigger the dropdown menu, after 5 seconds it'll automatically close itself.
  startTimer() {
    console.log(this.idleTimer)
    this.timer = setTimeout(() => {
      this.showDropdown = false;
    }, 3000); // 3 seconds
  }

  //Idle timer that kicks in when the navbar is loaded, when you navigate around it'll retrigger the timer. In that sense it'll act like an "idle timer"
  startIdleTimer() {
    this.idleTimer = setTimeout(() => {
      this.logOut();
    }, 900000); // 900 seconds
  }

 
//Dropdown menu toggler
  toggleDropdown() {
    this.resetTimer();
    this.startTimer();
    this.showDropdown = !this.showDropdown;
  }

  resetTimer() {
    clearTimeout(this.timer);
  }

  resetIdleTimer() {
    clearTimeout(this.idleTimer);
    this.startIdleTimer();
  }

  onItemClick(item: string) {
    // Handle the item click event here.
    // For example, you could emit an event or perform some action based on the clicked item.
    // console.log(`Item "${item}" clicked.`);
    
    // Hide the dropdown box.
    this.showDropdown = false;
  }

   //DropDownButton item definitions, using these strings in .html to do various things
    items = [
    {
      text: 'Dashboard',
    },
    {
      text: 'My Bets',
    },
    {
      text: 'Log Out',
    }
  ];
}
