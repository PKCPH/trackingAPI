import { Component, ElementRef, Renderer2 } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { MatchesService } from 'src/app/services/matches.service';
import { Match } from 'src/app/models/matches.model';

@Component({
  selector: 'app-main-matches',
  templateUrl: './main-matches.component.html',
  styleUrls: ['./main-matches.component.css']
})
export class MainMatchesComponent {
  matches: Match[] = [];
  deletedObject: boolean = false;
  errorMessage: string = "";
  
    constructor(private matchesService: MatchesService, private router: Router, 
      private location: Location, 
      private el: ElementRef, private renderer: Renderer2) {
  
        this.matchesService.errorMessage.subscribe(error => {
          this.errorMessage = error;
        })
  
      this.matchesService.getAllMatches()
      .subscribe({
        next: (matches) => {
          this.matches = matches.map(match => {
            return {
              ...match,
              availability: match.matchState ? 'Yes' : 'No'
            }
          });
          console.log(this.matches); 
          if (matches)
          {
            this.Hideloader();
          } 
        },
        error: (response) => {
          console.log(response);
        }
      });     
    }
  
    deleteMatch(id: string) {
      this.matchesService.deleteMatch(id)
      .subscribe({
        next: (response) => {
          this.matchesService.getAllMatches()
            .subscribe({
              next: (matches) => {
                this.matches = matches.map(match => {
                  return {
                    ...match,
                    availability: match.matchState ? 'Yes' : 'No'
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
  
    Hideloader() {
              // Setting display of spinner
              // element to none
              this.renderer.setStyle(this.el.nativeElement.querySelector('#addbutton'), 'display', 'inline-block');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
              this.renderer.setStyle(this.el.nativeElement.querySelector('#matchcontainer'), 'display', 'block');
    }
  
    GoAddMatch() {
      this.router.navigateByUrl('/matches/add')
    }
}
