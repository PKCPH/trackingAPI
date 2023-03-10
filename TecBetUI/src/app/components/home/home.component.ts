import { AfterViewInit, Component, ViewChild } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  
  constructor() { 
    const video = document.querySelector('video');
    if (video)
    {
    video.play;
  }
  }

restartVideo() {
    const video = document.querySelector('video');
    if (video)
    {
    video.currentTime = 0;
    video.play();
  }
}
}
