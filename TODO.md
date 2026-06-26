# TODO.md

# Affiliate Cashback Platform MVP

Status Legend

* [ ] Not Started
* [/] In Progress
* [x] Completed

---

# EPIC 1 - Project Foundation

## Feature 1.1 - Solution Setup

### Backend

* [x] Create ASP.NET Core Web API project
* [x] Configure Clean Architecture
* [x] Configure CQRS pattern
* [x] Configure MediatR
* [x] Configure FluentValidation
* [x] Configure AutoMapper

### Infrastructure

* [x] Setup PostgreSQL
* [x] Setup EF Core
* [x] Configure DbContext
* [x] Create Initial Migration

### Frontend

* [x] Create Next.js project
* [x] Configure TypeScript
* [x] Configure TailwindCSS
* [x] Configure Shadcn UI
* [x] Configure React Query
* [x] Configure Axios

---

## Feature 1.2 - DevOps Setup

* [x] Dockerfile Backend
* [x] Dockerfile Frontend
* [x] docker-compose.yml
* [x] Environment Configuration

---

# EPIC 2 - Authentication Module

## Feature 2.1 - Google Login

### Backend

* [x] Configure Google OAuth
* [x] Create Login Endpoint
* [x] Generate JWT
* [x] Generate Refresh Token
* [x] Store Refresh Token

### Frontend

* [x] Login Page
* [x] Google Login Button
* [x] Auth Context
* [x] Route Protection

---

## Feature 2.2 - User Management

* [x] User Entity
* [x] User Repository
* [x] Get Current User API

---

# EPIC 3 - Affiliate Module

## Feature 3.1 - Accesstrade Integration

### Infrastructure

* [x] Create IAffiliateProvider
* [x] Create AccesstradeProvider
* [x] Configure API Client
* [x] Configure Campaign Settings

### Backend

* [x] Generate Affiliate Link Command
* [x] URL Validation
* [x] Generate Sub1 From UserId
* [x] Save Affiliate Link

---

## Feature 3.2 - Affiliate Link History

* [x] AffiliateLink Entity
* [x] AffiliateLink Repository
* [x] Get User Links API
* [x] Get Link Detail API

---

## Feature 3.3 - Affiliate UI

* [x] Link Converter Page
* [x] Paste URL Form
* [x] Generate Link Button
* [x] Copy Link Button
* [x] Link History Table

---

# EPIC 4 - Orders Module

## Feature 4.1 - Order Domain

* [x] Order Entity
* [x] Order Repository
* [x] Order Status Enum

---

## Feature 4.2 - Cashback Calculation

* [x] Cashback Service
* [x] Cashback Configuration
* [x] Cashback Calculation Logic

---

## Feature 4.3 - Order APIs

* [x] Get Orders API
* [x] Get Order Detail API
* [x] Get Order Summary API

---

## Feature 4.4 - Orders UI

* [x] Orders Page
* [x] Status Filters
* [x] Order Details Modal
* [x] Earnings Summary

---

# EPIC 5 - Webhooks Module

## Feature 5.1 - Accesstrade Webhook

### Backend

* [x] Webhook Endpoint
* [x] Request Validation
* [x] Signature Validation
* [x] DTO Mapping

### Security Layer

* [x] IWebhookValidator
* [x] AccesstradeWebhookValidator
* [x] Webhook Security Middleware
* [x] Secret Validation
* [x] Signature Validation Placeholder

---

## Feature 5.2 - Idempotency

* [x] WebhookEvents Table
* [x] Duplicate Detection
* [x] Processed Event Tracking
* [x] IWebhookEventRepository
* [x] WebhookEventRepository
* [x] WebhookIdempotencyService

---

## Feature 5.3 - Webhook Processing

* [x] Create Order From Webhook
* [x] Update Existing Order
* [x] Handle Order Approval
* [x] Handle Order Rejection
* [x] CreateOrderFromWebhookHandler
* [x] WebhookProcessingService
* [x] WebhookSub1UserResolver

---

## Feature 5.4 - Order Status Synchronization

* [x] OrderApprovalHandler
* [x] OrderRejectionHandler
* [x] OrderPendingHandler
* [x] OrderSynchronizationService
* [x] Status Update Logic
* [x] AuditLog Entity
* [x] AuditLogService
* [x] Integration Tests

---

# EPIC 6 - Dashboard Module

## Earnings Summary Service

* [x] IEarningsService
* [x] EarningsService
* [x] EarningsSummaryDto
* [x] Summary Calculations
* [x] Unit Tests

---

## Feature 6.1 - User Dashboard

* [x] Available Balance
* [x] Pending Cashback
* [x] Total Cashback
* [x] Total Withdrawn
* [x] Recent Orders

---

## Feature 6.2 - Dashboard APIs

* [x] Dashboard Summary Query
* [x] Dashboard Endpoint

---

