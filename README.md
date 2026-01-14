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
```

## Database (SQLite)

This API uses **SQLite** and applies migrations automatically on startup (`db.Database.Migrate()`).

**Database file path** (from `appsettings.json`):
- `./data/referrals.db`

> Tip: If you delete `./data/referrals.db`, it will be recreated on the next run.

### Run tests
```bash
dotnet test
```

## Design decisions

- **Validators return `ValidationResult` (not exceptions):** Business rule failures (expired invite, cooldown not met, already redeemed, etc.) are *expected outcomes*, not system faults. Returning `ValidationResult` keeps flow explicit, avoids try/catch for control flow, and makes rules easy to unit test.

- **Resolve is public, Redeem/Create are authenticated:** `Resolve` is called during onboarding *before* a user signs in, so it must be accessible to determine the next screen (auth gate). `Create` and `Redeem` mutate state and/or depend on the current user, so they require authentication to prevent abuse and ensure correct ownership.

- **Split into use cases (Create / Resolve / Redeem):** Each operation has different dependencies and rules. Separating them reduces constructor bloat, improves SRP, makes each service easier to understand, and keeps unit tests focused and fast.

- **SQLite + auto-migrations on startup:** Reviewers can run the API on macOS without installing external databases. The app applies migrations automatically (`db.Database.Migrate()`), so `dotnet run` is enough to get a working environment.

- **Error codes mapped to HTTP status:** Responses include stable, machine-readable error codes (e.g., `InviteNotFound`, `InviteExpired`, `InviteAlreadyRedeemed`) while HTTP status communicates category: `404` not found, `409` conflict, `400` invalid input, `429` rate/limit, `500` unexpected errors handled by global middleware.

## Fake users (for local testing)

This take-home uses a fake user profile provider with a hardcoded in-memory user list.  
To simulate an authenticated user, send the header:

- `X-Debug-UserId: <email>`

Available users:

- `oburgosgo@gmail.com` — ReferralCode: `ZaH234` — Name: `Oscar Burgos`
- `hazel.rojasgmail.com` — ReferralCode: `GYU740` — Name: `Hazel Rojas`
- `tavo@gmail.com` — ReferralCode: `BVP652` — Name: `Tavo Pereira`
- `blalopez@gmail.com` — ReferralCode: `XOP823` — Name: `Bladimir Lopez`
- `chapin@gmail.com` — ReferralCode: `HaU654` — Name: `Ronald Oliveros`

> Note: Any email value will authenticate through the fake auth handler, but only the users above exist in the fake profile provider.

## API specification (Swagger)

This project includes an OpenAPI/Swagger specification.

After running the API, open Swagger UI at:

- `/swagger`

Example:
- `https://localhost:<port>/swagger`

Swagger contains the full API contract (endpoints, request/response models, auth header, and example payloads).

### Troubleshooting (macOS)
If HTTPS certificates cause issues, you can trust the dev certs:

```bash
dotnet dev-certs https --trust

