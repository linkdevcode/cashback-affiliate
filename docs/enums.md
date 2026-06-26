# Enums Specification

Version: 1.0

Purpose:

Centralized enum definitions used by:

- Backend (.NET)
- Frontend (React)
- Database
- API Contracts
- Documentation

Rule:

Never use magic strings in code.

Example:

❌ "Approved"

✅ OrderStatus.Approved

---

# 1. UserRole

Purpose:

Define user permissions.

Enum:

```csharp
public enum UserRole
{
    User = 1,
    Admin = 2
}
```

Database Value:

| Value | Name  |
| ----- | ----- |
| 1     | User  |
| 2     | Admin |

Description:

User

- Generate cashback links
- View orders
- Request withdrawals

Admin

- Manage users
- Manage orders
- Process withdrawals

---

# 2. UserStatus

Purpose:

Track user account lifecycle.

Enum:

```csharp
public enum UserStatus
{
    Active = 1,
    Suspended = 2,
    Deleted = 3
}
```

Database Value:

| Value | Name      |
| ----- | --------- |
| 1     | Active    |
| 2     | Suspended |
| 3     | Deleted   |

Description:

Active

Account can access platform features.

Suspended

Account cannot login or use platform features.

Deleted

Account is marked as deleted.

---

# 3. AuthProvider

Purpose:

Define authentication source.

Enum:

```csharp
public enum AuthProvider
{
    Local = 1,
    Google = 2
}
```

Database Value:

| Value | Name   |
| ----- | ------ |
| 1     | Local  |
| 2     | Google |

Description:

Local

Email + Password

Google

OAuth Login

---

# 4. OrderStatus

Purpose:

Track order lifecycle.

Enum:

```csharp
public enum OrderStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}
```

Database Value:

| Value | Name     |
| ----- | -------- |
| 1     | Pending  |
| 2     | Approved |
| 3     | Rejected |

Description:

Pending

Waiting for merchant confirmation.

Approved

Commission confirmed.

Rejected

Cancelled, returned, or invalid order.

---

# 5. WithdrawalStatus

Purpose:

Track withdrawal processing.

Enum:

```csharp
public enum WithdrawalStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Completed = 4
}
```

Database Value:

| Value | Name      |
| ----- | --------- |
| 1     | Pending   |
| 2     | Approved  |
| 3     | Rejected  |
| 4     | Completed |

Description:

Pending

Waiting for admin review.

Approved

Accepted and queued for payment.

Rejected

Denied by admin.

Completed

Successfully transferred.

---

# 6. TransactionType

Purpose:

Track balance changes.

Enum:

```csharp
public enum TransactionType
{
    CashbackPending = 1,
    CashbackApproved = 2,
    CashbackReversed = 3,
    WithdrawalRequested = 4,
    WithdrawalCompleted = 5,
    Adjustment = 6
}
```

Database Value:

| Value | Name                |
| ----- | ------------------- |
| 1     | CashbackPending     |
| 2     | CashbackApproved    |
| 3     | CashbackReversed    |
| 4     | WithdrawalRequested |
| 5     | WithdrawalCompleted |
| 6     | Adjustment          |

Description:

CashbackPending

Commission detected but not approved.

CashbackApproved

Commission approved and added to balance.

CashbackReversed

Previously approved commission removed.

WithdrawalRequested

User submitted withdrawal request.

WithdrawalCompleted

Withdrawal paid successfully.

Adjustment

Manual admin adjustment.

---

# 7. AuditAction

Purpose:

Standardize audit logs.

Enum:

```csharp
public enum AuditAction
{
    UserCreated = 1,
    UserUpdated = 2,
    UserDisabled = 3,

    LinkGenerated = 10,

    OrderCreated = 20,
    OrderUpdated = 21,

    WithdrawalRequested = 30,
    WithdrawalApproved = 31,
    WithdrawalRejected = 32,
    WithdrawalCompleted = 33
}
```

Description:

Used for tracking system events.

---

# 8. MerchantType

Purpose:

Identify order source.

Enum:

```csharp
public enum MerchantType
{
    Shopee = 1,
    Lazada = 2,
    TikTokShop = 3,
    Other = 99
}
```

Database Value:

| Value | Name       |
| ----- | ---------- |
| 1     | Shopee     |
| 2     | Lazada     |
| 3     | TikTokShop |
| 99    | Other      |

---

Future enums should be added here first before implementation.

---

# 9. NotificationType

Purpose:

Identify notification category.

Enum:

```csharp
public enum NotificationType
{
    OrderApproved = 1,
    OrderRejected = 2,
    WithdrawalCreated = 3,
    WithdrawalApproved = 4,
    WithdrawalRejected = 5,
    WithdrawalCompleted = 6,
    SystemAnnouncement = 7
}
```

---

# 10. WebhookEventStatus

Purpose:

Track webhook processing lifecycle.

Enum:

```csharp
public enum WebhookEventStatus
{
    Received = 1,
    Processing = 2,
    Processed = 3,
    Failed = 4,
    Ignored = 5
}
```

---

# 11. System Constants

Cashback Rate

```csharp
public const decimal DefaultCashbackRate = 0.7m;
```

Minimum Withdrawal

```csharp
public const decimal MinimumWithdrawalAmount = 100000m;
```

Currency

```csharp
public const string Currency = "VND";
```

---

# 12. Development Rules

Rule 1

All enums must be stored as integers in database.

Example:

OrderStatus = 2

Not:

OrderStatus = "Approved"

---

Rule 2

Frontend must map enum values to display labels.

Example:

2

↓

Approved

↓

"Đã duyệt"

---

Rule 3

API responses should return both value and label.

Example:

```json
{
  "status": 2,
  "statusName": "Approved"
}
```
