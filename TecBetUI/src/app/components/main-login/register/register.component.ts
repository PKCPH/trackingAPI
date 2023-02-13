import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  
  addLoginRequest: LoginModel = {
    id: '',
    username: '',
    password: '',
    role: ''
  };
  
  loginForm: FormGroup;
  submitted = false;
  invalidLogin: boolean = false;
  
  constructor(private authguardService: AuthguardService, private router: Router, private formBuilder: FormBuilder, private http: HttpClient, private loginService: LoginService) {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.minLength(6), Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }
  
  registerUser() {
    this.submitted = true;
    if (this.loginForm.valid)
    {
      if (this.addLoginRequest)
      {
  this.addLoginRequest = {
  ...this.addLoginRequest,
  username: this.loginForm.get('username')?.value,
  password: this.loginForm.get('password')?.value
  };
  }
    this.authguardService.register(this.addLoginRequest)
    .subscribe({
      next: (members) => {
        if (this.loginForm.valid) {
          this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.addLoginRequest, {
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
              this.authguardService.getUser(this.addLoginRequest.username)
              .subscribe({
              next: (response) => {
              this.addLoginRequest = response;
              this.loginService.updateCredentials(this.addLoginRequest);  
              localStorage.setItem("credentials", JSON.stringify(this.addLoginRequest.role));
              }
            });  
            },
            error: (err: HttpErrorResponse) => this.invalidLogin = true
          })
        }
      }
    });
  } else
  {
    for (const key in this.loginForm.controls) {
      if (this.loginForm.controls.hasOwnProperty(key)) {
        const control = this.loginForm.get(key);
        if (control && control.invalid) {
          console.log(key, control.errors);
        }
      }
    }
  }
  }

}
