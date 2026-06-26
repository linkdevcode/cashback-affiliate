# Folder Structure

Version: 1.0

Purpose

Define the official project structure for the Cashback Affiliate Platform.

All developers and AI assistants must follow this structure.

---

# 1. Repository Structure

root/

├── backend/
├── frontend/
├── tests/
├── docs/
├── scripts/
├── .cursor/
├── .github/
├── docker/
├── README.md
├── .gitignore
└── docker-compose.yml

---

# 2. Backend Structure

backend/

├── src/
│
├── tests/
│
├── Cashback.sln
│
└── Directory.Build.props

---

# 3. Backend Projects

backend/src/

├── Cashback.Api
├── Cashback.Application
├── Cashback.Domain
├── Cashback.Infrastructure
├── Cashback.Persistence
├── Cashback.Contracts
└── Cashback.Shared

---

# 4. Cashback.Api

Purpose

Application entry point.

Contains

- Controllers
- Middleware
- DI Configuration
- Swagger
- Authentication Setup

Structure

Cashback.Api/

├── Controllers/
├── Middleware/
├── Extensions/
├── Configurations/
├── Filters/
├── Program.cs
└── appsettings.json

Must Not Contain

- Business Logic
- EF Queries
- External API Logic

---

# 5. Cashback.Application

Purpose

Application Use Cases.

Structure

Cashback.Application/

├── Features/
├── Behaviors/
├── Interfaces/
├── Validators/
└── DependencyInjection.cs

---

# 6. Features Structure

Each feature follows Vertical Slice Architecture.

Example

Features/

├── Auth/
├── Dashboard/
├── AffiliateLinks/
├── Orders/
├── Withdrawals/
├── Admin/
└── Webhooks/

---

Example

Features/

└── AffiliateLinks/

```
├── Commands/
│
├── Queries/
│
├── DTOs/
│
├── Validators/
│
└── Handlers/
```

---

# 7. Command Structure

Example

GenerateAffiliateLink/

├── GenerateAffiliateLinkCommand.cs
├── GenerateAffiliateLinkHandler.cs
├── GenerateAffiliateLinkValidator.cs
├── GenerateAffiliateLinkRequest.cs
└── GenerateAffiliateLinkResponse.cs

One Use Case = One Folder

---

# 8. Cashback.Domain

Purpose

Core Business Model.

Structure

Cashback.Domain/

├── Entities/
├── Enums/
├── Constants/
├── Exceptions/
├── ValueObjects/
└── Common/

---

Example

Entities/

├── User.cs
├── Order.cs
├── AffiliateLink.cs
├── WithdrawRequest.cs
└── CommissionTransaction.cs

---

# 9. Cashback.Persistence

Purpose

Database Access.

Structure

Cashback.Persistence/

├── Context/
├── Configurations/
├── Repositories/
├── Migrations/
└── DependencyInjection.cs

---

Example

Configurations/

├── UserConfiguration.cs
├── OrderConfiguration.cs
└── AffiliateLinkConfiguration.cs

---

# 10. Cashback.Infrastructure

Purpose

External Services.

Structure

Cashback.Infrastructure/

├── Services/
├── Clients/
├── Providers/
├── Settings/
└── DependencyInjection.cs

---

Example

Clients/

├── AccesstradeClient.cs
└── GoogleOAuthClient.cs

---

# 11. Cashback.Contracts

Purpose

Shared Contracts.

Structure

Cashback.Contracts/

├── Requests/
├── Responses/
├── Enums/
└── Common/

---

Example

Requests/

├── CreateWithdrawalRequest.cs
└── GenerateAffiliateLinkRequest.cs

---

# 12. Cashback.Shared

Purpose

Reusable Utilities.

Structure

Cashback.Shared/

├── Results/
├── Pagination/
├── Extensions/
├── Helpers/
├── Constants/
└── Common/

---

Example

Results/

├── Result.cs
└── PagedResult.cs

---

# 13. Backend Test Structure

backend/tests/

├── Cashback.UnitTests
└── Cashback.IntegrationTests

---

Unit Tests

Cashback.UnitTests/

├── Application/
├── Domain/
└── Shared/

---

Integration Tests

Cashback.IntegrationTests/

├── Api/
├── Database/
└── Webhooks/

---

# 14. Frontend Structure

frontend/

├── public/
├── src/
├── package.json
├── vite.config.ts
└── tsconfig.json

---

# 15. Frontend src Structure

frontend/src/

├── app/
├── features/
├── pages/
├── layouts/
├── components/
├── services/
├── hooks/
├── store/
├── types/
├── utils/
├── routes/
├── assets/
└── styles/

---

# 16. Frontend Features Structure

Feature-based organization.

Example

features/

├── auth/
├── dashboard/
├── affiliate-links/
├── orders/
├── withdrawals/
└── admin/

---

Example

features/orders/

├── api/
├── components/
├── hooks/
├── pages/
├── schemas/
└── types/

---

# 17. Shared Components

components/

├── ui/
├── forms/
├── tables/
├── dialogs/
└── layouts/

---

ui/

Contains Shadcn components only.

---

# 18. Services Structure

services/

├── api-client.ts
├── auth-service.ts
├── dashboard-service.ts
├── order-service.ts
└── withdrawal-service.ts

Responsibilities

HTTP communication only.

No UI logic.

---

# 19. Documentation Structure

docs/

├── product-requirements.md
├── database-design.md
├── enums.md
├── user-flows.md
├── api-specification.md
├── tech-architecture.md
├── coding-standards.md
├── folder-structure.md
├── development-roadmap.md
└── deployment-guide.md

---

# 20. Cursor Structure

.cursor/

├── rules/
└── templates/

---

rules/

Contains .mdc files.

Example

auth-module.mdc

database-rules.mdc

frontend-rules.mdc

api-rules.mdc

---

templates/

Reusable prompt templates.

---

# 21. GitHub Structure

.github/

├── workflows/
└── pull_request_template.md

---

workflows/

ci.yml

build.yml

test.yml

---

# 22. Docker Structure

docker/

├── backend/
├── frontend/
└── postgres/

---

Example

docker/

├── backend/Dockerfile
├── frontend/Dockerfile
└── postgres/init.sql

---

# 23. Naming Rules

Folders

kebab-case

Example

affiliate-links

withdrawals

---

Backend Projects

PascalCase

Example

Cashback.Application

Cashback.Domain

---

Files

PascalCase

Example

User.cs

OrderConfiguration.cs

GenerateAffiliateLinkHandler.cs

---

# 24. Forbidden Structures

Do Not Create

Services/

Repositories/

Helpers/

Utils/

at project root level.

Everything must belong to a specific layer.

---

Bad

src/

├── Services/
├── Helpers/
└── Utils/

---

Good

Cashback.Infrastructure/Services/

Cashback.Shared/Helpers/

---

# 25. Definition of Done

A new feature is correctly implemented when:

✓ Files are placed in correct folders

✓ Follows Vertical Slice structure

✓ Respects Clean Architecture

✓ Includes validation

✓ Includes DTOs

✓ Includes tests when required

✓ Does not violate dependency rules

Folder structure consistency is more important than personal preference.
