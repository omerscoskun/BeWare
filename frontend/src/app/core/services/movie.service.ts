import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MovieDetail, MovieSummary } from '../models/movie.model';

@Injectable({ providedIn: 'root' })
export class MovieService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/movies`;

  getPopular(page = 1): Observable<MovieSummary[]> {
    return this.http.get<MovieSummary[]>(`${this.baseUrl}/popular`, { params: { page } });
  }

  getTrending(): Observable<MovieSummary[]> {
    return this.http.get<MovieSummary[]>(`${this.baseUrl}/trending`);
  }

  search(query: string, page = 1): Observable<MovieSummary[]> {
    return this.http.get<MovieSummary[]>(`${this.baseUrl}/search`, { params: { query, page } });
  }

  getDetail(id: number): Observable<MovieDetail> {
    return this.http.get<MovieDetail>(`${this.baseUrl}/${id}`);
  }
}
