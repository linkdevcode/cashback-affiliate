# UI Design Audit — Cashback Affiliate Platform



**Auditor role:** Senior SaaS Product Designer  

**Scope:** All frontend pages (`frontend/src/app/**`) and shared UI components (`frontend/src/components/**`, `frontend/src/components/ui/**`)  

**Date:** June 27, 2026  

**Method:** Static code review of layout, typography tokens, component patterns, and state handling across user dashboard, admin area, auth, and public routes.



**Last updated:** June 27, 2026 — User dashboard, orders page, and withdrawals page refactors applied per `docs/DESIGN.md`.



---



## Executive Summary



The frontend is structurally sound — Shadcn/UI cards, consistent page shells, and React Query loading/error patterns are in place. The **user dashboard (`/dashboard`)**, **orders page (`/dashboard/orders`)**, and **withdrawals page (`/dashboard/withdrawals`)** have been refactored to apply the design system: brand tokens, KPI hierarchy, skeleton loading, shared empty states, mobile navigation, responsive tables, unified status badges, and enhanced filters/pagination.



Remaining gaps are primarily **outside user-facing dashboard flows**: admin table action density, affiliate history page, landing page, locale, and auth loading polish.



---



## Dashboard Resolution Status



The following audit items were **resolved** for `/dashboard` and its feature components:



| # | Issue | Status | Resolution |

|---|-------|--------|------------|

| 1 | No mobile navigation | ✅ Resolved | Bottom tab bar (`dashboard-mobile-nav.tsx`) with route indicators; main content padded for safe area |

| 2 | Static header title | ✅ Resolved | `dashboard-header.tsx` derives title from `dashboard-navigation.ts`; page-level duplicate `<h1>` removed |

| 3 | Data tables desktop-only | ✅ Resolved | `recent-orders-widget.tsx` uses stacked row cards on `< md`, table on `≥ md` |

| 4 | Inverted KPI card hierarchy | ✅ Resolved | `MetricCard` component: label → value → description; Available Balance featured |

| 5 | Broken font token wiring | ✅ Resolved | `globals.css` maps `--font-sans: var(--font-geist-sans)` |

| 6 | Dashboard duplicates order content | ✅ Resolved | Layout follows `docs/DESIGN.md` §10.1: activity feed (narrative) + orders table (tabular) + quick actions; distinct scan patterns |

| 7 | Empty dashboard hides KPI structure | ✅ Resolved | KPI grid always renders at `₫0`; empty banner shown below |

| 8 | No brand color or visual identity | ✅ Resolved | Teal-green brand tokens in `globals.css`; sidebar active state, links, primary buttons |

| 9 | Chart colors are grayscale | ✅ Resolved | `--chart-cashback` brand teal applied in `earnings-chart.tsx` |

| 10 | Inconsistent empty states | ✅ Resolved (dashboard) | Shared `EmptyState` on chart, activity, orders widgets, and empty banner |

| 11 | Weak loading states | ✅ Resolved (dashboard) | Skeleton loaders for KPI cards, chart, activity feed, and orders table |

| 14 | Inconsistent table styling | ✅ Resolved (dashboard widget + orders page) | Full-page orders table aligned to design system spec |



---



## Orders Page Resolution Status



The following audit items were **resolved** for `/dashboard/orders` and its feature components:



| # | Issue | Status | Resolution |

|---|-------|--------|------------|

| 2 | Duplicate `<h1>` on orders page | ✅ Resolved | Page-level `<h1>` removed; header title driven from `dashboard-navigation.ts` |

| 3 | Data tables desktop-only | ✅ Resolved | `orders-table.tsx` uses stacked row cards on `< md`, table on `≥ md` |

| 4 | Inverted KPI card hierarchy | ✅ Resolved | `earnings-summary-cards.tsx` uses `MetricCard` with label → value → description |

| 10 | Inconsistent empty states | ✅ Resolved | Shared `EmptyState` for no-data and filtered views with contextual CTAs |

