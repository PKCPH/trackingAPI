import { AuthenticatedResponse } from '../models/AuthenticatedResponse';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel } from '../models/login.model';
import { Observable, catchError, of } from 'rxjs';
import { NgForm } from '@angular/forms';
import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthguardService implements CanActivate  {

  invalidLogin: boolean = false;
  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000'};

  baseApiUrl: string = 'https://localhost:5001';

  constructor(private router:Router, private jwtHelper: JwtHelperService, private http: HttpClient, private loginService: LoginService){}
  
  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = localStorage.getItem("jwt");

    if (token && !this.jwtHelper.isTokenExpired(token)){
      console.log(this.jwtHelper.decodeToken(token))
      return true;
    }

    const isRefreshSuccess = await this.tryRefreshingTokens(token); 
    if (!isRefreshSuccess) { 
      this.router.navigate(["login"]); 
    }

    return isRefreshSuccess;
  }

  private async tryRefreshingTokens(token: string | null): Promise<boolean> {
    // Try refreshing tokens using refresh token
    const refreshToken: string | null = localStorage.getItem("refreshToken");
    if (!token || !refreshToken) { 
      return false;
    }
    
    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    let isRefreshSuccess: boolean;

    const refreshRes = await new Promise<AuthenticatedResponse>((resolve, reject) => {
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/token/refresh", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: AuthenticatedResponse) => resolve(res),
        error: (_) => { reject; isRefreshSuccess = false;}
      });
    });

    localStorage.setItem("jwt", refreshRes.token);
    localStorage.setItem("refreshToken", refreshRes.refreshToken);
    isRefreshSuccess = true;

    return isRefreshSuccess;
  }

  getUser(username: string): Observable<LoginModel> {
    return this.http.get<LoginModel>(this.baseApiUrl + '/api/Auth/' + username);
  }

  register(addUserRequest: LoginModel): Observable<LoginModel> {
    //Adding this cos JSON doesnt like that we dont return anything to our GUID ID field, so we 
    //just return an empty guid thats gonna be overwritten by the API either way
    addUserRequest.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<LoginModel>(this.baseApiUrl + '/api/Auth/register', addUserRequest);
  }

  updateUser(id: string, updateUserRequest: LoginModel): Observable<LoginModel> {
    return this.http.put<LoginModel>(this.baseApiUrl + '/api/Auth/' + id, updateUserRequest);
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

          this.getUser(this.credentials.userName)
          .subscribe({
          next: (response) => {
          this.credentials = response;
          this.loginService.updateCredentials(this.credentials);  
          localStorage.setItem("credentials", JSON.stringify(this.credentials.role));
          }
        });  
        },
        error: (err: HttpErrorResponse) => this.invalidLogin = true
      })
    }
  }

}