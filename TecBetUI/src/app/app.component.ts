import { Router } from '@angular/router';
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
  credentials: LoginModel = {userName:'', password:'', role: '', id: '', balance: 0, email: ''};
  title = 'Soccer-Database';
  scrolled = 0;
  showDropdown = false;
  timer: any;
  idleTimer: any;
  updateSubscription: Subscription;

  constructor(private router: Router, private jwtHelper: JwtHelperService, private loginService: LoginService, 
    private authService: AuthguardService, private modalService: NgbModal)
  {

    //Here loginservice is used to update the credentials everytime component is loaded (all the time cos navbar)
    //Timer is strictly for hiding the dropdown menu if not being used

    this.startTimer();

    this.startIdleTimer();

    this.loginService.currentCredentials.subscribe(credentials => {
      this.credentials = credentials;
    });   
 

//Fetching stuff from localstorage

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString)
    {
      this.credentials =  JSON.parse(storedCredentialsString);
    }

    //Repeated API calls for balance update, maybe theres a better way to do this? Tried to bind to localstorage and eventually reached conclusion
    //that it would require the same api calls to update.

    this.updateSubscription = interval(1000).pipe(
      switchMap(() => this.authService.getUser(this.credentials.userName))
    ).subscribe({
      next: (response) => {
      this.credentials.id = response.id
      this.credentials.balance = response.balance,
      this.credentials.userName = response.userName,
      this.credentials.role = response.role
      },
      error: (response) => {
        console.log(response);
      }
    });   

  }

  openLogin() {
		this.modalService.open(LoginComponent, {centered: true, windowClass: 'modal-login'});
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
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  //Clears localstorage and credential role

  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    localStorage.removeItem("refreshToken");
    this.credentials.role = "";
    this.router.navigateByUrl("/");
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
      this.resetTimer();
    }, 4000); // 4 seconds
  }

  startIdleTimer() {
    this.idleTimer = setTimeout(() => {
      this.logOut();
    }, 600000); // 600 seconds
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
    // console.log(`Item "${item}" clicked.`);
    
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
