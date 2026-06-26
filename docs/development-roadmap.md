# Development Roadmap

Version: 1.0

Project

Cashback Affiliate Platform

Goal

Build and launch a working MVP capable of generating real affiliate commission revenue.

---

# 1. Product Vision

Users can:

1. Login with Google
2. Paste Shopee/Lazada/TikTok Shop links
3. Generate cashback links
4. Purchase products
5. Receive cashback
6. Withdraw money

The system earns profit from the difference between:

Commission Received

minus

Cashback Paid

---

# 2. Development Philosophy

Priority Order

Revenue

>

Core Functionality

>

Reliability

>

UI

>

Advanced Features

---

Always ask:

"Will this help generate the first real commission?"

If the answer is No:

Postpone it.

---

# 3. MVP Scope

Included

✓ Google Login

✓ Dashboard

✓ Generate Affiliate Link

✓ Order Tracking

✓ Cashback Calculation

✓ Withdrawal Request

✓ Admin Panel

✓ Accesstrade Integration

✓ Postback Processing

Excluded

✗ Referral System

✗ Notification System

✗ Mobile App

✗ Voucher Marketplace

✗ AI Features

✗ Multi Tenant

---

# 4. Development Phases

Phase 0

Project Foundation

Duration

1 Day

Goal

Prepare project structure.

Tasks

- Create repository
- Setup solution structure
- Setup React project
- Setup PostgreSQL
- Setup documentation
- Setup Cursor Rules
- Setup CI/CD skeleton

Deliverables

Running frontend

Running backend

Connected database

---

Phase 1

Authentication

Duration

1 Day

Goal

Users can log in.

Tasks

Backend

- JWT setup
- Google OAuth integration
- User creation
- Refresh token support

Frontend

- Login page
- Google Sign In
- Protected routes

Deliverables

User can log in successfully.

---

Phase 2

Affiliate Link Generation

Duration

1 Day

Goal

Users can generate cashback links.

Tasks

Backend

- Accesstrade client
- Generate affiliate link endpoint
- Save AffiliateLinks

Frontend

- Landing page
- Link generator form
- Copy button

Deliverables

User can generate affiliate links.

Success Criteria

Generated link opens correctly.

---

Phase 3

Dashboard

Duration

1 Day

Goal

Users can view earnings.

Tasks

Backend

- Dashboard endpoint
- Balance calculation
- Summary query

Frontend

- Dashboard page
- Balance cards
- Recent orders

Deliverables

Dashboard displays user statistics.

---

Phase 4

Orders Module

Duration

1 Day

Goal

Track affiliate conversions.

Tasks

Backend

- Orders API
- Order query
- Pagination
- Filtering

Frontend

- Orders page
- Status badges
- Filters

Deliverables

Users can view order history.

---

Phase 5

Accesstrade Postback

Duration

1 Day

Goal

Receive conversion data.

Tasks

Backend

- Webhook endpoint
- Signature validation
- Order processing
- Commission processing

Deliverables

Orders automatically appear after conversion.

Success Criteria

Test webhook updates database correctly.

---

Phase 6

Withdrawal Module

Duration

1 Day

Goal

Allow users to request payments.

Tasks

Backend

- Create withdrawal request
- Withdrawal history
- Validation

Frontend

- Withdrawal page
- Bank account form
- History table

Deliverables

Users can request withdrawals.

---

Phase 7

Admin Panel

Duration

1 Day

Goal

Manage operations.

Tasks

Backend

- Admin authorization
- Users endpoint
- Orders endpoint
- Withdrawals endpoint

Frontend

- Admin dashboard
- Withdrawal management
- Order management

Deliverables

Admin can operate platform manually.

---

Phase 8

Production Deployment

Duration

1 Day

Goal

Launch MVP.

Tasks

Backend

- Production configuration
- Logging
- Error monitoring

Frontend

- Production build
- SEO basics

Infrastructure

- Deploy API
- Deploy frontend
- Deploy database

Deliverables

Publicly accessible MVP.

---

# 5. Sprint Plan

Sprint 1

Foundation

Days 1-2

Modules

- Auth
- Users
- Database
- Infrastructure

Goal

User Login Working

---

Sprint 2

Core Revenue Engine

Days 3-4

Modules

- Affiliate Links
- Dashboard

Goal

Generate Cashback Links

---

Sprint 3

Commission Tracking

Days 5-6

Modules

- Orders
- Postback

Goal

Track Real Orders

---

Sprint 4

Monetization

Days 7-8

Modules

- Withdrawals
- Admin

Goal

Pay Users

---

Sprint 5

Launch

Days 9-10

Modules

- Deployment
- Bug Fixes
- Monitoring

Goal

Production Ready MVP

---

# 6. First Revenue Milestone

Milestone Name

First Commission

Definition

A real user generates a cashback link.

↓

Purchases a product.

↓

Accesstrade records conversion.

↓

Commission appears in system.

Success Metrics

- 1 successful conversion
- 1 successful cashback calculation
- 1 verified order

---

# 7. First User Milestone

Milestone Name

First External User

Definition

Someone other than the developer:

- Registers
- Generates a link
- Uses the product

Goal

Validate demand.

---

# 8. Launch Checklist

Authentication

✓ Google Login

✓ JWT

✓ Protected Routes

Affiliate

✓ Generate Link

✓ Store History

Orders

✓ View Orders

✓ Order Details

Cashback

✓ Pending Balance

✓ Available Balance

✓ Lifetime Cashback

Withdrawal

✓ Request Withdrawal

✓ View History

Admin

✓ Manage Withdrawals

✓ Manage Orders

Infrastructure

✓ HTTPS

✓ Database Backup

✓ Logging

✓ Error Handling

---

# 9. Post-MVP Roadmap

Version 1.1

- Merchant Logos
- Better Dashboard
- Search Orders

Version 1.2

- Referral System
- Invite Friends
- Referral Commission

Version 1.3

- Notifications
- Email Updates
- Telegram Alerts

Version 2.0

- Mobile App
- Public API
- Multi Merchant Cashback

---

# 10. Features That Must Be Rejected During MVP

Do Not Build

- Chatbot
- AI Assistant
- Loyalty Points
- Ranking System
- Social Features
- Dark Mode Customization
- Advanced Analytics

Reason

These features do not directly contribute to earning the first commission.

---

# 11. Definition of MVP Success

The MVP is considered successful when:

1. User logs in.

2. User generates affiliate link.

3. User purchases product.

4. Accesstrade reports conversion.

5. Cashback is calculated.

6. User requests withdrawal.

7. Admin completes payment.

8. Platform earns profit.

Nothing else is required for MVP success.