| 11 | Weak loading states | ✅ Resolved | Skeleton loaders for KPI cards and orders table (desktop + mobile) |

| 12 | No fetch/refetch feedback | ✅ Resolved | Progress bar + opacity overlay + "Updating…" during `isFetching` |

| 14 | Inconsistent table styling | ✅ Resolved | `rounded-xl border`, `bg-muted/40` header, `px-4 py-3` cells, hover rows |

| 15 | Status badge semantics | ✅ Resolved (orders) | Shared `StatusBadge` / `OrderStatusBadge` with semantic design tokens |

| 16 | Filter bars lack grouping | ✅ Resolved | `order-filters.tsx`: labeled search + status chips in `bg-muted/30` container; collapsible on mobile |

| 17 | Pagination is minimal | ✅ Resolved (orders) | `OrdersPagination` shows range ("Showing 1–20 of 143") + numbered page buttons |



Additional orders-specific improvements:

- **Search:** Order ID search input with clear button; client-side filter on current page (no API change)
- **Detail modal:** Flat detail fields (`bg-muted/30`), skeleton loading, mono order ID, prominent status badge
- **Responsive:** Mobile filter toggle, stacked order cards with semantic headings



---



## Withdrawals Page Resolution Status



The following audit items were **resolved** for `/dashboard/withdrawals` and its feature components:



| # | Issue | Status | Resolution |

|---|-------|--------|------------|

| 2 | Duplicate `<h1>` on withdrawals page | ✅ Resolved | Page-level `<h1>` removed; header title driven from `dashboard-navigation.ts` |

| 3 | Data tables desktop-only | ✅ Resolved | `withdrawals-table.tsx` uses stacked row cards on `< md`, table on `≥ md` |

| 10 | Inconsistent empty states | ✅ Resolved | Shared `EmptyState` for no-data and filtered views |

| 11 | Weak loading states | ✅ Resolved | Skeleton loaders for table (desktop + mobile); `MetricCardSkeleton` for balance |

| 12 | No fetch/refetch feedback | ✅ Resolved | Progress bar + opacity overlay + "Updating…" during `isFetching` |

| 14 | Inconsistent table styling | ✅ Resolved | Design-system table spec: `rounded-xl border`, muted header, consistent padding |

| 15 | Status badge semantics | ✅ Resolved (withdrawals) | `WithdrawalStatusBadge`: Pending = warning, Approved = info, Completed = success, Rejected = destructive |

| 16 | Filter bars lack grouping | ✅ Resolved | `withdrawal-filters.tsx`: labeled status chips in `bg-muted/30` container; collapsible on mobile |

| 17 | Pagination is minimal | ✅ Resolved | Reuses enhanced `OrdersPagination` with range + page numbers in `CardFooter` |



Additional withdrawals-specific improvements:

- **Form trust:** Featured `MetricCard` for available balance, processing/security info panel, balance reservation notice
- **Form usability:** Two-column bank layout, "Withdraw max" action, caption-medium labels, mono account number, minimum-balance warning
- **History readability:** Amount as primary metric, reference ID column, status description helper text per row
- **Status visibility:** Semantic badges with contextual descriptions (e.g. "Approved — transfer pending")



---



## Top 20 UI Problems



| # | Problem | Severity | Suggested Fix | Dashboard Status |

|---|---------|----------|---------------|------------------|

| 1 | **No mobile navigation.** Sidebars in `dashboard-sidebar.tsx` and `admin-sidebar.tsx` use `hidden … md:block` with no Sheet, bottom tab bar, or hamburger menu. Below `md`, users cannot reach Orders, Withdrawals, Affiliate Links, or Admin pages except via direct URL. | **Critical** | Add a mobile nav pattern: hamburger + `Sheet` drawer mirroring sidebar links, or a fixed bottom tab bar for the 4–5 primary routes. Show current route indicator and persist across pages. | ✅ User dashboard |

