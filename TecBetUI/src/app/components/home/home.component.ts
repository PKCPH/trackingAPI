import { AfterViewInit, Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('1.5s ease-in', style({ opacity: 1 })),
      ]),
    ]),
  ],
})
export class HomeComponent implements AfterViewInit {

  rndInt = Math.floor(Math.random() * 5) + 1;
  // rndInt = 2;
  videoSrc = `assets/montage${this.rndInt}.mp4`;

  constructor(private el: ElementRef, private renderer: Renderer2) { }



  ngAfterViewInit() {

    const video = document.querySelector('video');

    if (video && this.rndInt === 4) {
      const mediaQuery = window.matchMedia("(max-height: 950px)").matches;
      video.style.left = mediaQuery ? "3.2%" : "1.5%";
      video.style.width = mediaQuery ? "93%" : "97.4%";
    }

    if (video && this.rndInt === 5) {
      const mediaQuery = window.matchMedia("(max-height: 950px)").matches;
      video.style.top = mediaQuery ? "-6%" : "-6%";
      video.style.left = mediaQuery ? "4.6%" : "2.5%";
      video.style.width = mediaQuery ? "91%" : "95.2%";
    }

  }

  restartVideo() {
    this.rndInt = Math.floor(Math.random() * 5) + 1;
    this.videoSrc = `assets/montage${this.rndInt}.mp4`;
    const video = document.querySelector('video');

    if (video && this.rndInt === 4) {
      const mediaQuery = window.matchMedia("(max-height: 950px)").matches;
      video.style.left = mediaQuery ? "3.2%" : "1.5%";
      video.style.width = mediaQuery ? "93%" : "97.4%";
      video.src = this.videoSrc;
      video.currentTime = 0;
      video.play();
    }

    else if (video && this.rndInt === 5) {
      const mediaQuery = window.matchMedia("(max-height: 950px)").matches;
      video.style.top = mediaQuery ? "-6%" : "-6%";
      video.style.left = mediaQuery ? "4.6%" : "2.5%";
      video.style.width = mediaQuery ? "91%" : "95.2%";
      video.src = this.videoSrc;
      video.currentTime = 0;
      video.play();
    }

    else if (video) {
      video.src = this.videoSrc;
      video.currentTime = 0;
      video.play();
    }
  }
}
