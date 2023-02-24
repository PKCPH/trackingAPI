import { Component, ElementRef, ViewChild } from '@angular/core';

interface Animal {
  name: string;
  speed: number;
  strength: number;
  position: number;
  sprite: HTMLImageElement;
}

@Component({
  selector: 'app-main-horseracegame',
  templateUrl: './main-horseracegame.component.html',
  styleUrls: ['./main-horseracegame.component.css']
})

export class MainHorseracegameComponent {

  rndInt = Math.floor(Math.random() * 100) + 1;

  animals: Animal[] = [
    { name: 'Bear', speed: this.rndInt-20, strength: this.rndInt+30, position: 0, sprite: new Image() },
    { name: 'Horse', speed: this.rndInt+30, strength: this.rndInt-10, position: 0, sprite: new Image() },
    { name: 'Rabbit', speed: this.rndInt+20, strength: this.rndInt-20, position: 0, sprite: new Image() },
    { name: 'Rhino', speed: this.rndInt-10, strength: this.rndInt+25, position: 0, sprite: new Image() },
    { name: 'Sloth', speed: this.rndInt-25, strength: this.rndInt-25, position: 0, sprite: new Image() }
  ];

  winner: Animal | null = null;
  winnerMsg: string | any;
  state = false;

  @ViewChild('canvas')
  canvasRef!: ElementRef<HTMLCanvasElement>;

  constructor() {
    this.animals.forEach(animal => {
      animal.sprite.src = `images/${animal.name}.png`;
    });
  }

  drawAnimal = (animal: Animal, x: number, y: number, width: number, height: number) => {
    const canvas = document.getElementById("canvas") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
      if (ctx) {
        ctx.drawImage(animal.sprite, Math.floor(x), Math.floor(y), width, height);
      }
  };

  resetRace() {
    const canvas = document.getElementById("canvas") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
  
    if (ctx) {
      // Clear the canvas
      ctx.clearRect(0, 0, canvas.width, canvas.height);
  }}

  simulateRace() {

      
    let finished = false;

    let rndIntAgane = Math.floor(Math.random() * 100) + 1;

if(this.state === false)
{
  this.state = true;
  console.log(this.state);

    this.animals.forEach(animal => {
      if(animal.name == 'Bear')
      {
        //Sæt lige en ny number generator ind for hvert dyr
      animal.speed = rndIntAgane-20,
      animal.strength = rndIntAgane+30
    }else if(animal.name == 'Horse')
    {
      animal.speed = rndIntAgane+30,
      animal.strength = rndIntAgane-10
    }else if(animal.name == 'Rabbit')
    {
      animal.speed = rndIntAgane+20,
      animal.strength = rndIntAgane-20
    }else if(animal.name == 'Rhino')
    {
      animal.speed = rndIntAgane-10,
      animal.strength = rndIntAgane+25
    }else if(animal.name == 'Sloth')
    {
      animal.speed = rndIntAgane-25,
      animal.strength = rndIntAgane-25
    }
    animal.position = 0;
    });
  
    const canvas = document.getElementById("canvas") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
  
    if (ctx) {
      // Clear the canvas
      ctx.clearRect(0, 0, canvas.width, canvas.height);
  
      // Draw the start and finish lines
      ctx.beginPath();
      ctx.moveTo(50, 0);
      ctx.lineTo(50, canvas.height);
      ctx.stroke();
  
      ctx.beginPath();
      ctx.moveTo(canvas.width - 50, 0);
      ctx.lineTo(canvas.width - 50, canvas.height);
      ctx.stroke();
  
      // Calculate the race distance
      const distance = canvas.width + 4500;
  
      // Animate the animals
      const fps = 60;
      const interval = 1000 / fps;

      let winner: Animal;
  
      const animate = () => {
        if (finished) {
          // Display the winner
          this.winnerMsg = winner.name
          this.state = false;
          return;
        }
  
        ctx.clearRect(0, 0, canvas.width, canvas.height);
  
        // Draw the start and finish lines
        ctx.beginPath();
        ctx.moveTo(50, 0);
        ctx.lineTo(50, canvas.height);
  
        ctx.beginPath();
        ctx.moveTo(canvas.width - 20, 0);
        ctx.lineTo(canvas.width - 20, canvas.height);
        ctx.stroke();

        
  
        // Update the position of each animal
        this.animals.forEach((animal) => {
          const speed = (1.5 * animal.speed) + (Math.random() * 2000) + (1.2 * animal.strength);
          animal.position += speed / fps;
  
          // Draw the animal at its current position
          const x = 0 + (animal.position / distance) * (canvas.width - 30);
          const y = 0 + (this.animals.indexOf(animal) * 30);
          const width = 30;
          const height = 30;
          this.drawAnimal(animal, x, y, width, height);
  
          // Check if the animal has finished the race
          if (animal.position >= distance) {
            finished = true;
            winner = animal;
          }
        });
        setTimeout(animate, interval);
      };
      animate();
    }
  }
}
}





