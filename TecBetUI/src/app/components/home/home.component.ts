import { AfterViewInit, Component, ElementRef, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements AfterViewInit {

  rndInt = Math.floor(Math.random() * 5) + 1;
  // rndInt = 5;
  videoSrc = `assets/montage${this.rndInt}.mp4`;
  
  constructor(private el: ElementRef, private renderer: Renderer2) { }

  
  ngAfterViewInit() {

    const video = document.querySelector('video');

    if (video && this.rndInt === 4) {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#videodiv'), 'left', '1.5%');
      video.style.width = "1865px";
    }

    if (video && this.rndInt === 5) {
      this.renderer.setStyle(this.el.nativeElement.querySelector('#videodiv'), 'left', '2.5%');
      this.renderer.setStyle(this.el.nativeElement.querySelector('#videodiv'), 'top', '-6%');
      video.style.width = "1825px";
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