| 2 | **Static header title on every page.** `dashboard-header.tsx` always renders `<h1>Dashboard</h1>` regardless of route (Orders, Withdrawals, etc.), while each page also has its own `<h1>`. This creates duplicate headings and breaks wayfinding. | **High** | Drive header title from route metadata (e.g. a nav config map). Demote page-level heading to `<h2>` or remove redundant title — keep one primary heading per view. | ✅ User dashboard shell |

| 3 | **Data tables are desktop-only.** All tables (`orders-table`, `withdrawals-table`, admin tables) enforce `min-w-[640px]`–`min-w-[960px]` with horizontal scroll only. No responsive card/list fallback for mobile. | **High** | Introduce a shared `DataTable` with a `mobileCardView` breakpoint: stack row fields as label/value pairs on `< sm`. Keep table view for `≥ md`. Prioritize Orders and Admin Withdrawals first. | ✅ User orders + withdrawals pages |

| 4 | **Inverted KPI card hierarchy.** In `dashboard-summary-cards.tsx` and `earnings-summary-cards.tsx`, `CardDescription` (secondary) renders **above** `CardTitle` (primary). Users scan description text before the metric name. | **High** | Swap order: `CardTitle` → metric value → `CardDescription`. Make the currency value the largest element (`text-2xl`/`text-3xl`). Optionally highlight Available Balance with accent border or subtle background. | ✅ Dashboard + orders + withdrawals balance |

| 5 | **Broken font token wiring.** `globals.css` sets `--font-sans: var(--font-sans)` (circular reference). `layout.tsx` loads Geist as `--font-geist-sans` but never maps it to `--font-sans`. Typography may fall back to system fonts. | **High** | Fix token chain: `--font-sans: var(--font-geist-sans)` in `:root` or apply `geistSans.className` on `<body>`. Audit `font-heading` usage in `card.tsx` after fix. | ✅ Global tokens |

| 6 | **Dashboard duplicates order content.** `dashboard-overview.tsx` renders both `RecentActivities` and `RecentOrdersWidget` from the same `recentOrders` array — redundant tables/lists on one screen. | **High** | Keep one widget. Use `RecentActivities` for narrative feed OR `RecentOrdersWidget` for scannable table — not both. Reclaim vertical space for a primary CTA row (Generate link · Withdraw). | ✅ Per DESIGN.md §10.1 |

| 7 | **Empty dashboard hides all KPI structure.** `isDashboardEmpty()` in `dashboard-summary-cards.tsx` replaces the 4-card grid with a single empty-state card. New users lose sight of what metrics they'll eventually see. | **Medium** | Always render the 4 metric cards with `₫0` values and muted styling. Show the illustrated empty state below or as a banner — not instead of the grid. | ✅ Dashboard |

| 8 | **No brand color or visual identity.** Entire theme in `globals.css` is neutral oklch grayscale. Primary, charts, and sidebar share the same monochrome palette. Product is indistinguishable from a generic Shadcn starter. | **Medium** | Define a brand accent (e.g. emerald/teal for cashback growth). Apply to primary actions, Available Balance, chart bars, and active nav item. Keep neutrals for chrome; use color semantically for money and status. | ✅ Global + dashboard |

| 9 | **Chart colors are grayscale.** `--chart-1` through `--chart-5` are gray shades. Bar charts (`earnings-chart.tsx`, admin charts) lack visual differentiation and feel like wireframes. | **Medium** | Assign distinct hues per series (cashback = brand green, orders = blue, revenue = purple). Ensure WCAG contrast for axis labels. Add a legend when multiple series exist. | ✅ User dashboard chart |

| 10 | **Inconsistent empty states.** Only dashboard has icon + headline + CTA (`DashboardEmptyState`). Orders, withdrawals, affiliate history, and admin lists use a single centered `<p>` with muted text — no illustration, no next-step action. | **Medium** | Create a shared `EmptyState` component: icon, title, description, optional CTA. Use context-specific copy and actions (e.g. Orders → "Generate a link"; filtered views → "Clear filters" button). | ✅ Dashboard + orders + withdrawals pages |

