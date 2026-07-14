import { Component, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { MovieSummary } from '../../core/models/movie.model';
import { MovieService } from '../../core/services/movie.service';
import { MovieGrid } from '../../shared/movie-grid/movie-grid';

@Component({
  selector: 'app-home',
  imports: [MovieGrid],
  templateUrl: './home.html',
})
export class Home {
  private readonly movieService = inject(MovieService);

  protected readonly trending = signal<MovieSummary[]>([]);
  protected readonly popular = signal<MovieSummary[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal(false);

  constructor() {
    forkJoin({
      trending: this.movieService.getTrending(),
      popular: this.movieService.getPopular(),
    }).subscribe({
      next: ({ trending, popular }) => {
        this.trending.set(trending);
        this.popular.set(popular);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }
}
