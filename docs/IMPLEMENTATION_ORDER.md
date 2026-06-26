# IMPLEMENTATION_ORDER.md

# Affiliate Cashback Platform MVP

## Purpose

This document defines the official implementation sequence for the project.

All development activities must follow this order unless explicitly approved.

The primary goal is:

```text
Generate Affiliate Link
→ User Purchases Product
→ Accesstrade Sends Webhook
→ Order Created
→ Cashback Recorded
→ User Withdraws Money
```

Any feature that does not directly contribute to this flow should be implemented later.

---

# PHASE 0 - Project Foundation

Goal:

Establish project architecture and infrastructure.

Expected Outcome:

Project builds successfully.

---

## Backend Setup

Tasks

* Create ASP.NET Core Web API
* Setup Clean Architecture
* Setup CQRS
* Setup MediatR
* Setup FluentValidation
* Setup AutoMapper

Done Criteria

* Application runs successfully
* Clean Architecture folders created

---

## Database Setup

Tasks

* Configure PostgreSQL
* Configure EF Core
* Configure DbContext
* Create Initial Migration

Done Criteria

* Database migration successful

---

## Frontend Setup

Tasks

* Create Next.js App
* Configure TypeScript
* Configure TailwindCSS
* Configure Shadcn UI
* Configure Axios
* Configure React Query

Done Criteria

* Frontend starts successfully

---

# PHASE 1 - Authentication

Goal:

Allow users to login.

Expected Outcome:

Users can authenticate and receive JWT tokens.

---

## User Entity

Tasks

* Create User Entity
* Create User Repository

---

## Google OAuth

Tasks

* Configure Google OAuth
* Create Login Endpoint
* Generate JWT
* Generate Refresh Token

---

## Frontend Authentication

Tasks

* Login Page
* Google Login Button
* Auth Context
* Route Protection

---

Done Criteria

✓ User can login

✓ JWT generated

✓ Protected routes working

---

# PHASE 2 - Affiliate Link Generation

Goal:

Generate affiliate links from Accesstrade.

Expected Outcome:

Users can paste Shopee URLs and receive affiliate links.

---

## Affiliate Infrastructure

Tasks

* Create IAffiliateProvider
* Create AccesstradeProvider
* Configure API Client

---

## Affiliate Domain

Tasks

* AffiliateLink Entity
* AffiliateLink Repository

---

## Generate Link

Tasks

* GenerateAffiliateLinkCommand
* URL Validation
* Sub1 Generation
* Save Link History

---

## Frontend

Tasks

* Link Converter Page
* Copy Link Functionality
* Link History Page

---

Done Criteria

✓ User pastes Shopee URL

✓ Affiliate link generated

✓ Link saved in database

---

# PHASE 3 - Webhook Foundation

Goal:

Receive conversion data from Accesstrade.

Expected Outcome:

Webhook events stored successfully.

---

## Webhook Infrastructure

Tasks

* Webhook Endpoint
* DTO Mapping
* Signature Validation

---

## Webhook Storage

Tasks

* WebhookEvents Entity
* Event Repository
* Event Persistence

---

## Idempotency

Tasks

* Duplicate Detection
* Event Status Tracking

---

Done Criteria

✓ Webhook received

✓ Event stored

✓ Duplicate events ignored

---

# PHASE 4 - Orders & Cashback

Goal:

Convert webhook events into orders.

Expected Outcome:

Cashback appears in user account.

---

## Order Domain

Tasks

* Order Entity
* Order Repository
* Order Status Logic

---

## Cashback Logic

Tasks

* Cashback Service
* Cashback Configuration
* Commission Calculation

---

## Order Processing

Tasks

* Create Order From Webhook
* Update Existing Orders
* Status Transitions

---

Done Criteria

✓ Order created

✓ Cashback calculated

✓ User balance updated

---

# PHASE 5 - Dashboard

Goal:

Allow users to track earnings.

Expected Outcome:

Users can see balances and orders.

---

## Dashboard API

Tasks

* Summary Query
* Dashboard Endpoint

---

## Dashboard UI

Tasks

* Summary Cards
* Recent Orders
* Earnings Statistics

---

Done Criteria

✓ Dashboard displays accurate data

---

# PHASE 6 - Withdrawals

Goal:

Allow users to withdraw cashback.

Expected Outcome:

Users can submit withdrawal requests.

---

## Withdrawal Domain

Tasks

* Withdrawal Entity
* Transaction Entity

---

## Withdrawal Logic

Tasks

* Create Request
* Balance Validation
* Status Changes

---

## Withdrawal UI

Tasks

* Withdrawal Form
* Withdrawal History

---

Done Criteria

✓ User can request withdrawal

✓ Balance locked correctly

---

# PHASE 7 - Admin Panel

Goal:

Allow platform management.

Expected Outcome:

Admin controls operational workflows.

---

## Admin Dashboard

Tasks

* Revenue Statistics
* Order Statistics
* Withdrawal Statistics

---

## User Management

Tasks

* User List
* User Detail
* User Suspension

---

## Withdrawal Management

Tasks

* Approve Withdrawal
* Reject Withdrawal
* Complete Withdrawal

---

Done Criteria

✓ Admin can operate platform

---

# PHASE 8 - Notifications

Goal:

Improve user experience.

Expected Outcome:

Users receive system notifications.

---

Tasks

* Notification Entity
* Notification APIs
* Notification Bell
* Notification Page

---

Done Criteria

✓ Notifications displayed correctly

---

# PHASE 9 - Background Jobs

Goal:

Improve reliability.

Expected Outcome:

Async processing operational.

---

Tasks

* Install Hangfire
* Retry Failed Webhooks
* Sync Orders
* Cleanup Tasks

---

Done Criteria

✓ Jobs execute successfully

✓ Retry mechanism works

---

# PHASE 10 - Security Hardening

Goal:

Prepare for production.

Expected Outcome:

Security baseline achieved.

---

Tasks

* Ownership Validation
* Rate Limiting
* Audit Logs
* Secret Management

---

Done Criteria

✓ Security review passed

---

# PHASE 11 - Observability

Goal:

Improve monitoring.

Expected Outcome:

System health visible.

---

Tasks

* Serilog
* Correlation Id
* Health Checks
* Slow Request Logging

---

Done Criteria

✓ Logs available

✓ Monitoring operational

---

# PHASE 12 - Production Deployment

Goal:

Launch MVP.

Expected Outcome:

System accessible by real users.

---

Tasks

* Docker
* CI/CD
* Domain
* SSL
* Backup
* Monitoring

---

Done Criteria

✓ Production environment deployed

✓ Backups operational

✓ Monitoring operational

---

# STRICT DEVELOPMENT RULES

Do NOT implement:

* Referral Program
* Multi-level Cashback
* Multi-provider Support
* Push Notifications
* Mobile Apps
* Telegram Integration
* AI Features

until MVP is fully completed.

---

# MVP COMPLETION DEFINITION

The MVP is complete when:

1. User logs in via Google
2. User generates affiliate link
3. User purchases product
4. Accesstrade sends webhook
5. System creates order
6. Cashback is recorded
7. User requests withdrawal
8. Admin approves withdrawal
9. User receives payment

If any step above is missing, MVP is not complete.

Focus on revenue-generating flow first.
