# Coding Standards

Version: 1.0

Purpose

Define coding conventions and development rules for the Cashback Affiliate Platform.

These standards must be followed by:

- Developers
- AI Coding Assistants
- Cursor
- Future Contributors

---

# 1. General Principles

## Rule 1

Code must be:

- Readable
- Predictable
- Testable
- Maintainable

Do not optimize prematurely.

---

## Rule 2

Prefer clarity over cleverness.

Good:

```csharp
var user = await userRepository.GetByIdAsync(id);
```

Bad:

```csharp
var u = await repo.Get(id);
```

---

## Rule 3

Every feature must follow existing patterns.

Never introduce a new pattern if an existing one already solves the problem.

---

# 2. Language Standards

Backend

- C# 13
- .NET 9

Frontend

- TypeScript

Do not use JavaScript.

---

# 2.1 XML Documentation (Backend)

All backend classes, interface methods, and properties must use XML documentation with `<summary>` only.

Rules:

- Document classes and interfaces with `<summary>`
- Document interface methods with `<summary>` on the interface
- Use `/// <inheritdoc/>` on implementation methods that fulfill an interface or override a base member
- Document private methods with their own `<summary>`
- Do not use `<param>`, `<returns>`, `<remarks>`, or `<exception>`

`GenerateDocumentationFile` is enabled for all backend projects.

---

# 3. Naming Conventions

## Classes

PascalCase

Good

```csharp
UserService
GenerateAffiliateLinkCommand
```

Bad

```csharp
userService
generateAffiliateLink
```

---

## Interfaces

Prefix with I

Good

```csharp
IUserRepository
IAccesstradeClient
```

---

## Methods

PascalCase

Good

```csharp
GetUserByIdAsync()
```

---

## Variables

camelCase

Good

```csharp
userId
availableBalance
```

---

## Constants

PascalCase

Good

```csharp
MinimumWithdrawalAmount
```

---

# 4. Async Rules

All asynchronous methods must end with Async.

Good

```csharp
GetByIdAsync()
CreateOrderAsync()
```

Bad

```csharp
GetById()
```

when method is async.

---

# 5. Clean Architecture Rules

Dependencies must point inward.

Allowed

Api
→ Application
→ Domain

Infrastructure
→ Application
→ Domain

Persistence
→ Domain

---

Forbidden

Domain
→ Infrastructure

Domain
→ Persistence

Application
→ Api

---

# 6. Controller Rules

Controllers must be thin.

Controllers may:

- Receive request
- Validate model
- Call MediatR
- Return response

Controllers must not:

- Query database
- Execute business logic
- Call external APIs

Bad

```csharp
public async Task<IActionResult> Create()
{
    var user = await _db.Users.FirstAsync();

    user.Balance += 1000;

    await _db.SaveChangesAsync();
}
```

---

# 7. Application Layer Rules

Business logic belongs here.

Allowed

Commands

Queries

Validators

Handlers

Use Cases

---

Example

GenerateAffiliateLinkHandler

CreateWithdrawalHandler

ApproveWithdrawalHandler

---

# 8. Domain Layer Rules

Domain must contain:

- Entities
- Enums
- Value Objects
- Domain Rules

Domain must not contain:

- EF Core
- HTTP Clients
- Logging
- External Services

---

# 9. Entity Rules

Entities represent business concepts.

Example

User

Order

WithdrawRequest

AffiliateLink

---

Entities should not expose public setters unnecessarily.

Prefer:

```csharp
public decimal AvailableBalance
{
    get;
    private set;
}
```

---

# 10. DTO Rules

Never expose entities directly.

Good

```csharp
UserDto
OrderDto
DashboardDto
```

Bad

```csharp
return user;
```

---

# 11. Validation Rules

Use FluentValidation.

Validation must be placed in Application Layer.

Example

CreateWithdrawalValidator

GenerateAffiliateLinkValidator

---

Do not validate business rules in controllers.

---

# 12. Exception Handling

Use Global Exception Middleware.

Do not use:

```csharp
try
{
}
catch
{
}
```

inside every controller.

---

Allowed Exceptions

ValidationException

NotFoundException

BusinessRuleException

ForbiddenException

---

# 13. Logging Rules

Use Serilog.

Log:

- Errors
- Important business events
- External API failures

Do not log:

- Passwords
- Tokens
- Secrets

---

# 14. Database Rules

Use EF Core Fluent API.

Avoid DataAnnotations when possible.

Preferred

```csharp
builder.Property(x => x.Email)
    .HasMaxLength(255)
    .IsRequired();
```

---

All migrations must be committed.

Never modify applied migrations.

Create a new migration instead.

---

# 15. Repository Rules

Repositories only handle persistence.

Good

```csharp
IUserRepository
IOrderRepository
```

Repositories must not contain:

- Business logic
- HTTP calls

---

# 16. API Design Rules

Use REST.

Good

GET

POST

PUT

DELETE

---

Bad

```text
POST /GetOrders
POST /DeleteOrder
```

---

Good

```text
GET /orders
DELETE /orders/{id}
```

---

# 17. Response Format

All APIs must use standard response wrapper.

Success

{
"success": true,
"data": {}
}

Failure

{
"success": false,
"message": "Validation failed"
}

---

# 18. Frontend Standards

Language

TypeScript only.

---

Components

PascalCase

Good

```tsx
DashboardCard.tsx
WithdrawForm.tsx
```

---

Hooks

Prefix with use

Good

```tsx
useDashboard()
useOrders()
```

---

# 19. React Rules

Use:

- React Query
- React Hook Form
- Zod

Avoid:

- Redux (MVP)
- Context abuse

---

# 20. Styling Rules

Use:

- TailwindCSS
- Shadcn/UI

Avoid:

- Inline styles

Bad

```tsx
<div style={{ marginTop: 20 }}>
```

---

# 21. Security Rules

Never trust client input.

Validate everything server-side.

Never expose:

- API Keys
- Secrets
- Tokens

Use environment variables.

---

# 22. Testing Rules

Minimum Coverage

Critical Business Logic Only

Must Test

- Cashback Calculation
- Withdrawal Validation
- Order Processing
- Commission Processing

---

# 23. Git Rules

Branch Naming

feature/generate-affiliate-link

feature/withdrawal-module

bugfix/order-processing

---

Commit Format

feat:

fix:

refactor:

docs:

test:

Example

feat: add affiliate link generation

---

# 24. AI Coding Rules

Cursor must:

✓ Follow existing patterns

✓ Reuse existing abstractions

✓ Create DTOs

✓ Create validators

✓ Create tests when requested

Cursor must not:

✗ Put business logic in controllers

✗ Expose entities directly

✗ Duplicate code

✗ Create new architecture patterns

✗ Bypass validation

---

# 25. Definition of Good Code

Good code is:

- Easy to read
- Easy to change
- Easy to test
- Consistent with existing patterns

The simplest correct solution is preferred.