| 11 | **Weak loading states — spinner-only.** Tables and charts show centered `Loader2` + text. Summary cards show a spinner inside empty cards rather than skeleton shapes. No layout-preserving placeholders. | **Medium** | Add Shadcn `Skeleton` for cards (title bar + value block), table rows (5–8 shimmer rows), and chart area. Keeps layout stable and reduces perceived load time. | ✅ Dashboard + orders + withdrawals pages |

| 12 | **No fetch/refetch feedback during pagination.** When `isFetching` is true, pagination buttons disable but table content doesn't indicate background refresh — users may think the app is frozen. | **Medium** | Add a subtle top-of-table progress bar or opacity overlay (`opacity-60 pointer-events-none`) while `isFetching`. Optionally show "Updating…" near pagination summary. | ✅ Orders + withdrawals pages |

| 13 | **Admin table action density.** `admin-withdrawals-table.tsx` rows can show View + Approve + Reject (+ Complete) as full labeled buttons. On tablet/mobile this wraps into multi-line action clusters. | **Medium** | Collapse row actions into a `DropdownMenu` (⋯) with labeled items. Keep primary action (Approve) as standalone only on Pending rows. Add confirmation dialogs for destructive actions. | ⏳ Open (admin) |

| 14 | **Inconsistent table styling across surfaces.** Widget tables (`recent-orders-widget.tsx`) use `px-2 py-3`, no header background, no border wrapper. Full-page tables use `px-4 py-3`, `bg-muted/40` thead, and `rounded-lg border`. | **Medium** | Extract shared `Table`, `TableHeader`, `TableRow`, `TableCell` primitives (Shadcn table). Enforce one padding scale (`px-4 py-3`) and one header treatment everywhere. | ✅ Dashboard widget + orders + withdrawals pages |

| 15 | **Status badge semantics differ by domain.** Orders: Approved = emerald. Withdrawals: Approved = blue, Completed = emerald. Users may misread "Approved" withdrawal as "paid/completed." | **Medium** | Align status color language platform-wide: Pending = amber, Success/Completed = emerald, Rejected = rose, In-progress/Approved (not yet paid) = blue. Document in a shared `StatusBadge` component. | ✅ Orders + withdrawals (`StatusBadge`); ⏳ Admin |

| 16 | **Filter bars lack grouping and labels.** Status filters are unlabeled button rows inside card headers. Admin order page stacks search, date range, and status filters with only `gap-4` — visually flat. | **Low** | Add a "Filter by status" label above chip rows. Wrap filter groups in a subtle `bg-muted/30 rounded-lg p-4` container. On mobile, collapse filters into a "Filters" drawer. | ✅ User orders + withdrawals pages; ⏳ Admin |

| 17 | **Pagination is minimal.** All pagination components only offer Previous/Next. No page numbers, jump-to-page, or page-size selector. Poor for admin datasets. | **Low** | Add numbered page buttons (truncate with ellipsis for large sets). Show "Rows per page" select (10/20/50). Display "Showing 1–20 of 143" instead of only "page X of Y." | ✅ Orders + withdrawals pages; ⏳ Admin/affiliate |

| 18 | **Public homepage is empty.** `(public)/page.tsx` returns `null` — visitors see header/footer with blank content. No value proposition, CTA, or trust signals. | **Low** | Build a minimal landing: hero headline, 3-step "How cashback works", Google login CTA, supported merchants (Shopee). Match auth card visual language. | ⏳ Open |

| 19 | **Locale mismatch: English UI, Vietnamese formatting.** Copy is English; `formatCurrency` and `formatDateTime` use `vi-VN`. Mixed locale feels unintentional for a Vietnam-focused product. | **Low** | Decide primary locale. If Vietnamese: translate UI strings. If bilingual: add i18n. Align number/date formatting with chosen language and document in design tokens. | ⏳ Open |

