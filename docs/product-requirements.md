# Cashback Affiliate Platform - Product Requirements

Version: 1.0
Status: MVP
Author: Linh Nguyen

---

# 1. Product Vision

Xây dựng nền tảng Cashback Affiliate cho người dùng Việt Nam.

Người dùng chỉ cần dán link sản phẩm từ Shopee, Lazada hoặc TikTok Shop để nhận được link hoàn tiền.

Khi người dùng mua hàng thông qua link đó, nền tảng sẽ nhận hoa hồng Affiliate từ Accesstrade và chia lại một phần cho người dùng dưới dạng Cashback.

---

# 2. Business Goal

Mục tiêu MVP:

- Xác thực mô hình kinh doanh hoạt động được
- Tạo được affiliate link tự động
- Theo dõi được đơn hàng
- Theo dõi được commission
- Hỗ trợ người dùng yêu cầu rút tiền

Không tập trung vào:

- Tăng trưởng người dùng
- Marketing automation
- Gamification
- Referral
- Mobile App

---

# 3. Revenue Model

Ví dụ:

Order Value: 1.000.000 VNĐ

Affiliate Commission:
100.000 VNĐ

Cashback To User:
70.000 VNĐ

Platform Profit:
30.000 VNĐ

Formula:

Cashback Amount = Commission × Cashback Rate

Platform Profit = Commission - Cashback Amount

Default Cashback Rate:

70%

Configurable by Admin.

---

# 4. User Roles

## Guest

Unauthenticated user.

Permissions:

- View Landing Page
- Login
- Register

---

## User

Authenticated user.

Permissions:

- Generate Cashback Link
- View Order History
- View Cashback Balance
- Request Withdrawal

---

## Admin

System administrator.

Permissions:

- Manage Users
- View Orders
- Manage Withdraw Requests
- Configure Cashback Rate

---

# 5. Core User Flow

## Flow 1 - Generate Cashback Link

User

↓

Paste Product URL

↓

System validates URL

↓

System calls Accesstrade API

↓

System generates Tracking Link

↓

User copies generated link

---

## Flow 2 - Receive Commission

User clicks generated link

↓

User purchases product

↓

Accesstrade records conversion

↓

Accesstrade sends order data

↓

System maps order to user via SubId

↓

System calculates cashback

↓

System updates balance

---

## Flow 3 - Withdrawal

User enters withdrawal amount

↓

System validates balance

↓

System creates withdrawal request

↓

Admin reviews request

↓

Admin transfers money manually

↓

Admin marks request as completed

↓

Balance reduced

---

# 6. MVP Features

## F001 - Authentication

Priority: Must Have

Description:

Allow users to login.

Initial implementation:

- Google Login

Future:

- Email Password Login

---

## F002 - Generate Cashback Link

Priority: Must Have

Input:

Product URL

Output:

Affiliate Tracking URL

Business Rules:

- Only supported domains accepted
- Invalid URL rejected
- Store generated links history

---

## F003 - Dashboard

Priority: Must Have

Display:

- Available Cashback
- Pending Cashback
- Total Cashback Earned
- Recent Orders

---

## F004 - Orders

Priority: Must Have

Display:

- Order Code
- Merchant
- Commission
- Cashback
- Status
- Created Date

Statuses:

- Pending
- Approved
- Rejected

---

## F005 - Withdrawal Request

Priority: Must Have

User inputs:

- Amount
- Bank Name
- Account Number
- Account Holder

Validation:

- Amount > 0
- Amount <= Available Balance
- Minimum Withdrawal Amount

Default:

100,000 VNĐ

---

## F006 - Admin Orders Management

Priority: Must Have

Admin can:

- View Orders
- Search Orders
- Filter Orders

---

## F007 - Admin Withdrawal Management

Priority: Must Have

Admin can:

- View Requests
- Approve Requests
- Reject Requests
- Mark Completed

---

# 7. External Integrations

## Accesstrade

Required APIs:

### Authentication

Get Access Token

### Generate Tracking Link

Create affiliate link

### Orders

Sync conversions

### Commission

Sync commission data

### Postback

Receive conversion events

---

# 8. Business Rules

## BR001

Every generated link belongs to exactly one user.

---

## BR002

Each order belongs to exactly one user.

---

## BR003

User cannot withdraw more than available balance.

---

## BR004

Rejected orders generate no cashback.

---

## BR005

Approved orders generate cashback.

---

## BR006

Cashback is calculated from actual commission received.

---

# 9. Non Functional Requirements

## Performance

Link generation:

< 3 seconds

---

## Security

- JWT Authentication
- HTTPS Only
- Secure API Keys
- Server-side validation

---

## Logging

Log:

- Link generation
- Order sync
- Withdrawal requests
- Admin actions

---

# 10. Out Of Scope (MVP)

Not included:

- Referral Program
- Multi-level Commission
- Mobile Application
- Push Notifications
- Loyalty Points
- AI Features
- Multi-Tenant SaaS

---

# 11. Success Metrics

MVP considered successful if:

- Users can generate affiliate links
- Orders can be mapped to users
- Cashback can be calculated correctly
- Withdrawal process works

Business KPI:

- First successful commission
- First cashback payout
- First profitable transaction

---

# 12. MVP Definition of Done

Project is complete when:

✓ User can login

✓ User can generate cashback link

✓ User can view orders

✓ User can view cashback balance

✓ User can request withdrawal

✓ Admin can manage orders

✓ Admin can manage withdrawals

✓ Accesstrade integration works

✓ First real commission received