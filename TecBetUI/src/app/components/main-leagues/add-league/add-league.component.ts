import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Leagues } from 'src/app/models/leagues.model';
import { LeaguesService } from 'src/app/services/leagues.service';

@Component({
  selector: 'app-add-league',
  templateUrl: './add-league.component.html',
  styleUrls: ['./add-league.component.css']
})
export class AddLeagueComponent implements OnInit {

  addLeagueRequest: Leagues = {
    id: '',
    name: '',
    startDate: '',
    match: []
  }

  leagueForm: FormGroup | any;
  submitted = false;

  constructor(private leagueService: LeaguesService, private router: Router, private formBuilder: FormBuilder) {
  this.buildValidator();
  }
  buildValidator() {
    this.leagueForm = this.formBuilder.group({
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

  addLeague() {
    this.submitted = true;
    if(this.leagueForm.valid){
      if(this.addLeagueRequest){
        this.addLeagueRequest = {
          ...this.addLeagueRequest,
          name: this.leagueForm.get('name')?.value,
          startDate: this.leagueForm.get('startDate')?.value
        };
      }

      this.leagueService.addLeague(this.addLeagueRequest)
      .subscribe({
        next: (members) => {
          this.router.navigate(['leagues']);
        },
        error: (error) => {
          console.log(error); // Log the error for debugging purposes
        }
      });
    } else {
      for (const key in this.leagueForm.controls) {
        if (this.leagueForm.controls.hasOwnProperty(key)){
          const control = this.leagueForm.get(key);
          if(control && control.invalid){
            console.log(key, control.errors);
          }
        }
      }
    }
  }
}
