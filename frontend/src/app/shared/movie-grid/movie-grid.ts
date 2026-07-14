import { Component, input } from '@angular/core';
import { MovieSummary } from '../../core/models/movie.model';
import { MovieCard } from '../movie-card/movie-card';

@Component({
  selector: 'app-movie-grid',
  imports: [MovieCard],
  templateUrl: './movie-grid.html',
})
export class MovieGrid {
  readonly movies = input.required<MovieSummary[]>();
}
