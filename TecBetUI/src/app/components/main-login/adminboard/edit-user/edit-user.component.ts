import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent {

  Userdetails: LoginModel = { userName: '', password: '', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: '' };
  loginForm: FormGroup | any;
  submitted = false;
  errorMessage: string = "";
  timer: any;
  adminUsername: any;

  constructor(private authService: AuthguardService, private route: ActivatedRoute, private formBuilder:
    FormBuilder, private router: Router) {

    this.checkCredentials();

    this.buildValidator();

    this.fetch();
  }

  fetch() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const username = params.get('username');
        if (username) {
          //Call API
          this.authService.getUser(username)
            .subscribe({
              next: (response) => {
                this.Userdetails = response;
                this.Userdetails.id = response.id;
              }
            });
        }
      }
    })
  }

  buildValidator() {
    this.loginForm = this.formBuilder.group({
      name: [this.Userdetails.userName, [Validators.required]],
      id: [this.Userdetails.id],
      role: [this.Userdetails.role],
      balance: [100, [Validators.min(100), Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }

  checkCredentials() {
    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString) {
      storedCredentials = JSON.parse(storedCredentialsString);

      let role = storedCredentials.role;
      this.adminUsername = storedCredentials.userName;

      if (role === 'Admin') {
        // console.log("youre good!");
      } else {
        this.router.navigate(['/']);
      }
    }
  }

  updateUser() {
    this.submitted = true;
    if (this.loginForm.valid) {
      if (this.Userdetails) {
        this.Userdetails = {
          ...this.Userdetails,
          balance: this.loginForm.get('balance')?.value,
        }
      }

      this.authService.updateUser(this.Userdetails.id, this.Userdetails)
        .subscribe({
          next: (response) => {
            this.router.navigate(['adminboard']);
          }
        });
    } else {
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
