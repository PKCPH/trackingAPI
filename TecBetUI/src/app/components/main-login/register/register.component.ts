import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, Renderer2 } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, of, tap, throwError } from 'rxjs';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { LoginService } from 'src/app/services/login.service';
import { Location } from '@angular/common'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnDestroy {
  
  addLoginRequest: LoginModel = {
    id: '',
    userName: '',
    password: '',
    role: '',
    balance: 0,
    email: ''
  };
  
  loginForm: FormGroup | any;
  submitted = false;
  errorMessage: string = "";
  invalidRegister: boolean = true;
  
  
  constructor(private authguardService: AuthguardService, private router: Router, private formBuilder: 
    FormBuilder, private http: HttpClient, private loginService: LoginService,
    private el: ElementRef, private renderer: Renderer2, private location: Location) {

this.buildValidator();

  }

  ngOnDestroy(): void {
    const event = new CustomEvent('userLoggedIn');
    window.dispatchEvent(event);
  }

  buildValidator() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.minLength(6), Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }
  
  registerUser() {
    this.showLoader();
    this.submitted = true;
    if (this.loginForm.valid) {
      if (this.addLoginRequest) {
  this.addLoginRequest = {
  ...this.addLoginRequest,
  userName: this.loginForm.get('username')?.value,
  password: this.loginForm.get('password')?.value
  };
  }
    this.authguardService.register(this.addLoginRequest).pipe(
      tap(logins => {
        this.errorMessage = '';
      }),
      catchError((error: HttpErrorResponse) => {
        
        if (error.status === 500) {
          this.errorMessage = 'Username already exists.'; 
        } else {
          this.errorMessage = 'Http failure response';
        }
        this.hideLoader();
        this.invalidRegister = true;
        return of(error);
      })
    )
    .subscribe({
      next: (members) => {
        if (this.loginForm.valid && this.errorMessage === '') {
          this.hideLoader();
          this.invalidRegister = false;
          this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.addLoginRequest, {
            headers: new HttpHeaders({ "Content-Type": "application/json"})
          })
          .subscribe({
            next: (response: AuthenticatedResponse) => {
              const token = response.token;
              const refreshToken = response.refreshToken;
              localStorage.setItem("jwt", token); 
              localStorage.setItem("refreshToken", refreshToken);
              this.authguardService.getUser(this.addLoginRequest.userName)
              .subscribe({
              next: (response) => {
              this.addLoginRequest = response;
              this.loginService.updateCredentials(this.addLoginRequest);  

              let storedCredentials = {
                userName: this.addLoginRequest.userName,
                role: this.addLoginRequest.role,
              };
    
              localStorage.setItem("credentials", JSON.stringify(storedCredentials));
              }
            });  
            }
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
          this.hideLoader();
          console.log(key, control.errors);
        }
      }
    }
  }
  }

  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("credentials");
    this.addLoginRequest.role = "";
    this.router.navigateByUrl("/");
  }

  showLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'block');
  }

  hideLoader() {
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
  }

  goBack() {
    this.location.back();
  }

}
