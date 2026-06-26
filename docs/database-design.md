# Database Design

Version: 1.1

---

# 1. Design Principles

MVP Database Goals:

- Simple
- Easy to implement
- Easy to scale
- Support Accesstrade Integration
- Support Google OAuth
- Future-ready for Email/Password login

Database Engine:

- PostgreSQL

ORM:

- Entity Framework Core

Naming Convention:

- Table: PascalCase
- Column: PascalCase
- Primary Key: Id
- Foreign Key: EntityNameId

---

# 2. Entity Relationship Diagram

Users
│
├── AffiliateLinks
│
├── Orders
│
├── Transactions
│
└── Withdrawals

Orders
│
└── (referenced by Transactions via ReferenceId)

---

# 3. Users

Purpose:

Store user accounts and account balances.

Table: Users

| Column           | Type          | Nullable |
| ---------------- | ------------- | -------- |
| Id               | UUID          | No       |
| Email            | VARCHAR(255)  | No       |
| PasswordHash     | TEXT          | Yes      |
| FullName         | VARCHAR(255)  | No       |
| AvatarUrl        | TEXT          | Yes      |
| Provider         | VARCHAR(50)   | No       |
| ProviderUserId   | VARCHAR(255)  | Yes      |
| Role             | VARCHAR(20)   | No       |
| Status           | INTEGER       | No       |
| EmailVerified    | BOOLEAN       | No       |
| AvailableBalance | DECIMAL(18,2) | No       |
| PendingBalance   | DECIMAL(18,2) | No       |
| LifetimeCashback | DECIMAL(18,2) | No       |
| LastLoginAt      | TIMESTAMP     | Yes      |
| CreatedAt        | TIMESTAMP     | No       |
| UpdatedAt        | TIMESTAMP     | No       |

Indexes:

- Email (Unique)
- ProviderUserId
- Role

Examples:

Google User

Provider = Google

ProviderUserId = google_user_id

PasswordHash = NULL

Local User

Provider = Local

PasswordHash = BCrypt Hash

---

# 3.1 UserRefreshTokens

Purpose:

Store JWT refresh tokens for session renewal.

Table: UserRefreshTokens

| Column    | Type         | Nullable |
| --------- | ------------ | -------- |
| Id        | UUID         | No       |
| UserId    | UUID         | No       |
| TokenHash | VARCHAR(255) | No       |
| ExpiresAt | TIMESTAMP    | No       |
| RevokedAt | TIMESTAMP    | Yes      |
| CreatedAt | TIMESTAMP    | No       |

Indexes:

- TokenHash (Unique)
- UserId

---

# 4. AffiliateLinks

Purpose:

Store generated affiliate links.

Table: AffiliateLinks

| Column       | Type         | Nullable |
| ------------ | ------------ | -------- |
| Id           | UUID         | No       |
| UserId       | UUID         | No       |
| OriginalUrl  | TEXT         | No       |
| AffiliateUrl | TEXT         | No       |
| ShortUrl     | TEXT         | Yes      |
| CampaignId   | VARCHAR(100) | Yes      |
| SubId        | VARCHAR(100) | No       |
| Merchant     | VARCHAR(100) | Yes      |
| CreatedAt    | TIMESTAMP    | No       |

Indexes:

- UserId
- SubId (Unique)

Example:

SubId = USER_123

Used to map orders back to users.

---

# 5. Orders

Purpose:

Store conversion/order data received from Accesstrade.

Table: Orders

| Column           | Type          | Nullable |
| ---------------- | ------------- | -------- |
| Id               | UUID          | No       |
| UserId           | UUID          | No       |
| AffiliateLinkId  | UUID          | Yes      |
| NetworkOrderId   | VARCHAR(255)  | No       |
| Merchant         | VARCHAR(100)  | Yes      |
| OrderAmount      | DECIMAL(18,2) | Yes      |
| CommissionAmount | DECIMAL(18,2) | No       |
| CashbackAmount   | DECIMAL(18,2) | No       |
| PlatformProfit   | DECIMAL(18,2) | No       |
| Status           | VARCHAR(50)   | No       |
| OrderDate        | TIMESTAMP     | Yes      |
| CreatedAt        | TIMESTAMP     | No       |
| UpdatedAt        | TIMESTAMP     | No       |

