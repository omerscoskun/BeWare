import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AddFavoriteRequest, FavoriteDto } from '../models/movie.model';

@Injectable({ providedIn: 'root' })
export class FavoriteService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/favorites`;

  getAll(): Observable<FavoriteDto[]> {
    return this.http.get<FavoriteDto[]>(this.baseUrl);
  }

  add(request: AddFavoriteRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, request);
  }

  remove(tmdbMovieId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${tmdbMovieId}`);
  }
}
