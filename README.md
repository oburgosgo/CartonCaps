# CartonCaps.Referrals API

Referral invites service that supports:
- **Create invite** (generates a deeplink + channel-specific share content)
- **Resolve invite** (used by the mobile app to decide the next onboarding screen)
- **Redeem invite** (new user redeems an invite)
- **Query invites** (by user / by id)
- **Get referral code** (for current user)

---

## Tech stack
- ASP.NET Core Web API
- EF Core + SQLite
- AutoMapper
- xUnit + Moq (+ FluentAssertions)
- Clean architecture style: `Api / Application / Domain / Infrastructure`

---

## Quick start (macOS / Linux / Windows)

### Prerequisites
- .NET SDK (the version used by this repo)

### Run API
From the repo root:

```bash
dotnet restore
dotnet build
dotnet run --project src/CartonCaps.Referrals.Api

## Database (SQLite)

This API uses **SQLite** and applies migrations automatically on startup (`db.Database.Migrate()`).

**Database file path** (from `appsettings.json`):
- `./data/referrals.db`

> Tip: If you delete `./data/referrals.db`, it will be recreated on the next run.

### Run tests
```bash
dotnet test