import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AutoAnimate } from '../../shared/auto-animate.directive';
import { environment } from '../../../environments/environment';
import { FavoriteDto } from '../../core/models/movie.model';
import { FavoriteService } from '../../core/services/favorite.service';

@Component({
  selector: 'app-favorites',
  imports: [RouterLink, AutoAnimate],
  templateUrl: './favorites.html',
})
export class Favorites {
  private readonly favoriteService = inject(FavoriteService);

  protected readonly favorites = signal<FavoriteDto[]>([]);
  protected readonly loading = signal(true);

  constructor() {
    this.favoriteService.getAll().subscribe({
      next: (f) => {
        this.favorites.set(f);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  protected remove(tmdbMovieId: number): void {
    this.favoriteService.remove(tmdbMovieId).subscribe(() => {
      this.favorites.update((list) => list.filter((f) => f.tmdbMovieId !== tmdbMovieId));
    });
  }

  protected posterUrl(path: string | null): string | null {
    return path ? `${environment.tmdbImageBaseUrl}/w500${path}` : null;
  }
}
