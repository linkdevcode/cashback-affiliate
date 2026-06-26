# Technical Architecture

Version: 1.0

Project Name

Cashback Affiliate Platform

---

# 1. Architecture Goals

The system must be:

- Simple for MVP
- Easy to maintain
- Easy to scale
- Easy for AI-assisted development
- Cloud ready
- Testable

---

# 2. Architecture Style

Primary Architecture

Clean Architecture

Application Structure

Modular Monolith

Feature Organization

Vertical Slice Architecture

Communication

REST API

Authentication

JWT + Google OAuth

Database

PostgreSQL

Caching

Redis (Phase 2)

Background Jobs

Hangfire (Phase 2)

---

# 3. High Level Architecture

┌─────────────────────┐
│ React Frontend      │
└──────────┬──────────┘
│
▼
┌─────────────────────┐
│ ASP.NET Core API    │
└──────────┬──────────┘
│
▼
┌─────────────────────┐
│ Application Layer   │
└──────────┬──────────┘
│
▼
┌─────────────────────┐
│ Domain Layer        │
└──────────┬──────────┘
│
▼
┌─────────────────────┐
│ Infrastructure      │
└──────────┬──────────┘
│
▼
┌─────────────────────┐
│ PostgreSQL          │
└─────────────────────┘

External Systems

- Google OAuth
- Accesstrade API
- Accesstrade Postback

---

# 4. Solution Structure

src/

├── Cashback.Api
├── Cashback.Application
├── Cashback.Domain
├── Cashback.Infrastructure
├── Cashback.Persistence
├── Cashback.Shared
└── Cashback.Contracts

tests/

├── Cashback.UnitTests
└── Cashback.IntegrationTests

docs/

├── product-requirements.md
├── database-design.md
├── enums.md
├── user-flows.md
├── api-specification.md
└── tech-architecture.md

---

# 5. Project Responsibilities

## Cashback.Api

Responsibilities

- Controllers
- Authentication
- Middleware
- Dependency Injection
- Swagger

Must Not Contain

- Business Logic
- Database Logic

---

## Cashback.Application

Responsibilities

- Use Cases
- Commands
- Queries
- Validators
- DTOs

Patterns

- CQRS
- MediatR

Example

GenerateAffiliateLinkCommand

CreateWithdrawalRequestCommand

GetDashboardQuery

---

## Cashback.Domain

Responsibilities

- Entities
- Enums
- Domain Rules
- Value Objects

Contains

User

Order

AffiliateLink

WithdrawRequest

CommissionTransaction

No external dependencies allowed.

---

## Cashback.Infrastructure

Responsibilities

- External Services
- Accesstrade API
- Google OAuth
- Email Service (Future)

Contains

AccesstradeClient

GoogleAuthService

---

## Cashback.Persistence

Responsibilities

- EF Core
- DbContext
- Repositories
- Migrations

Contains

ApplicationDbContext

EntityConfigurations

---

## Cashback.Shared

Responsibilities

- Common Helpers
- Constants
- Extensions
- Base Classes

Contains

Result<T>

PagedResult<T>

BaseEntity

---

## Cashback.Contracts

Responsibilities

Shared contracts between frontend and backend.

Contains

Request DTOs

Response DTOs

Enums

---

# 6. Vertical Slice Structure

Inside Application Layer

Features/

├── Auth
├── Dashboard
├── AffiliateLinks
├── Orders
├── Withdrawals
├── Admin
└── Webhooks

Example

Features/

└── AffiliateLinks

```
├── Commands
│
├── Queries
│
├── Validators
│
├── DTOs
│
└── Handlers
```

Example

AffiliateLinks/

├── GenerateAffiliateLink
│
├── GetAffiliateLinks
│
└── DeleteAffiliateLink

---

# 7. Domain Entities

Core Entities

User

AffiliateLink

Order

WithdrawRequest

CommissionTransaction

AuditLog

Relationships

User

→ AffiliateLinks

→ Orders

→ WithdrawRequests

Order

→ CommissionTransactions

---

# 8. Authentication Architecture

Login Flow

React

↓

Google OAuth

↓

Backend Verification

↓

Generate JWT

↓

Return Token

Token Lifetime

Access Token

15 minutes

Refresh Token

7 days

---

# 9. Integration Architecture

## Google OAuth

Responsibilities

- Verify Google Token
- Retrieve Profile

Used By

Auth Module

---

## Accesstrade API

Responsibilities

- Generate Affiliate Links
- Get Campaigns
- Sync Orders

Used By

Affiliate Module

Order Module

---

## Accesstrade Postback

Responsibilities

- Receive Commission Events
- Update Orders
- Update Balances

Used By

Webhook Module

---

# 10. Database Strategy

ORM

Entity Framework Core

Database

PostgreSQL

Migration

EF Core Migration

Naming

snake_case

Example

users

affiliate_links

orders

withdraw_requests

---

# 11. Error Handling

Global Exception Middleware

Response Format

{
"success": false,
"message": "Unexpected error"
}

Custom Exceptions

ValidationException

NotFoundException

ForbiddenException

BusinessRuleException

---

# 12. Logging

Library

Serilog

Outputs

- Console
- File

Future

- Seq
- Elasticsearch

Log Levels

Information

Warning

Error

Critical

---

# 13. API Documentation

Swagger OpenAPI

Enabled

Development

Staging

Disabled

Production

---

# 14. Frontend Architecture

Framework

React

Language

TypeScript

Build Tool

Vite

UI Library

Shadcn/UI

Styling

TailwindCSS

State Management

TanStack Query

Forms

React Hook Form

Validation

Zod

Routing

React Router

---

# 15. Deployment Architecture

Frontend

Vercel

Backend

Railway / Render / VPS

Database

Neon PostgreSQL

Object Storage

Cloudflare R2 (Future)

---

# 16. Security

HTTPS Only

JWT Authentication

Google OAuth Verification

Input Validation

Rate Limiting

Secret Management

Environment Variables

Never commit:

- API Keys
- OAuth Secrets
- Database Passwords

---

# 17. Future Scalability

Phase 2

- Redis Cache
- Hangfire Jobs
- Notification System
- Referral System

Phase 3

- Multi Tenant
- Mobile App
- Public API

Phase 4

- Service Extraction

Auth Service

Affiliate Service

Payment Service

Notification Service

---

# 18. MVP Definition

Required Modules

✓ Auth

✓ Dashboard

✓ Affiliate Links

✓ Orders

✓ Withdrawals

✓ Admin

✓ Webhooks

Excluded

✗ Referral

✗ Notification

✗ Mobile App

✗ AI Features

✗ Multi Tenant

The architecture must prioritize shipping the MVP quickly while keeping the codebase scalable for future growth.
