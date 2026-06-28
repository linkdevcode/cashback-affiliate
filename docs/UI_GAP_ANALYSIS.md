# UI Gap Analysis

This document compares the current frontend implementation with the mockup-derived reference in `docs/UI_REFERENCE.md`.

## Overview

The current frontend is implemented as an authenticated dashboard application with a sidebar navigation model and shadcn/Tailwind-based cards. It does not currently mirror the public landing experience, floating glass header, and homepage workspace layout shown in the mockup reference.

Key high-level differences:
- Current UI is app-first, with protected dashboard routes and sidebar navigation.
- `docs/UI_REFERENCE.md` is based on a public landing flow plus a workspace shell with glassmorphism and a top tab-style app nav.
- Theme toggle and transparent glass shell are present in the mockup concept but not implemented in the current frontend.
- The current app uses a neutral Tailwind theme with generic `brand` variables, rather than the specific emerald/slate palette in the reference.

---

## Dashboard

### Current state
- Route: `/dashboard`
- Components:
  - `DashboardOverview`
  - `DashboardSummaryCards`
  - `DashboardEmptyBanner`
  - `DashboardQuickActions`
  - `EarningsChart`
  - `RecentActivities`
  - `RecentOrdersWidget`
- Layout: sidebar app shell, header title, summary metrics, chart, activity section, recent orders.
- Styling: standard shadcn cards, consistent page spacing, generic `brand` accent color.
- Functionality: dashboard summary loads data, handles loading/error, shows empty state, and offers CTA buttons to affiliate link generation and withdrawals.

### Missing features
- Inline workspace homepage design from UI reference that combines the converter and wallet overview in one top section.
- Dedicated wallet overview card with explicit available balance / pending cashback breakdown in the same hero area.
- A `Create link` block directly on the dashboard page, as in the reference workspace.
- The mockup's trust metrics and summary highlight panels are not present on this page.

### Visual differences
- Sidebar navigation vs mockup's top floating glass navigation and app tabs.
- No glassmorphism / translucent header shell.
- Current page looks more like a traditional admin dashboard rather than a modern fintech landing/workspace hybrid.
- The mockup uses emerald accent highlights and gradient branding; current UI uses generic brand colors from global CSS.
- Current card and section spacing is tighter and more conventional than the expansive hero / workspace composition in the reference.

### Refactor complexity
- Medium.
- Existing dashboard components and page structure are in place, so the work is mostly layout reorganization and surface-level UI restyling.
- Reusing the existing affiliate converter component inside the dashboard page would reduce complexity.

### Estimated effort
- 3–4 days to align the dashboard page with the UI reference, depending on how closely the public landing/workspace split should be reproduced.

---

## Affiliate

### Current state
- Route: `/dashboard/affiliate`
- Components:
  - `AffiliatePage`
  - `LinkConverterForm`
  - `AffiliateLinkResultCard`
- Layout: centered form container, page heading, description, history link, URL input, generate button, and post-generation result card.
- Functionality: affiliate link generation, clipboard copy, server error handling, result preview.

### Missing features
- The reference’s more vivid inline preview UI with platform badge styling is not present.
- There is no one-click demo or sample merchant badge experience.
- Current page does not explicitly render the same card visual treatment as the mockup's hero tool widget.

### Visual differences
- Current implementation is more minimal and uses standard shadcn cards.
- Button and input styles are simpler than the mockup’s rounded, gradient-rich form.
- The reference has a wider hero-style section with more whitespace and emphasis on the conversion workflow.

### Refactor complexity
- Low.
- The feature exists already and would mainly require styling updates and optional addition of a merchant/platform preview panel.

### Estimated effort
- 1–2 days for alignment and polish.

---

## Orders

### Current state
- Route: `/dashboard/orders`
- Components:
  - `OrdersPage`
  - `EarningsSummaryCards`
  - `OrdersTable`
  - `OrderFilters`
  - `OrdersPagination`
  - `OrderDetailModal`
