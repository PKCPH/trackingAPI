import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent {

  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};

  constructor(private authService: AuthguardService, private route: ActivatedRoute, private router: Router) {

    this.route.paramMap.subscribe({
      next: (params) => {
        const userName = params.get('username');
        if (userName) {
          //Call API
this.authService.getUser(userName)
.subscribe({
  next: (response) => {
this.credentials.balance = response.balance,
this.credentials.email = response.email,
this.credentials.userName = response.userName,
this.credentials.role = response.role
console.log(this.credentials);
  }
});
        }
      }
    })
  }

  deleteUser(username: string) {
    this.authService.deleteUser(username)
    .subscribe({
      next: (response) => {
        localStorage.removeItem("jwt");
        localStorage.removeItem("credentials");
        this.credentials.role = "";
        this.router.navigate(['/'])
      }
    })
  }
  
}
