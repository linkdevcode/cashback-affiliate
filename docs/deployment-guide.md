# Deployment Guide

Version: 1.0

Project

Cashback Affiliate Platform

Purpose

Define deployment strategy for Development, Staging and Production environments.

---

# 1. Deployment Philosophy

Goals

- Low Cost
- Fast Deployment
- Easy Maintenance
- Suitable for Solo Developer
- Easy to Scale Later

MVP Priority

Shipping > Perfection

---

# 2. Environment Strategy

Three Environments

Development

Local machine.

Staging

Pre-production testing.

Production

Public environment.

---

# 3. Infrastructure Overview

Frontend

Vercel

Backend

Railway

Alternative

- Render
- VPS
- Fly.io

Database

Neon PostgreSQL

Alternative

- Railway PostgreSQL
- Supabase PostgreSQL

Future

Redis

Cloudflare R2

---

# 4. System Architecture

User Browser

↓

Frontend (Vercel)

↓

Backend API (Railway)

↓

PostgreSQL (Neon)

External Services

↓

Google OAuth

Accesstrade API

Accesstrade Webhook

---

# 5. Local Development Setup

Requirements

- .NET 9 SDK
- Node.js LTS
- PostgreSQL
- Git

Optional

- Docker Desktop
- pgAdmin

---

Backend

Run

```bash
dotnet restore
dotnet build
dotnet run
```

Default

```text
https://localhost:5001
```

---

Frontend

Run

```bash
npm install
npm run dev
```

Default

```text
http://localhost:5173
```

---

Database

Apply migrations

```bash
dotnet ef database update
```

---

# 6. Environment Variables

Never hardcode secrets.

All sensitive values must come from environment variables.

---

Backend Variables

Required

```env
ASPNETCORE_ENVIRONMENT=Production

DATABASE_CONNECTION_STRING=

JWT_SECRET=

JWT_ISSUER=

JWT_AUDIENCE=

GOOGLE_CLIENT_ID=

GOOGLE_CLIENT_SECRET=

ACCESSTRADE_TOKEN=

ACCESSTRADE_BASE_URL=

ACCESSTRADE_WEBHOOK_SECRET=
```

---

Frontend Variables

Required

```env
VITE_API_URL=

VITE_GOOGLE_CLIENT_ID=
```

---

# 7. Database Deployment

Provider

Neon PostgreSQL

---

Connection String Example

```text
Host=
Port=5432
Database=
Username=
Password=
SSL Mode=Require
```

---

Migration Deployment

Apply migration

```bash
dotnet ef database update
```

---

Rules

Never manually edit production database.

All schema changes must go through EF migrations.

---

# 8. Backend Deployment

Provider

Railway

---

Deployment Steps

1. Connect GitHub repository

2. Select backend project

3. Configure environment variables

4. Configure startup command

Example

```bash
dotnet Cashback.Api.dll
```

5. Deploy

---

Health Check

Endpoint

```text
/api/v1/health
```

Expected

```json
{
  "success": true
}
```

---

# 9. Frontend Deployment

Provider

Vercel

---

Deployment Steps

1. Connect GitHub repository

2. Select frontend folder

3. Configure environment variables

4. Deploy

---

Build Command

```bash
npm run build
```

Output Directory

```text
dist
```

---

# 10. Domain Setup

Frontend

Example

```text
cashback.example.com
```

---

API

Example

```text
api.cashback.example.com
```

---

DNS

Provider

Cloudflare

Recommended

Proxy Enabled

HTTPS Enabled

---

# 11. HTTPS Requirements

Production must use HTTPS only.

Required

- Frontend HTTPS
- Backend HTTPS
- OAuth Redirect HTTPS

---

Forbidden

```text
http://
```

in production.

---

# 12. Google OAuth Setup

Required Redirect URL

Development

```text
http://localhost:5173
```

Production

```text
https://cashback.example.com
```

---

Backend Callback

Example

```text
https://api.cashback.example.com/auth/google/callback
```

---

# 13. Accesstrade Setup

Required

- Publisher Account
- API Token
- Campaign Access

---

Webhook URL

Example

```text
https://api.cashback.example.com/api/v1/webhooks/accesstrade
```

---

Security

Validate

- Secret Key
- Signature
- Source

Never trust incoming requests directly.

---

# 14. Logging Strategy

Development

Console Logs

---

Production

Serilog

Store

- Information
- Warning
- Error

---

Future

Seq

Elasticsearch

Grafana

---

# 15. Monitoring

Minimum Monitoring

- API Availability
- Database Availability
- Webhook Success Rate

Metrics

- Active Users
- Generated Links
- Orders Received
- Cashback Amount
- Withdrawals

---

# 16. Backup Strategy

Database

Daily Backup

Retention

30 Days

---

Files

Future Feature

Cloudflare R2

---

# 17. Security Checklist

Before Production Launch

✓ HTTPS Enabled

✓ JWT Secret Configured

✓ Environment Variables Configured

✓ Swagger Disabled

✓ Rate Limiting Enabled

✓ Error Middleware Enabled

✓ Logging Enabled

✓ Database Backup Enabled

✓ CORS Restricted

---

# 18. CI/CD Pipeline

Trigger

Push To Main

---

Backend Pipeline

1. Restore

2. Build

3. Test

4. Publish

5. Deploy

---

Frontend Pipeline

1. Install Dependencies

2. Build

3. Lint

4. Deploy

---

# 19. Production Readiness Checklist

Backend

✓ Health Check

✓ Logging

✓ Error Handling

✓ Migrations Applied

✓ Environment Variables Configured

---

Frontend

✓ Build Success

✓ API Connected

✓ Google Login Working

---

Database

✓ Backup Configured

✓ Connection Tested

---

External Integrations

✓ Google OAuth Working

✓ Accesstrade API Working

✓ Webhook Working

---

# 20. MVP Deployment Goal

A deployment is considered successful when:

1. User can access website.

2. User can login with Google.

3. User can generate affiliate link.

4. Accesstrade webhook reaches API.

5. Orders appear in dashboard.

6. User can request withdrawal.

7. Admin can process withdrawal.

8. Platform can earn commission.

Everything else is secondary.
