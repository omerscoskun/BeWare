import { DecimalPipe } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';
import { MovieService } from '../../core/services/movie.service';

@Component({
  selector: 'app-movie-card',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './movie-card.html',
})
export class MovieCard {
  private readonly movieService = inject(MovieService);

  readonly tmdbId = input.required<number>();
  readonly title = input.required<string>();
  readonly posterPath = input<string | null>(null);
  readonly voteAverage = input<number>(0);

  private prefetched = false;

  protected posterUrl(): string | null {
    const p = this.posterPath();
    return p ? `${environment.tmdbImageBaseUrl}/w500${p}` : null;
  }

  protected prefetch(): void {
    if (this.prefetched) return;
    this.prefetched = true;
    this.movieService.getDetail(this.tmdbId()).subscribe({ error: () => (this.prefetched = false) });
  }

  protected navState() {
    return { title: this.title(), posterPath: this.posterPath(), voteAverage: this.voteAverage() };
  }
}
