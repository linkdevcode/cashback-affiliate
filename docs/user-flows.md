# User Flows

Version: 1.0

Purpose:

Define all user journeys in the Cashback Affiliate Platform MVP.

---

# Actors

## Guest

Unauthenticated user.

## User

Authenticated user.

## Admin

System administrator.

---

# Flow 1 - Login With Google

Actor:

Guest

Goal:

Access the system.

---

Steps

1. User clicks "Login with Google"

2. System redirects to Google OAuth

3. User selects Google account

4. Google returns user profile

5. System checks if user exists

6. If user does not exist

   - Create new user

7. Generate JWT token

8. Redirect to Dashboard

---

Success Result

User logged in successfully.

---

Failure Cases

- Google OAuth failed
- User cancelled login
- Google account unavailable

---

# Flow 2 - Generate Cashback Link

Actor:

User

Goal:

Create affiliate cashback link.

---

Steps

1. User enters product URL

Example:

https://shopee.vn/xxx

2. User clicks Generate Link

3. System validates URL

4. System calls Accesstrade API

5. Accesstrade returns affiliate URL

6. System stores AffiliateLink

7. System displays generated link

---

Success Result

User receives cashback link.

---

Failure Cases

- Empty URL
- Invalid URL
- Unsupported domain
- Accesstrade API unavailable

---

# Flow 3 - Copy Cashback Link

Actor:

User

Goal:

Use generated cashback link.

---

Steps

1. User clicks Copy

2. System copies affiliate URL

3. User shares or opens link

---

Success Result

Affiliate link copied successfully.

---

# Flow 4 - Affiliate Conversion

Actor:

User + Accesstrade

Goal:

Generate commission.

---

Steps

1. User opens affiliate link

2. User purchases product

3. Merchant confirms order

4. Accesstrade records conversion

5. Accesstrade sends conversion data

6. System identifies User via SubId

7. System creates Order

8. System creates CommissionTransaction

9. Pending balance updated

---

Success Result

Order appears in dashboard.

---

Failure Cases

- Tracking lost
- Merchant rejected order
- Conversion not reported

---

# Flow 5 - View Dashboard

Actor:

User

Goal:

Monitor cashback earnings.

---

Steps

1. User opens Dashboard

2. System loads:

   - Available Balance
   - Pending Balance
   - Lifetime Cashback
   - Recent Orders

3. Display data

---

Success Result

Dashboard displayed successfully.

---

# Flow 6 - View Order History

Actor:

User

Goal:

View cashback orders.

---

Steps

1. User navigates to Orders

2. System loads order list

3. User filters orders

Optional Filters:

- Status
- Date Range
- Merchant

4. Display results

---

Success Result

Orders displayed successfully.

---

# Flow 7 - Submit Withdrawal Request

Actor:

User

Goal:

Withdraw cashback.

---

Preconditions

Available Balance >= Minimum Withdrawal

---

Steps

1. User clicks Withdraw

2. User enters:

   - Amount
   - Bank Name
   - Account Number
   - Account Holder

3. System validates request

4. Create WithdrawRequest

5. Create Audit Log

---

Success Result

Withdrawal request submitted.

---

Failure Cases

- Insufficient balance
- Invalid amount
- Missing bank information

---

# Flow 8 - View Withdrawal History

Actor:

User

Goal:

Track withdrawal status.

---

Steps

1. User opens Withdrawals

2. System loads request history

3. Display status

Possible Status:

- Pending
- Approved
- Rejected
- Completed

---

Success Result

History displayed successfully.

---

# Flow 9 - Admin View Orders

Actor:

Admin

Goal:

Monitor system orders.

---

Steps

1. Admin opens Orders page

2. System loads orders

3. Admin searches or filters

4. Display results

---

Filters

- User
- Merchant
- Status
- Date

---

Success Result

Orders displayed successfully.

---

# Flow 10 - Admin Process Withdrawal

Actor:

Admin

Goal:

Pay users.

---

Steps

1. Admin opens Withdrawal Requests

2. Admin selects request

3. Review details

4. Transfer money manually

5. Mark request as Completed

6. System:

   - Update WithdrawalStatus
   - Update Balance
   - Create Transaction
   - Create Audit Log

---

Success Result

Withdrawal completed.

---

Failure Cases

- Bank transfer failed
- Invalid request

---

# Flow 11 - Accesstrade Postback

Actor:

Accesstrade

Goal:

Sync commissions.

---

Steps

1. Accesstrade sends webhook

2. System validates request

3. Extract:

   - OrderId
   - SubId
   - Commission
   - Status

4. Find User by SubId

5. Create or update Order

6. Update balances

7. Create CommissionTransaction

8. Create Audit Log

---

Success Result

Order synchronized successfully.

---

Failure Cases

- Invalid payload
- Unknown SubId
- Duplicate OrderId

---

# Flow 12 - User Registration (Automatic)

Actor:

Guest

Goal:

Become a user.

---

Steps

1. Login with Google

2. User not found

3. Create account automatically

4. Assign UserRole = User

5. Initialize balances

6. Redirect Dashboard

---

Success Result

New account created automatically.

---

# MVP Screens Mapping

Landing Page

- Flow 1
- Flow 2

Dashboard

- Flow 5

Orders

- Flow 6

Withdraw

- Flow 7
- Flow 8

Admin Orders

- Flow 9

Admin Withdrawals

- Flow 10

Webhook Endpoint

- Flow 11

---

# Future Flows (Phase 2)

Not included in MVP.

Potential additions:

- Referral Program
- Cashback Rate Management
- Notifications
- User Profile Management
- Merchant Pages
- Deals & Voucher System
- Marketing Campaign Tracking
