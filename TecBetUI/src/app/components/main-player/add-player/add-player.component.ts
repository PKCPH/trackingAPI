import { Component } from '@angular/core';
import { Player } from 'src/app/models/player.model';
import { PlayersService } from 'src/app/services/players.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-player',
  templateUrl: './add-player.component.html',
  styleUrls: ['./add-player.component.css']
})
export class AddPlayerComponent {

  addPlayerRequest: Player = {
    id: '',
    name: '',
    age: 0,
    teamId: '',
  }

  constructor(private playerService: PlayersService, private router: Router){ }

  ngOnInit(): void {
    
  }

  addPlayer(){
    this.playerService.addPlayer(this.addPlayerRequest)
    .subscribe({
      next: (player) => {
        this.router.navigate(['players'])
        console.log(player)
      }
    })
  }
}
