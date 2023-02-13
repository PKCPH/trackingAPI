import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { LoginModel } from 'src/app/models/login.model';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { NgForm } from '@angular/forms';
import { AuthguardService } from 'src/app/services/authguard.service';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean = false;
  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000'};

  constructor(private router: Router, private http: HttpClient, private authguard: AuthguardService, private loginService: LoginService) { }
  
  ngOnInit(): void {
    
  }

  login = ( form: NgForm) => {
    if (form.valid) {
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
            username: this.credentials.userName,
            role: this.credentials.role
          };

          localStorage.setItem("credentials", JSON.stringify(storedCredentials));
          }
        });  
        },
        error: (err: HttpErrorResponse) => this.invalidLogin = true
      })
    }
  }
}
