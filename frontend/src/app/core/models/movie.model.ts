export interface MovieSummary {
  tmdbId: number;
  title: string;
  posterPath: string | null;
  voteAverage: number;
  releaseDate: string | null;
}

export interface MovieDetail {
  tmdbId: number;
  title: string;
  overview: string | null;
  posterPath: string | null;
  backdropPath: string | null;
  voteAverage: number;
  releaseDate: string | null;
  runtime: number | null;
  genres: string[];
  cast: string[];
  trailerYoutubeKey: string | null;
}

export interface FavoriteDto {
  tmdbMovieId: number;
  title: string;
  posterPath: string | null;
  voteAverage: number;
  createdAt: string;
}

export interface AddFavoriteRequest {
  tmdbMovieId: number;
  title: string;
  posterPath: string | null;
  voteAverage: number;
}
