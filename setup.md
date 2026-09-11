# DB Baðlantý Stratejisi — SkyAirportAPI

Projede 3 farklý senaryoya göre 3 ayrý appsettings dosyasý var. Hangisinin ne zaman
devreye girdiðini karýþtýrmamak için bu not var.

## Dosyalar ve ne zaman kullanýlýrlar

| Dosya | Ne zaman yüklenir | Port | Host |
|---|---|---|---|
| `appsettings.json` | Native local çalýþtýrma (varsayýlan, ekstra bir þey yapmadan) | 5432 | localhost |
| `appsettings.Docker.json` | API **container içinde** çalýþýrken (`ASPNETCORE_ENVIRONMENT=Docker`), `docker-compose up` ile otomatik | 5432 (internal) | postgres (container adý) |
| `appsettings.DockerHost.json` | Host'tan (senin makinenden) Docker'daki Postgres'e eriþmek için — migration atarken, ya da hibrit debug'da | 5433 | localhost |

> **Neden 5433?** Native Postgres zaten 5432'de dinlediði için, Docker container'ýnýn
> host'a açtýðý port çakýþmasýn diye 5433'e maplendi (docker-compose.yml'da `5433:5432`).

## Senaryo 1 — Native / Local çalýþtýrma

```
Visual Studio: üst dropdown'dan "https" seç ? F5
```
`appsettings.json` kullanýlýr, native Postgres'e (5432) baðlanýr.

## Senaryo 2 — Docker'da tam çalýþtýrma (API + DB container'da)

```cmd
docker-compose up -d --build
cd API
dotnet ef database update --project DataAccess --startup-project API -- --environment DockerHost
```
- `docker-compose up`, API'yi `appsettings.Docker.json` ile (internal 5432) baþlatýr.
- Migration'ý host'tan `DockerHost` environment'ý üzerinden (5433) atýyorsun —
  ayný fiziksel veriye yazýyor, container içindeki API de bunu görür.
- Ýlk kurulumda ve her yeni migration eklediðinde bu `dotnet ef` komutunu tekrar çalýþtýr.

## Senaryo 3 — Hibrit: API host'ta (debug), DB Docker'da

```cmd
docker-compose up -d postgres
```
Visual Studio: üst dropdown'dan **"DockerDB-Local"** profilini seç ? F5
(ya da terminal: `dotnet run --launch-profile DockerDB-Local`)

`appsettings.DockerHost.json` kullanýlýr (5433). Nadiren gerekir — genelde container
içindeki bir bug'ý host'tan breakpoint koyarak debug etmek istediðinde.

## Önemli notlar

- `dotnet ef` komutlarý `launchSettings.json`'daki profilleri **okumaz**. Migration
  atarken her zaman `-- --environment <isim>` ile açýkça belirtmen gerekir.
- Yeni bir PC'ye geçtiðinde: Docker Desktop kur, repo'yu klonla, `dotnet-ef` tool'unu
  kur (`dotnet tool install --global dotnet-ef`), sonra Senaryo 2'deki adýmlarý izle.
- `appsettings.DockerHost.json`, `Password` gibi bilgiler içerdiði için normalde
  git'e commitlenmemesi (ya da en azýndan gerçek prod þifreleriyle karýþtýrýlmamasý)
  gereken bir dosya — þu an local dev þifresi (123456) olduðu için risksiz, ama
  ileride gerçek bir ortama taþýnýrsa User Secrets / environment variable'a geçmek
  daha doðru olur.