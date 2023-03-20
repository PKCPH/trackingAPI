import { AfterViewInit, Component, ElementRef, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements AfterViewInit {

  // rndInt = Math.floor(Math.random() * 5) + 1;
  rndInt = 1;
  videoSrc = `assets/montage${this.rndInt}.mp4`;
  
  constructor(private el: ElementRef, private renderer: Renderer2) { }

  
  ngAfterViewInit() {

    const video = document.querySelector('video');

    if (video && this.rndInt === 4) {
      const mediaQuery = window.matchMedia("(max-height: 968px)").matches;
      video.style.left = mediaQuery ? "3.2%" : "1.5%";
      video.style.width = mediaQuery ? "1800px" : "1865px";
    }

    if (video && this.rndInt === 5) {
      const mediaQuery = window.matchMedia("(max-height: 968px)").matches;
      video.style.top = mediaQuery ? "-6%" : "-6%";
      video.style.left = mediaQuery ? "4.6%" : "2.5%";
      video.style.width = mediaQuery ? "1765px" : "1825px";
    }

  }

restartVideo() {
    this.rndInt = Math.floor(Math.random() * 5) + 1;
    this.videoSrc = `assets/montage${this.rndInt}.mp4`;
    const video = document.querySelector('video');
    if (video)
    {
    video.src = this.videoSrc;
    video.currentTime = 0;
    video.play();
  }
}
}
