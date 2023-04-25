import { Component, ComponentFactoryResolver, ElementRef, NgModule, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Leagues } from 'src/app/models/leagues.model';
import { Team } from 'src/app/models/teams.model';
import { LeaguesService } from 'src/app/services/leagues.service';
import { TeamsService } from 'src/app/services/teams.service';
import { interval, Subscription, switchMap } from 'rxjs';
import { Sort, MatSort } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-league',
  templateUrl: './add-league.component.html',
  styleUrls: ['./add-league.component.css']
})
export class AddLeagueComponent implements OnInit {

  teamList: Team[] = [];
  shownTeams: Team[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription | any;
  storedCredentialsString: any;
  role: any;
  sortedTeams: Team[] = [];
  max: number | any;
  addLeagueRequest: Leagues = {
    id: '',
    name: '',
    startDate: '',
    amountOfTeams: '',
    match: [],
  }

  teamModel = {
    id: "",
    name: ""
  }

  //ViewChield is used to fetch a components html element object
  @ViewChild(MatSort, { static: true }) sort: MatSort | any;

  leagueForm: FormGroup | any;
  submitted = false;

  constructor(
    private leagueService: LeaguesService,
    private router: Router,
    private formBuilder: FormBuilder,
    private teamsService: TeamsService,
    private el: ElementRef,
    private renderer: Renderer2
  ) {
    this.buildValidator();
  }

  ngOnInit(): void {
    this.teamsService.getAvailableTeams()
      .subscribe({
        next: (teams) => {
          console.log(teams);
          this.teamList = teams
          this.buildValidator();
        }
      })
  }

  buildValidator() {
    this.leagueForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      startDate: ['', [Validators.required]],
      amountOfTeams: ['', [Validators.required, Validators.min(2), this.maxValidator.bind(this)]]
    });
  }

  maxValidator(control: any) {
    if (control.value > this.teamList.length) {
      return { 'max': true };
    }
    return null;
  }


  addLeague() {
    this.submitted = true;
    if (this.leagueForm.valid) {
      console.log("leagueForm Valid")
      if (this.addLeagueRequest) {
        this.addLeagueRequest = {
          ...this.addLeagueRequest,
          name: this.leagueForm.get('name')?.value,
          startDate: this.leagueForm.get('startDate')?.value,
          amountOfTeams: this.leagueForm.get('amountOfTeams')?.value
        };
      }
      console.log("3434343", this.addLeagueRequest);
      this.leagueService.addLeague(this.addLeagueRequest)
        .subscribe({
          next: async (members) => {
            await delay(3000);
            this.router.navigate(['leagues']);
          },
          error: (error) => {
            console.log(error); // Log the error for debugging purposes
          }
        });

    } else {
      for (const key in this.leagueForm.controls) {
        if (this.leagueForm.controls.hasOwnProperty(key)) {
          const control = this.leagueForm.get(key);
          if (control && control.invalid) {
            console.log(key, control.errors);
          }
        }
      }
    }
  }

  fetchTeams() {

    this.teamsService.errorMessage.subscribe(error => {
      this.errorMessage = error;
    });

    this.teamsService.getAvailableTeams()
      .subscribe({
        next: (teams) => {
          this.teamList = teams.map(team => {
            return {
              ...team
            }
          });
          console.log(this.teamList);
          if (teams) {
            this.sortedTeams = this.teamList.slice();
            this.toggleOverflowDiv();
            this.Hideloader();
          }
          if (teams.length > 0) {
            this.errorMessage = "";
          }
        }
      });
  }

  getCredentials() {
    let storedCredentials;

    this.storedCredentialsString = localStorage.getItem("credentials");
    if (this.storedCredentialsString) {
      storedCredentials = JSON.parse(this.storedCredentialsString);

      this.role = storedCredentials.role;
    }
  }

  //Hides loader, and shows relevant divs that should only appear after loading have finished.

  Hideloader() {
    // Setting display of spinner element to none
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
    this.renderer.setStyle(this.el.nativeElement.querySelector('#teamcontainer'), 'display', 'block');
  }


  //Functions to check if a certain table is overflowing, and if it is display a div.

  isTableOverflowing(): boolean {
    return this.el.nativeElement.querySelector('#tablediv').scrollHeight > this.el.nativeElement.querySelector('#tablediv').clientHeight;
  }

  toggleOverflowDiv(): void {
    if (this.isTableOverflowing()) {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#overflow-div'), 'display', 'block');
    } else {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#overflow-div'), 'display', 'none');
    }
  }
}

function delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}