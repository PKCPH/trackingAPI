import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.css']
})
export class PlayerListComponent {
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