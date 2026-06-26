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
* [ ] Health Check Endpoint
* [ ] CI/CD Pipeline

---

# EPIC 2 - Authentication Module

## Feature 2.1 - Google Login

### Backend

* [ ] Configure Google OAuth
* [ ] Create Login Endpoint
* [ ] Generate JWT
* [ ] Generate Refresh Token
* [ ] Store Refresh Token

### Frontend

* [ ] Login Page
* [ ] Google Login Button
* [ ] Auth Context
* [ ] Route Protection

---

## Feature 2.2 - User Management

* [ ] User Entity
* [ ] User Repository
* [ ] Get Current User API
* [ ] Update Profile API
* [ ] User Dashboard Summary API

---

# EPIC 3 - Affiliate Module

## Feature 3.1 - Accesstrade Integration

### Infrastructure

* [ ] Create IAffiliateProvider
* [ ] Create AccesstradeProvider
* [ ] Configure API Client
* [ ] Configure Campaign Settings

### Backend

* [ ] Generate Affiliate Link Command
* [ ] URL Validation
* [ ] Generate Sub1 From UserId
* [ ] Save Affiliate Link

---

## Feature 3.2 - Affiliate Link History

* [ ] AffiliateLink Entity
* [ ] AffiliateLink Repository
* [ ] Get User Links API
* [ ] Get Link Detail API

---

## Feature 3.3 - Affiliate UI

* [ ] Link Converter Page
* [ ] Paste URL Form
* [ ] Generate Link Button
* [ ] Copy Link Button
* [ ] Link History Table

---

# EPIC 4 - Orders Module

## Feature 4.1 - Order Domain

* [ ] Order Entity
* [ ] Order Repository
* [ ] Order Status Enum

---

## Feature 4.2 - Cashback Calculation

* [ ] Cashback Service
* [ ] Cashback Configuration
* [ ] Cashback Calculation Logic

---

## Feature 4.3 - Order APIs

* [ ] Get Orders API
* [ ] Get Order Detail API
* [ ] Get Order Summary API

---

## Feature 4.4 - Orders UI

* [ ] Orders Page
* [ ] Status Filters
* [ ] Order Details Modal
* [ ] Earnings Summary

---

# EPIC 5 - Webhooks Module

## Feature 5.1 - Accesstrade Webhook

### Backend

* [ ] Webhook Endpoint
* [ ] Request Validation
* [ ] Signature Validation
* [ ] DTO Mapping

---

## Feature 5.2 - Idempotency

* [ ] WebhookEvents Table
* [ ] Duplicate Detection
* [ ] Processed Event Tracking

---

## Feature 5.3 - Webhook Processing

* [ ] Create Order From Webhook
* [ ] Update Existing Order
* [ ] Handle Order Approval
* [ ] Handle Order Rejection

---

# EPIC 6 - Dashboard Module

## Feature 6.1 - User Dashboard

* [ ] Available Balance
* [ ] Pending Cashback
* [ ] Total Cashback
* [ ] Total Withdrawn
* [ ] Recent Orders

---

## Feature 6.2 - Dashboard APIs

* [ ] Dashboard Summary Query
* [ ] Dashboard Endpoint

---

## Feature 6.3 - Dashboard UI

* [ ] Summary Cards
* [ ] Earnings Chart
* [ ] Recent Activities

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
* [ ] Cashback Calculation
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
