import { Router } from '@angular/router';
import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { LoginModel } from 'src/app/models/login.model';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { NgForm } from '@angular/forms';
import { AuthguardService } from 'src/app/services/authguard.service';
import { LoginService } from 'src/app/services/login.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean = false;
  invalidConn: boolean = false;
  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};

  constructor(private router: Router, private http: HttpClient, private authguard: AuthguardService, private loginService: LoginService,
    private el: ElementRef, private renderer: Renderer2, public activeModal: NgbActiveModal) { }
  
  ngOnInit(): void {
    
  }

  login = ( form: NgForm) => {
    if (form.valid) {
      this.showLoader();
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.credentials, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      })
      .subscribe({
        next: (response: AuthenticatedResponse) => {
          const token = response.token;
          const refreshToken = response.refreshToken;
          localStorage.setItem("jwt", token); 
          localStorage.setItem("refreshToken", refreshToken);
          this.invalidLogin = false; 
          this.router.navigate(["/"]);

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
          this.hideLoader();
          this.activeModal.close();
          }
        });  
        },
        error: (err: HttpErrorResponse) => 
        {
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

  showLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'block');
  }

  hideLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
  }

}
