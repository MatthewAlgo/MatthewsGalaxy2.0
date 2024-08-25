import { Component } from '@angular/core';
import { AfterViewInit, ElementRef, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-applicationblogintro',
  templateUrl: './applicationblogintro.component.html',
  styleUrl: './applicationblogintro.component.css',
  standalone: true,
})
export class ApplicationblogintroComponent implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    const blogContainer =
      this.el.nativeElement.querySelector('.blog-container');

    let observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            this.renderer.addClass(entry.target, 'zoom-in');
            this.renderer.removeClass(entry.target, 'zoom-out');
          } else {
            this.renderer.addClass(entry.target, 'zoom-out');
            this.renderer.removeClass(entry.target, 'zoom-in');
          }
        });
      },
      { threshold: 0.1 }
    );

    observer.observe(blogContainer);
  }

  blogIntroContent = `Welcome to Matthew's Galaxy, a place where each thought sparkles like a star in an infinite expanse of curiosity and wonder. Here, my cosmic musings traverse the vast reaches of space and beyond, delving into the mysteries of the universe, the beauty of the cosmos, and the awe-inspiring journey of exploration and discovery.
    As you navigate through my celestial constellation of ideas, you'll encounter nebulae of insight, clusters of creativity, and the occasional supernova of inspiration. Whether you're a seasoned stargazer or simply curious about the wonders that lie beyond our world, this is a place for you to dream big, ponder the unknown, and connect with the cosmos in a way that sparks your imagination.
    So buckle up your space suit and join me on this extraordinary journey through the stars. Let's explore the universe together and illuminate our minds with the brilliance of the night sky. Welcome to Matthew's Galaxy!`;
}
