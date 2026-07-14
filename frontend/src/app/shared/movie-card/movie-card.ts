import { DecimalPipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-movie-card',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './movie-card.html',
})
export class MovieCard {
  readonly tmdbId = input.required<number>();
  readonly title = input.required<string>();
  readonly posterPath = input<string | null>(null);
  readonly voteAverage = input<number>(0);

  protected posterUrl(): string | null {
    const p = this.posterPath();
    return p ? `${environment.tmdbImageBaseUrl}/w500${p}` : null;
  }
}