## Feature 6.3 - Dashboard UI

* [x] Summary Cards
* [x] Earnings Chart
* [x] Recent Activities

---

# EPIC 7 - Withdrawals Module

## Feature 7.1 - Withdrawal Requests

### Backend

* [ ] Withdrawal Entity
* [ ] Create Withdrawal Command
* [ ] Balance Validation
* [ ] Withdrawal Repository

### Frontend

* [ ] Withdrawal Form
* [ ] Withdrawal History

---

## Feature 7.2 - Withdrawal Processing

* [ ] Approve Withdrawal
* [ ] Reject Withdrawal
* [ ] Complete Withdrawal

---

## Feature 7.3 - Financial Transactions

* [ ] Transaction Entity
* [ ] Transaction Repository
* [ ] Audit Trail

---

# EPIC 8 - Admin Module

## Feature 8.1 - Admin Dashboard

* [ ] User Statistics
* [ ] Order Statistics
* [ ] Withdrawal Statistics
* [ ] Revenue Statistics

---

## Feature 8.2 - User Management

* [ ] Users List
* [ ] User Details
* [ ] Suspend User
* [ ] Activate User

---

## Feature 8.3 - Order Management

* [ ] Orders List
* [ ] Order Search
* [ ] Order Filters

---

## Feature 8.4 - Withdrawal Management

* [ ] Withdrawal List
* [ ] Approve Action
* [ ] Reject Action
* [ ] Complete Action

---

# EPIC 9 - Settings Module

## Feature 9.1 - System Settings

* [ ] SystemSetting Entity
* [ ] Settings Repository
* [ ] Settings API

---

## Feature 9.2 - Admin Settings UI

* [ ] Cashback Percentage
* [ ] Min Withdrawal
* [ ] Max Withdrawal
* [ ] Campaign Id

---

# EPIC 10 - Notification Module

## Feature 10.1 - Notifications

* [ ] Notification Entity
* [ ] Notification Repository
* [ ] Notification APIs

---

## Feature 10.2 - Notification UI

* [ ] Notification Bell
* [ ] Notifications Page
* [ ] Unread Counter

---

# EPIC 11 - Background Jobs

## Feature 11.1 - Hangfire

* [ ] Install Hangfire
* [ ] Configure Dashboard
* [ ] Configure PostgreSQL Storage

---

## Feature 11.2 - Retry Jobs

* [ ] Retry Webhook Job
* [ ] Failed Event Recovery

---

## Feature 11.3 - Sync Jobs

* [ ] Sync Orders Job
* [ ] Sync Provider Status Job

---

# EPIC 12 - Security

## Feature 12.1 - Authorization

* [ ] JWT Validation
* [ ] Role Validation
* [ ] Ownership Validation

---

## Feature 12.2 - Rate Limiting

* [ ] Login Rate Limit
* [ ] Affiliate Rate Limit
* [ ] Webhook Rate Limit

---

## Feature 12.3 - Audit Logs

* [ ] AuditLog Entity
* [ ] Audit Repository
* [ ] Audit APIs

---

# EPIC 13 - Observability

## Feature 13.1 - Logging

* [ ] Install Serilog
* [ ] Structured Logging
* [ ] Correlation Id

---

## Feature 13.2 - Monitoring

* [ ] Health Checks
* [ ] Slow Request Logging
* [ ] Error Tracking

---

# EPIC 14 - Production Readiness

## Feature 14.1 - Testing

* [ ] Unit Tests
* [ ] Integration Tests
* [ ] API Tests

---

## Feature 14.2 - Deployment

* [ ] Health Check Endpoint
* [ ] CI/CD Pipeline
* [ ] Production Environment
* [ ] Database Migration Pipeline
* [ ] Backup Strategy

---

# MVP RELEASE CHECKLIST

## Core Business

* [ ] Google Login
* [ ] Generate Affiliate Link
* [ ] Webhook Processing
* [ ] Order Tracking
* [x] Cashback Calculation
* [ ] Withdrawal Requests

---

## Admin

* [ ] Admin Dashboard
* [ ] Withdrawal Approval
* [ ] User Management

---

## Reliability

* [ ] Logging
* [ ] Health Checks
* [ ] Error Handling
* [ ] Audit Logs

---

## Go Live

* [ ] Production Deployment
* [ ] Domain Setup
* [ ] SSL Setup
* [ ] Monitoring Setup
* [ ] Backup Setup

---

# MVP SUCCESS CRITERIA

* User đăng nhập bằng Google
* User tạo được affiliate link Shopee
* User mua hàng qua link
* Accesstrade gửi webhook
* Hệ thống tạo Order
* Cashback được ghi nhận
* User gửi yêu cầu rút tiền
* Admin duyệt rút tiền
* Người dùng nhận tiền thành công

MVP hoàn thành khi luồng trên hoạt động end-to-end.
