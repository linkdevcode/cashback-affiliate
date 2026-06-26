# API Specification

Version: 1.0

Base URL

/api/v1

Architecture

REST API

Authentication

JWT Bearer Token

---

# Response Standard

All APIs must use the following response format.

Success

{
  "success": true,
  "message": "Operation completed successfully",
  "data": {}
}

Failure

{
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Invalid input"
  ]
}

---

# Authentication

Authentication Method

JWT Bearer Token

Header

Authorization: Bearer {token}

---

# AUTH MODULE

Route

/api/v1/auth

---

## Login With Google

POST /auth/google

Description

Authenticate user using Google OAuth token.

Request

{
  "idToken": "google_oauth_token"
}

Response

{
  "success": true,
  "data": {
    "accessToken": "jwt_token",
    "refreshToken": "refresh_token",
    "user": {
      "id": "uuid",
      "email": "user@gmail.com",
      "fullName": "John Doe",
      "role": 1
    }
  }
}

---

## Refresh Token

POST /auth/refresh-token

Request

{
  "refreshToken": "refresh_token"
}

Response

{
  "success": true,
  "data": {
    "accessToken": "new_access_token"
  }
}

---

## Get Current User

GET /auth/me

Response

{
  "success": true,
  "data": {
    "id": "uuid",
    "email": "user@gmail.com",
    "fullName": "John Doe",
    "role": 1,
    "availableBalance": 100000,
    "pendingBalance": 50000
  }
}

---

# DASHBOARD MODULE

Route

/api/v1/dashboard

---

## Get Dashboard Summary

GET /dashboard

Response

{
  "success": true,
  "data": {
    "availableBalance": 100000,
    "pendingBalance": 50000,
    "lifetimeCashback": 350000,
    "totalOrders": 12,
    "recentOrders": []
  }
}

---

# AFFILIATE LINKS MODULE

Route

/api/v1/affiliate-links

---

## Generate Cashback Link

POST /affiliate-links

Description

Generate affiliate tracking link.

Request

{
  "url": "https://shopee.vn/..."
}

Response

{
  "success": true,
  "data": {
    "id": "uuid",
    "originalUrl": "https://shopee.vn/...",
    "affiliateUrl": "https://tracking.accesstrade.vn/...",
    "shortUrl": "https://shorten.accesstrade.vn/..."
  }
}

Business Rules

- URL required
- Only supported merchants allowed
- Store generated link history

---

## Get My Generated Links

GET /affiliate-links

Query Parameters

?page=1&pageSize=20

Response

{
  "success": true,
  "data": {
    "items": [],
    "totalCount": 100
  }
}

---

# ORDERS MODULE

Route

/api/v1/orders

---

## Get My Orders

GET /orders

Query Parameters

?page=1
&pageSize=20
&status=2

Response

{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "orderCode": "ORDER001",
        "merchant": "Shopee",
        "orderAmount": 1000000,
        "commissionAmount": 100000,
        "cashbackAmount": 70000,
        "status": 2,
        "statusName": "Approved",
        "createdAt": "2026-06-24T10:00:00Z"
      }
    ],
    "totalCount": 1
  }
}

---

## Get Order Details

GET /orders/{id}

Response

{
  "success": true,
  "data": {
    "id": "uuid",
    "orderCode": "ORDER001",
    "merchant": "Shopee",
    "orderAmount": 1000000,
    "commissionAmount": 100000,
    "cashbackAmount": 70000,
    "platformProfit": 30000,
    "status": 2
  }
}

---

# WITHDRAWAL MODULE

Route

/api/v1/withdrawals

---

## Create Withdrawal Request

POST /withdrawals

Request

{
  "amount": 100000,
  "bankName": "Vietcombank",
  "bankAccountNumber": "0123456789",
  "bankAccountName": "Nguyen Van A"
}

Response

{
  "success": true,
  "message": "Withdrawal request submitted"
}

Validation

- Amount > 0
- Amount >= MinimumWithdrawal
- Amount <= AvailableBalance

---

## Get Withdrawal History

GET /withdrawals

Response

{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "amount": 100000,
        "status": 4,
        "statusName": "Completed",
        "requestedAt": "2026-06-24T10:00:00Z"
      }
    ]
  }
}

---

# ADMIN MODULE

Route

/api/v1/admin

Authentication

Admin only

---

# ADMIN USERS

## Get Users

GET /admin/users

Query Parameters

?page=1
&pageSize=20

Response

{
  "success": true,
  "data": {
    "items": []
  }
}

---

## Get User Details

GET /admin/users/{id}

Response

{
  "success": true,
  "data": {}
}

---

# ADMIN ORDERS

## Get Orders

GET /admin/orders

Filters

- UserId
- Merchant
- Status
- Date Range

Response

{
  "success": true,
  "data": {
    "items": []
  }
}

---

## Get Order Details

GET /admin/orders/{id}

Response

{
  "success": true,
  "data": {}
}

---

# ADMIN WITHDRAWALS

## Get Withdraw Requests

GET /admin/withdrawals

Response

{
  "success": true,
  "data": {
    "items": []
  }
}

---

## Approve Withdrawal

POST /admin/withdrawals/{id}/approve

Response

{
  "success": true
}

---

## Reject Withdrawal

POST /admin/withdrawals/{id}/reject

Request

{
  "reason": "Invalid bank account"
}

Response

{
  "success": true
}

---

## Complete Withdrawal

POST /admin/withdrawals/{id}/complete

Response

{
  "success": true
}

Business Rules

- Only approved requests can be completed
- Completion updates user balance
- Create transaction log

---

# WEBHOOK MODULE

Route

/api/v1/webhooks

Authentication

Secret Key Validation

---

## Accesstrade Postback

POST /webhooks/accesstrade

Description

Receive conversion data from Accesstrade.

Expected Payload

{
  "orderId": "ORDER123",
  "sub1": "USER_123",
  "commission": 100000,
  "status": "approved"
}

Processing

1. Validate secret
2. Validate payload
3. Find user by sub1
4. Create or update order
5. Update balances
6. Create transaction
7. Create audit log

Response

{
  "success": true
}

Idempotency

Duplicate orderId must not create duplicate records.

---

# HEALTH MODULE

Route

/api/v1/health

---

## Health Check

GET /health

Response

{
  "success": true,
  "data": {
    "status": "Healthy"
  }
}

---

# Error Codes

400

Validation Error

401

Unauthorized

403

Forbidden

404

Not Found

409

Conflict

500

Internal Server Error

---

# Pagination Standard

Request

?page=1&pageSize=20

Response

{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 100,
  "totalPages": 5
}

---

# MVP API List

Authentication

✓ Login Google
✓ Refresh Token
✓ Get Current User

Dashboard

✓ Get Dashboard

Affiliate Links

✓ Generate Link
✓ Get Links

Orders

✓ Get Orders
✓ Get Order Detail

Withdrawals

✓ Create Request
✓ Get History

Admin

✓ Users
✓ Orders
✓ Withdrawals

Webhook

✓ Accesstrade Postback

System

✓ Health Check