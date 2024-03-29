import { Router } from '@angular/router';
import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { LoginModel } from 'src/app/models/login.model';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { NgForm } from '@angular/forms';
import { AuthguardService } from 'src/app/services/authguard.service';
import { LoginService } from 'src/app/services/login.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Location } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnDestroy {
  invalidLogin: boolean = false;
  invalidConn: boolean = false;
  credentials: LoginModel = { userName: '', password: '', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: '' };

  constructor(private router: Router, private http: HttpClient, private authguard: AuthguardService, private loginService: LoginService,
    private el: ElementRef, private renderer: Renderer2, public activeModal: NgbActiveModal, private location: Location) { }

  // Login function is fairly simple, it takes the credentials model - and populates it with user inputs. Then it does a post to the API, where it gets authenticated if the user inputs
  // matches a set of login information in the database.
  // Couple booleans are made to ensure that both the login is valid and that the connection is valid, otherwise a error msg will be displayed
  //It also sets the user inputs as credentials if vaild in localstorage for further use mainly username and role.
  //Also sends out a custom "event" that lets other components know that the user has logged in.

  login = (form: NgForm) => {
    if (form.valid) {
      this.showLoader();
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.credentials, {
        headers: new HttpHeaders({ "Content-Type": "application/json" })
      })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            const token = response.token;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token);
            localStorage.setItem("refreshToken", refreshToken);
            this.invalidLogin = false;

            this.authguard.getUser(this.credentials.userName)
              .subscribe({
                next: (response) => {
                  this.credentials = response;
                  this.loginService.updateCredentials(this.credentials);

                  let storedCredentials = {
                    userName: this.credentials.userName,
                    role: this.credentials.role,
                  };

                  localStorage.setItem("credentials", JSON.stringify(storedCredentials));

                  const event = new CustomEvent('userLoggedIn');
                  window.dispatchEvent(event);

                  // If theres no main component behind the locked component, it'll forward you to a non existent page, for that I've created my own custom 404 page, that will handle unexpected navigation
                  // This if statement will make sure you return to your intended location (after logging in)

                  if (this.router.url.includes('404')) {
                    this.location.back();
                  }
                  this.hideLoader();
                  this.activeModal.close();
                }
              });
          },
          error: (err: HttpErrorResponse) => {
            if (err.status == 401) {
              this.hideLoader();
              this.invalidConn = false;
              this.invalidLogin = true;
            }
            else {
              this.hideLoader();
              this.invalidLogin = false;
              this.invalidConn = true;
            }
          }
        })
    }
  }

  close() {
    this.activeModal.close();
  }

  ngOnDestroy() {
    if (this.router.url.includes("404")) {
      this.router.navigateByUrl('/');
    }
  }

  showLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'block');
  }

  hideLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
  }

}
