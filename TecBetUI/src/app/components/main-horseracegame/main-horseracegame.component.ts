import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { color } from 'highcharts';

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

  animals: Animal[] = [
    { name: 'Bear', speed: (Math.floor(Math.random() * 100)-20), strength: (Math.floor(Math.random() * 100)+30), position: 0, sprite: new Image() },
    { name: 'Horse', speed: (Math.floor(Math.random() * 100)+30), strength: (Math.floor(Math.random() * 100)-10), position: 0, sprite: new Image() },
    { name: 'Rabbit', speed: (Math.floor(Math.random() * 100)+20), strength: (Math.floor(Math.random() * 100)-20), position: 0, sprite: new Image() },
    { name: 'Rhino', speed: (Math.floor(Math.random() * 100)-10), strength: (Math.floor(Math.random() * 100)+25), position: 0, sprite: new Image() },
    { name: 'Sloth', speed: (Math.floor(Math.random() * 100)-25), strength: (Math.floor(Math.random() * 100)-25), position: 0, sprite: new Image() }
  ];

  winner: Animal | null = null;
  winnerMsg: string | any;
  state = false;
  delay: any;
  countdown: number = 3;
  interval: any;

  @ViewChild('canvas')
  canvasRef!: ElementRef<HTMLCanvasElement>;

  constructor(private el: ElementRef, private renderer: Renderer2) {
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

    this.startTimer()
      
    let finished = false;

    let rndIntAgane = Math.floor(Math.random() * 100) + 1;

    this.delay = setTimeout(() => {  
    this.renderer.setStyle(this.el.nativeElement.querySelector('#go'), 'display', 'none');
    this.countdown = 3;
    clearInterval(this.interval)
    }, 2500)

if(this.state === false)
{
  this.state = true;
  console.log(this.state);

    this.animals.forEach(animal => {
      if(animal.name == 'Bear')
      {
        //Sæt lige en ny number generator ind for hvert dyr
      animal.speed = (Math.floor(Math.random() * 100)-20),
      animal.strength = (Math.floor(Math.random() * 100)+30)
    }else if(animal.name == 'Horse')
    {
      animal.speed = (Math.floor(Math.random() * 100)+30),
      animal.strength = (Math.floor(Math.random() * 100)-10)
    }else if(animal.name == 'Rabbit')
    {
      animal.speed = (Math.floor(Math.random() * 100)+20),
      animal.strength = (Math.floor(Math.random() * 100)-20)
    }else if(animal.name == 'Rhino')
    {
      animal.speed = (Math.floor(Math.random() * 100)-10),
      animal.strength = (Math.floor(Math.random() * 100)+25)
    }else if(animal.name == 'Sloth')
    {
      animal.speed = (Math.floor(Math.random() * 100)-25),
      animal.strength = (Math.floor(Math.random() * 100)-25)
    }
    animal.position = 0;
    });
  
    const canvas = document.getElementById("canvas") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");
  
    if (ctx) {
      // Clear the canvas
      ctx.clearRect(0, 0, canvas.width, canvas.height);
  
      // Draw the start and finish lines
      // ctx.beginPath();
      // ctx.moveTo(50, 0);
      // ctx.lineTo(50, canvas.height);
      // ctx.stroke();
  
      // ctx.beginPath();
      // ctx.moveTo(canvas.width - 50, 0);
      // ctx.lineTo(canvas.width - 50, canvas.height);
      // ctx.stroke();
  
      // Calculate the race distance
      const distance = canvas.width + 10000;
  
      // Animate the animals
      const fps = 20;
      const interval = 1500 / fps;

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
        // ctx.beginPath();
        // ctx.moveTo(50, 0);
        // ctx.lineTo(50, canvas.height);
  
        ctx.beginPath();
        ctx.moveTo(canvas.width - 10, 0);
        ctx.lineTo(canvas.width - 10, canvas.height);
        ctx.fillStyle = "green";
        ctx.fillRect(canvas.width - 10, 0, 300, canvas.height);
        ctx.stroke();

        // Update the position of each animal
        this.animals.forEach((animal) => {
          this.delay = setTimeout(() => {   
          const speed = (1.5 * animal.speed) + (Math.random() * 2000) + (1.2 * animal.strength);
          animal.position += speed / fps;
        }, 1500);
  
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

startTimer() {
  this.renderer.setStyle(this.el.nativeElement.querySelector('#countdown'), 'display', 'block');
  this.interval = setInterval(() => {
    if(this.countdown > 1) {
        this.countdown--;
    } else {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#countdown'), 'display', 'none');
      this.renderer.setStyle(this.el.nativeElement.querySelector('#go'), 'display', 'block');
    }
  },500)
}
}





