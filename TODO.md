# BeWare Movies — Demo İskelet TODO

Bu liste, [Project_Details.md](Project_Details.md) mimarisine göre demo iskeletini kurmak için yapılacakları içerir.

## 0. Ön Hazırlık
- [x] TMDB hesabı aç, API key (v4 token) al
- [x] .NET SDK (8/9), Node.js + Angular CLI, Docker Desktop kurulu olduğunu doğrula
- [x] Kök `.gitignore` ekle (`bin/`, `obj/`, `node_modules/`, `.env`, `appsettings.*.local.json`) Not: Proje ilerledikçe, .gitignore geliştirilecek. Şu anda taslak olarak kuruldu.
- [x] Repo klasör yapısını belirle: `/backend`, `/frontend`, `/docker`, kökte `docker-compose.yml`

## 1. Backend — ASP.NET Core Web API (4 katmanlı)

> Bağımlılık yönü: **API → Application → Domain**. Infrastructure, Application'daki arayüzleri uygular.

- [x] Solution oluştur (`dotnet new sln`) ve 4 proje ekle: `Api`, `Application`, `Domain`, `Infrastructure`
- [x] Proje referanslarını bağla: `Api → Application`, `Application → Domain`, `Infrastructure → Application` (+ `Domain`)

### 1a. Domain (Varlıklar)
- [x] `Favorite` entity'si (dış bağımlılığı olmayan saf katman)

### 1b. Application (İş Mantığı)
- [x] Arayüz sözleşmeleri: `ITmdbClient`, `IFavoriteRepository`, `IMovieCache` (Infrastructure bunları uygular)
- [x] Servisler: `MovieService` (list/search/detail), `FavoriteService` (ekle/çıkar/listele)
- [x] DTO'lar + mapping, iş kuralları

### 1c. Infrastructure (Veri ve Dış Servisler)
- [x] TMDB `HttpClient` servisi (typed client, `tr-TR`, API key appsettings/secret'ten) — popüler, trend, arama, detay
- [x] Redis cache (`IDistributedCache`), TMDB cevaplarını cache'le (TTL belirle)
- [x] PostgreSQL + EF Core: `DbContext`, `FavoriteRepository`, migration

### 1d. API (Sunum)
- [x] Controller'lar: `MoviesController` (list/search/detail), `FavoritesController` (ekle/çıkar/listele — cihaz bazlı `deviceId`)
- [x] DI kayıtları (servis + arayüz eşleştirmeleri), global exception handling, CORS (Angular origin), health check
- [x] Swagger/OpenAPI aktif et
- [?] `appsettings.json` + örnek `appsettings.Development.json` / secrets şablonu

## 2. Frontend — Angular + Tailwind
- [ ] `ng new frontend` (routing + SCSS/CSS)
- [ ] Tailwind kur ve yapılandır (koyu tema varsayılan)
- [ ] Ortam dosyası: backend API base URL (`environment.ts`)
- [ ] `MovieService` (yalnızca kendi backend'e konuşur — TMDB'ye doğrudan **değil**)
- [ ] Sayfalar/route'lar: Ana sayfa (popüler + trend grid), Arama sonuçları, Film detay, Favoriler
- [ ] Bileşenler: poster kartı, film grid, arama çubuğu, skeleton loader, boş/hata durumu
- [ ] Favori state yönetimi (cihaz bazlı `deviceId`'yi localStorage'da tut)
- [ ] Responsive layout (mobil + masaüstü)

## 3. Altyapı & Çalıştırma
- [ ] `docker-compose.yml`: `postgres`, `redis`, `backend`, (opsiyonel) `frontend` servisleri + volume'ler
- [ ] Backend + frontend için `Dockerfile`
- [ ] `.env` şablonu (`.env.example`) — TMDB key, DB şifresi, connection string'ler
- [ ] Migration'ları başlangıçta uygulama (auto-migrate veya init script)

## 4. CI/CD & Dokümantasyon
- [ ] GitHub Actions workflow: backend build/test + frontend build/lint
- [ ] `README.md`: kurulum, `docker compose up`, env değişkenleri
- [ ] Project_Details.md'deki MVP checkbox'larını ilerledikçe işaretle