| 20 | **Auth and route loading states are bare.** `protected-route.tsx` and login submitting state show plain "Loading…" / "Signing in…" text with no spinner or branded shell. | **Low** | Use centered logo + spinner inside the same card shell as login. Match dashboard loading polish. Avoid full-screen flash of unstyled text. | ⏳ Open |



---



## Severity Legend



| Level | Definition |

|-------|------------|

| **Critical** | Blocks core tasks or makes the app unusable on common devices |

| **High** | Significantly hurts usability, trust, or comprehension |

| **Medium** | Noticeable friction; fix improves polish and consistency |

| **Low** | Nice-to-have; improves completeness or brand perception |



---



## Positive Patterns Worth Preserving



- **Consistent page shell:** `space-y-6` page wrapper + title + description pattern across user and admin routes.

- **Card-based information architecture:** Logical grouping of forms, tables, and widgets inside Shadcn `Card`.

- **Error handling:** `role="alert"` on error messages; API errors surfaced with user-friendly copy.

- **Form quality:** Withdrawal and affiliate forms have labels, helper text, validation feedback, and disabled states during submit.

- **Affiliate result UX:** Copy button with success feedback in `affiliate-link-result-card.tsx` is a good micro-interaction pattern to replicate elsewhere.

- **Admin dashboard structure:** Statistics cards grouped by domain (users, orders, withdrawals, revenue) with charts and recent-activity widgets follows SaaS admin conventions.

- **Shared design components:** `MetricCard`, `EmptyState`, `StatusBadge`, `Skeleton`, and `dashboard-navigation.ts` config — reuse on remaining pages.



---



## Recommended Fix Priority



### Phase 1 — Unblock mobile & wayfinding (Critical / High)

1. ~~Mobile navigation (Sheet or bottom tabs)~~ ✅ User dashboard

2. ~~Dynamic header titles / fix duplicate `<h1>`~~ ✅ User dashboard shell

3. Responsive table card view — ✅ User orders + withdrawals pages; ⏳ admin pages

4. ~~Fix font token wiring~~ ✅



### Phase 2 — Dashboard & data clarity (High / Medium)

5. ~~KPI card hierarchy + always-visible metric grid~~ ✅

6. ~~Remove duplicate recent orders widgets~~ ✅ Per DESIGN.md

7. ~~Shared `EmptyState` and `Skeleton` components~~ ✅ Created; rolled out to dashboard + orders page

8. ~~Brand accent + chart colors~~ ✅

9. ~~Orders page refactor (filters, pagination, badges, modal, responsive)~~ ✅

10. ~~Withdrawals page refactor (form trust, history table, status badges, responsive)~~ ✅



### Phase 3 — Admin & polish (Medium / Low)

11. Row action menus + confirmation dialogs

12. Shared table primitives and status badge system — ✅ `StatusBadge` created; roll out to admin

13. Enhanced pagination — ✅ User orders + withdrawals; roll out to admin/affiliate

14. Landing page + locale decision



---



## Files Reviewed



**Pages:** `(public)/page.tsx`, `(auth)/login/page.tsx`, `(dashboard)/dashboard/*`, `(admin)/admin/*`  

**Layouts:** `dashboard-layout`, `admin-layout`, `auth-layout`, `public-layout`, sidebars, headers  

**Shared UI:** `button`, `card`, `input`, `dialog`, `chart`, `separator`, `skeleton`, `metric-card`, `empty-state`  

**Feature components:** Dashboard overview, orders/withdrawals/affiliate tables & forms, admin dashboard, users/orders/withdrawals admin tables, filters, modals, pagination  

**Global styles:** `globals.css`, root `layout.tsx`



---



*Dashboard refactor completed June 27, 2026. Orders page refactor completed June 27, 2026. Withdrawals page refactor completed June 27, 2026. See `docs/DESIGN.md` for the authoritative design system.*

