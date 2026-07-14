import { DecimalPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { MovieDetail } from '../../core/models/movie.model';
import { FavoriteService } from '../../core/services/favorite.service';
import { MovieService } from '../../core/services/movie.service';

interface MoviePreview {
  title: string;
  posterPath: string | null;
  voteAverage: number;
}

@Component({
  selector: 'app-detail',
  imports: [DecimalPipe],
  templateUrl: './detail.html',
})
export class Detail {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly movieService = inject(MovieService);
  private readonly favoriteService = inject(FavoriteService);
  private readonly sanitizer = inject(DomSanitizer);

  protected readonly movie = signal<MovieDetail | null>(null);
  protected readonly preview = signal<MoviePreview | null>(null);
  protected readonly loading = signal(true);
  protected readonly error = signal(false);
  protected readonly isFavorite = signal(false);

  protected readonly heroTitle = computed(() => this.movie()?.title ?? this.preview()?.title ?? null);
  protected readonly heroPoster = computed(() => {
    const p = this.movie()?.posterPath ?? this.preview()?.posterPath ?? null;
    return p ? `${environment.tmdbImageBaseUrl}/w500${p}` : null;
  });
  protected readonly heroVote = computed(() => this.movie()?.voteAverage ?? this.preview()?.voteAverage ?? null);

  constructor() {
    const state = this.router.getCurrentNavigation()?.extras.state as MoviePreview | undefined;
    if (state?.title) this.preview.set(state);

    this.route.paramMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      const id = Number(params.get('id'));
      if (id) this.load(id);
    });
  }

  private load(id: number): void {
    this.loading.set(true);
    this.error.set(false);
    this.movie.set(null);

    this.movieService.getDetail(id).subscribe({
      next: (m) => {
        this.movie.set(m);
        this.loading.set(false);
      },
      error: () => {
        this.error.set(true);
        this.loading.set(false);
      },
    });

    this.favoriteService.getAll().subscribe({
      next: (favs) => this.isFavorite.set(favs.some((f) => f.tmdbMovieId === id)),
    });
  }

  protected toggleFavorite(): void {
    const m = this.movie();
    if (!m) return;
    if (this.isFavorite()) {
      this.favoriteService.remove(m.tmdbId).subscribe(() => this.isFavorite.set(false));
    } else {
      this.favoriteService
        .add({ tmdbMovieId: m.tmdbId, title: m.title, posterPath: m.posterPath, voteAverage: m.voteAverage })
        .subscribe(() => this.isFavorite.set(true));
    }
  }

  protected trailerUrl(): SafeResourceUrl | null {
    const key = this.movie()?.trailerYoutubeKey;
    return key
      ? this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${key}`)
      : null;
  }
}