Indexes:

- UserId
- NetworkOrderId (Unique)
- Status

Status:

- Pending
- Approved
- Rejected

Business Rule:

CashbackAmount = CommissionAmount × CashbackRate

PlatformProfit = CommissionAmount - CashbackAmount

---

# 6. Transactions

Purpose:

Maintain balance history and audit trail.

Table: Transactions

| Column        | Type          | Nullable |
| ------------- | ------------- | -------- |
| Id            | UUID          | No       |
| UserId        | UUID          | No       |
| Type          | INTEGER       | No       |
| Amount        | DECIMAL(18,2) | No       |
| BalanceBefore | DECIMAL(18,2) | No       |
| BalanceAfter  | DECIMAL(18,2) | No       |
| ReferenceId   | UUID          | Yes      |
| Description   | TEXT          | Yes      |
| CreatedAt     | TIMESTAMP     | No       |

Indexes:

- UserId
- ReferenceId
- Type
- CreatedAt

Types:

- CashbackEarned
- WithdrawalRequested
- WithdrawalApproved
- WithdrawalRejected
- WithdrawalCompleted

Examples:

Approved Order

Amount: +70,000

BalanceBefore: 30,000

BalanceAfter: 100,000

Withdrawal Request

Amount: -50,000

BalanceBefore: 100,000

BalanceAfter: 50,000

---

# 7. Withdrawals

Purpose:

Store withdrawal requests.

Table: Withdrawals

| Column            | Type          | Nullable |
| ----------------- | ------------- | -------- |
| Id                | UUID          | No       |
| UserId            | UUID          | No       |
| Amount            | DECIMAL(18,2) | No       |
| BankName          | VARCHAR(255)  | No       |
| BankAccountNumber | VARCHAR(100)  | No       |
| BankAccountHolder | VARCHAR(255)  | No       |
| Status            | INTEGER       | No       |
| RequestedAt       | TIMESTAMP     | No       |
| ProcessedAt       | TIMESTAMP     | Yes      |

Indexes:

- UserId
- Status
- RequestedAt

Status:

- Pending
- Approved
- Rejected
- Completed

---

# 8. AuditLogs

Purpose:

Track administrative actions.

Table: AuditLogs

| Column     | Type         | Nullable |
| ---------- | ------------ | -------- |
| Id         | UUID         | No       |
| UserId     | UUID         | Yes      |
| Action     | VARCHAR(255) | No       |
| EntityName | VARCHAR(100) | No       |
| EntityId   | UUID         | Yes      |
| Metadata   | JSONB        | Yes      |
| CreatedAt  | TIMESTAMP    | No       |

Examples:

- Withdrawal Approved
- Withdrawal Rejected
- Order Updated
- User Disabled

---

# 9. Balance Strategy

Source Of Truth:

Transactions

Cached Values:

- AvailableBalance
- PendingBalance
- LifetimeCashback

Purpose:

Fast dashboard queries.

If balance inconsistency occurs:

Recalculate from Transactions.

---

# 10. Business Rules

BR001

One AffiliateLink belongs to one User.

BR002

One Order belongs to one User.

BR003

NetworkOrderId must be unique.

BR004

User cannot withdraw more than AvailableBalance.

BR005

Rejected orders generate no cashback.

BR006

Approved orders generate cashback.

BR007

Cashback is calculated from actual commission received.

BR008

Completed withdrawals reduce AvailableBalance.

---

# 11. MVP Tables

Required:

✓ Users

✓ AffiliateLinks

✓ Orders

✓ Transactions

✓ Withdrawals

Optional:

✓ AuditLogs

---

# 12. Future Tables (Phase 2)

Not required for MVP.

Potential additions:

- CashbackRates
- MerchantConfigs
- Notifications
- ReferralPrograms
- ReferralCommissions
- Campaigns
- UserDevices
- MarketingEvents
- ApiKeys
- FeatureFlags
