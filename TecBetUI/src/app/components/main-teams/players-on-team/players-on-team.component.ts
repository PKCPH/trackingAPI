import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';

@Component({
  selector: 'app-players-on-team',
  templateUrl: './players-on-team.component.html',
  styleUrls: ['./players-on-team.component.css']
})
export class PlayersOnTeamComponent {

  players: Player[] = [];
  constructor(private playersService: PlayersService, private router: Router) {}

  ngOnInit(): void {
    this.playersService.getAllPlayers()
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
  GoAddPlayer() {
    this.router.navigateByUrl('/players/add')
  }
}