- Layout: descriptive heading, summary card section, filter toolbar, responsive orders table, empty states.
- Functionality: order loading, filter by status, search by ID, pagination, detail modal, API loading/error states.

### Missing features
- The reference page is not fully defined for orders, but the current page lacks the exact visual pattern of the mockup’s transaction log card style and status chips.
- There is no direct link to a legacy “recent activity” block from the mockup, though the current page does have order history.

### Visual differences
- Current orders table uses a compact shadcn table and uses muted headings, whereas the mockup shows a softer row/card look with lighter header backgrounds.
- Status badges in current UI are standard badges, whereas the reference uses more distinct background tints and pill-like labels.
- The current page is functionally rich but more utilitarian than the polished marketing-style card grid in the reference.

### Refactor complexity
- Low to medium.
- Most functionality exists; the work is mainly visual refinement of the table and badges, plus possibly aligning the summary section more closely with the reference card style.

### Estimated effort
- 1–3 days.

---

## Withdrawals

### Current state
- Route: `/dashboard/withdrawals`
- Components:
  - `WithdrawalsPage`
  - `WithdrawalForm`
  - `WithdrawalsTable`
- Layout: two-column grid with balance summary and action card, followed by a withdrawal history table.
- Functionality: balance display, bank details form, validation, `Withdraw max` action, submission handling, withdrawal history list.

### Missing features
- The reference has a more explicit balance panel with a green inset accent and alert banner; current UI has a metric card and warning banner but a different visual emphasis.
- The reference’s bank details form has a distinct design language with a more prominent top alert and softer form treatment.
- The current page does not include the exact UI of the reference’s withdrawal summary block and note panel.

### Visual differences
- Current page is more modular and less visually integrated than the reference.
- The form uses generic inputs and standard card sections, while the reference shows a rounded, medicine-box style form with a strong accent area.
- The table and card visual language is more functional and less polished than the mockup.

### Refactor complexity
- Low to medium.
- The core withdrawal flow exists; work is primarily styling and aligning page composition with the reference layout.

### Estimated effort
- 2–3 days.

---

## Admin

### Current state
- Route: `/admin`
- Components:
  - `AdminDashboardPage`
  - `AdminDashboardOverview`
  - `AdminStatisticsCards`
  - `AdminOrdersChart`
  - `AdminRevenueChart`
  - `AdminRecentUsersWidget`
  - `AdminRecentOrdersWidget`
  - `AdminRecentWithdrawalsWidget`
- Layout: sidebar-based admin shell, top header, summary cards, charts, recent widgets, pending actions banner.
- Functionality: admin dashboard overview with multiple data sections and loading/error handling.

### Missing features
- The reference does not explicitly define the admin page, so there is no direct one-to-one feature gap here.
- In terms of UI system consistency, the admin experience should still adopt the same branding, spacing, and card styling as the reference.
- There may be missing admin-specific quick actions or more pronounced admin page hero/status summary visuals.

### Visual differences
- The admin page uses a classic sidebar admin layout and shadcn-style cards, making it look more utilitarian than the mockup’s polished consumer dashboard.
- The navigation and header are functional but do not follow the mockup’s glass blur or top-floating shell aesthetic.
- The current page is consistent with the app’s existing component library, but it is not specifically stylized to match the reference’s fintech brand voice.

### Refactor complexity
- Medium.
- If the goal is to bring admin into the same visual system as the user dashboard, most of the work is styling and layout updates rather than feature rework.

### Estimated effort
- 2–4 days.

---

## Conclusion

The current frontend implements the core app pages and workflows, but it does not yet reflect the reference mockup’s visual identity, page composition, and workspace-style behavior.

Priority areas for alignment:
- Dashboard page layout and workspace composition.
- Navigation model and top shell visual language.
- Form/card styling on the Affiliate and Withdrawals pages.
- Bringing the dashboard and admin experiences into the same branded color, spacing, and motion system.
