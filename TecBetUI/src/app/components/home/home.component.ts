import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  isUserAuthenticated = (): boolean => {
    return false
  }
  logOut = () => {
    localStorage.removeItem("jwt");
  }
}
