# Film Keşif Uygulaması — MAPEG Staj Projesi

Tek günde geliştirilen, TMDB verisiyle çalışan responsive bir film keşif sitesi.

## Proje Nedir?

- Popüler ve trend filmleri listeleyen, arama ve detay inceleme sunan bir web uygulaması.
- Mobil ve masaüstünde sorunsuz çalışan responsive arayüz.
- Film verileri TMDB (The Movie Database) API'sinden canlı olarak çekilir.
- Kullanıcı beğendiği filmleri favorilerine ekleyip listesini yönetebilir.

## Temel Fikir

- Angular arayüzü doğrudan TMDB ile değil, yalnızca kendi C# backend'imizle konuşur.
- Backend, TMDB isteklerini tek noktadan yönetir: API anahtarı gizli kalır, cevaplar Redis'te önbelleklenir (hız + istek limiti koruması).
- Favoriler PostgreSQL'de saklanır; projenin kalıcı veri katmanını oluşturur.
- Kod tabanı SOLID, DRY ve KISS prensiplerine uygun, 4 katmanlı bir mimaride ilerler.

## Özellikler (MVP)

- [ ] Ana sayfa: popüler + trend filmler (poster grid)
- [ ] Film arama
- [ ] Film detay sayfası (özet, puan, oyuncular, fragman)
- [ ] Favorilere ekleme / çıkarma
- [ ] Türkçe içerik desteği (TMDB `tr-TR`)
- [ ] Koyu tema, yüklenme (skeleton) animasyonları, hata ve boş durum ekranları

## Teknoloji Yığını

| Katman             | Teknoloji                           |
| ------------------ | ----------------------------------- |
| Frontend           | Angular + TypeScript + Tailwind CSS |
| Backend            | C# / ASP.NET Core Web API           |
| Veri kaynağı       | TMDB API                            |
| Önbellek           | Redis                               |
| Veritabanı         | PostgreSQL                          |
| API dokümantasyonu | Swagger (OpenAPI)                   |
| CI/CD              | GitHub Actions                      |
| Ortam              | Docker Compose, VS Code             |

## Mimari Akış

```
Angular  →  ASP.NET Core API  →  Redis (cache)  →  TMDB API
                    ↘
                     PostgreSQL (favoriler)
```

## Katmanlı Mimari (4 Katman)

1. **API (Sunum)** — HTTP uçları, istek/yanıt modelleri, Swagger, CORS.
2. **Application (İş Mantığı)** — Servisler, iş kuralları, arayüz sözleşmeleri, DTO'lar.
3. **Domain (Varlıklar)** — Çekirdek varlıklar (ör. Favorite); dış bağımlılığı olmayan saf katman.
4. **Infrastructure (Veri ve Dış Servisler)** — TMDB istemcisi, Redis önbelleği, EF Core + PostgreSQL.

> Bağımlılık yönü: API → Application → Domain. Infrastructure, Application'daki arayüzleri uygular.

## Bilinçli Tercihler (1 günlük kapsam)

- Mikroservis yerine 4 katmanlı modüler monolit — tek günde teslim garantisi.
- RabbitMQ yok — projede asenkron event ihtiyacı bulunmuyor.
- Kullanıcı girişi (auth) yok — favoriler cihaz bazlı tutulur.
- Jenkins yerine GitHub Actions — dakikalar içinde kurulan CI.