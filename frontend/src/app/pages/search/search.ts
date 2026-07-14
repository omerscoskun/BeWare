import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { MovieSummary } from '../../core/models/movie.model';
import { MovieService } from '../../core/services/movie.service';
import { MovieGrid } from '../../shared/movie-grid/movie-grid';

@Component({
  selector: 'app-search',
  imports: [MovieGrid],
  templateUrl: './search.html',
})
export class Search {
  private readonly route = inject(ActivatedRoute);
  private readonly movieService = inject(MovieService);

  protected readonly query = signal('');
  protected readonly results = signal<MovieSummary[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal(false);

  constructor() {
    this.route.queryParamMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const q = params.get('query') ?? '';
      this.query.set(q);
      q ? this.runSearch(q) : this.results.set([]);
    });
  }

  private runSearch(q: string): void {
    this.loading.set(true);
    this.error.set(false);
    this.movieService.search(q).subscribe({
      next: (r) => {
        this.results.set(r);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });
  }
}
