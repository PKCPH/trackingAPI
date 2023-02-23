import { Component, ElementRef, Renderer2, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { interval, Subscription, switchMap } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-adminboard',
  templateUrl: './adminboard.component.html',
  styleUrls: ['./adminboard.component.css']
})
export class AdminboardComponent implements OnDestroy{

  users: LoginModel[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;
  adminUsername: string = "";

//ngOnDestroy is when you route out of a component it triggers, and inside it I unsubscribe to everything, so it doesnt keep running while on another component.

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }
  
    constructor(private authService: AuthguardService, private router: Router, 
      private el: ElementRef, private renderer: Renderer2) {

        //Basically this is the checker, to prevent users from trying to navigate to this url without being an admin it checks the localstorage for role credentials
        //If you dont have admin you'll be redirected to the homepage

        let storedCredentials;

        let storedCredentialsString = localStorage.getItem("credentials");
        if (storedCredentialsString)
        {
        storedCredentials = JSON.parse(storedCredentialsString);

        let role = storedCredentials.role;
        this.adminUsername = storedCredentials.username;

        if (role === 'Admin') {
          this.router.navigate(['/adminboard']);
          } else {
            this.router.navigate(['/']);
          }  
        }

        this.updateSubscription = interval(1500).pipe(
          switchMap(() => this.authService.getUsers())
        )
        .subscribe({
          next: (users) => {
            this.users = users.map(users => {
              return {
                ...users,
                password: '',
                id: users.id.substring(0, 8),
              }
            });
            // console.log(this.users)
            if (users)
            {
              this.Hideloader();
            }
            this.authService.errorMessage.subscribe(error => {
              this.errorMessage = error;       
            });
          }
        });   
    }
  
    deleteUser(username: string) {
      this.authService.deleteUser(username)
      .subscribe({
        next: (response) => {
          this.authService.getUsers()
            .subscribe({
              next: (users) => {
                this.users = users.map(users => {
                  return {
                    ...users,
                    password: ''
                  }
                });
              },
              error: (response) => {
                console.log(response);
              }
            });
        }
      });
    } 

    //Hides loader, and shows relevant divs that should only appear after loading have finished.
  
    Hideloader() {
              // Setting display of spinner element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#usercontainer'), 'display', 'block');
    }

}
