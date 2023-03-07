import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from 'src/app/models/player.model';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-players-on-team',
  templateUrl: './players-on-team.component.html',
  styleUrls: ['./players-on-team.component.css']
})
export class PlayersOnTeamComponent {

  players: Player[] = [];
  id: string = '';
  constructor(private route: ActivatedRoute, private teamService: TeamsService, private router: Router) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if(id){
          this.id = id
          this.teamService.getPlayers(id)
          .subscribe({
            next: (players) => {
              console.log(players);
              this.players = players
            },
            error: (response) => {
              console.log(response);
            }
          })
        }       
      }
    })
    
  }
  GoAddPlayers() {
    this.router.navigateByUrl('/teams/players/' + this.id + '/add')
  }
  NewPlayer(){
    this.router.navigateByUrl('/teams/players/' + this.id + '/new')
  }

  ChangePlayer(playerId: string){
    this.router.navigateByUrl('/teams/players/' + this.id + '/change/' + playerId)
  }
}
