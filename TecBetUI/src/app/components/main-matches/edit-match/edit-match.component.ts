import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Match } from 'src/app/models/matches.model';
import { MatchesService } from 'src/app/services/matches.service';

@Component({
  selector: 'app-edit-match',
  templateUrl: './edit-match.component.html',
  styleUrls: ['./edit-match.component.css']
})
export class EditMatchComponent {

  matchForm: FormGroup | any;
  submitted = false;

  matchDetails: Match | any;

  constructor(private route: ActivatedRoute, private matchesService: MatchesService, private router: Router, private formBuilder: FormBuilder) {

    this.buildValidator();

    this.fetch();

  }

  //Includes GetOne + getID
  fetch() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          //Call API
          this.matchesService.getMatch(id)
            .subscribe({
              next: (response) => {
                this.matchDetails = response;
              }
            });
        }
      }
    })
  }

  buildValidator() {
    this.matchForm = this.formBuilder.group({
      matchState: [0, [Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }

  updateMatch() {
    this.submitted = true;
    if (this.matchForm.valid) {
      if (this.matchDetails) {
        this.matchDetails = {
          ...this.matchDetails,
          // name: this.matchForm.get('name')?.value
        }
      }

      this.matchesService.updateMatch(this.matchDetails.id, this.matchDetails)
        .subscribe({
          next: (response) => {
            this.router.navigate(['matches']);
          }
        });
    } else {
      for (const key in this.matchForm.controls) {
        if (this.matchForm.controls.hasOwnProperty(key)) {
          const control = this.matchForm.get(key);
          if (control && control.invalid) {
            console.log(key, control.errors);
          }
        }
      }
    }
  }
}
