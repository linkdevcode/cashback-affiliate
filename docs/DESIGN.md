# Cashback Affiliate — Design System

Version: 1.0  
Status: Authoritative  
Audience: Product, design, frontend engineering

---

## 1. Product Context

Cashback Affiliate is a **Vietnam-focused fintech product** that helps users earn cashback on Shopee purchases through affiliate links. Users track earnings, monitor order status, and withdraw real money to their bank account. Admins operate payouts and platform health.

The interface must communicate **financial clarity** — users are making decisions about real VND. Every screen should feel precise, calm, and accountable.

---

## 2. Brand Personality

### Essence

**Honest money, made simple.**

The brand is a reliable financial companion — not a flashy marketplace, not a generic admin panel. It earns trust through clarity of numbers, predictable layouts, and restrained visual language.

### Personality Traits

| Trait | Expression in UI |
|-------|------------------|
| **Trustworthy** | Exact currency formatting, explicit status labels, confirmation before destructive actions, no hidden information behind decorative UI |
| **Modern** | Generous whitespace, Geist typography, flat surfaces with hairline borders, purposeful motion only on state changes |
| **Clean** | One primary action per section, no visual clutter, no nested cards, no gradient backgrounds |
| **Fintech** | Numbers are the hero; metrics use tabular figures; charts are minimal; admin tools feel operational, not playful |
| **Approachable** | Plain language, guided empty states, clear next steps — finance without intimidation |

### Voice & Tone

- **Direct:** "Available balance" not "Your wallet overview"
- **Precise:** "₫80.000" not "~80k"
- **Reassuring:** "Awaiting approval" not "Processing…"
- **Action-oriented:** "Generate link" / "Request withdrawal" — verb-first CTAs

### What We Are Not

- A crypto or trading app (no neon, no ticker aesthetics)
- A consumer e-commerce app (no product grids, no promotional banners)
- A generic SaaS dashboard (no purple gradients, no floating card stacks, no decorative illustrations on every page)

---

## 3. Design Principles

1. **Numbers first.** Currency and counts are the largest, highest-contrast elements in any financial view.
2. **Flat hierarchy.** Use spacing and typography — not shadows or nested containers — to create structure.
3. **Color means something.** Color encodes money state (earned, pending, withdrawn) and system status. Never decorative.
4. **One surface per idea.** Each card holds one concept (a metric, a form, a table). Do not nest cards inside cards.
5. **Borders over shadows.** Depth comes from `1px` borders and background tints, not drop shadows.
6. **Consistency builds trust.** Same table treatment, same badge semantics, same spacing rhythm everywhere.

---

## 4. Color Palette

All colors use **OKLCH** for perceptual uniformity. Tokens map to CSS variables in `globals.css` and Tailwind theme.

### 4.1 Brand Colors

| Token | Light Mode | Dark Mode | Usage |
|-------|------------|-----------|-------|
| `--brand` | `oklch(0.52 0.14 168)` | `oklch(0.62 0.14 168)` | Primary brand accent — CTAs, active nav, links, cashback highlights |
| `--brand-foreground` | `oklch(0.99 0 0)` | `oklch(0.13 0.02 168)` | Text on brand surfaces |
| `--brand-muted` | `oklch(0.95 0.03 168)` | `oklch(0.22 0.04 168)` | Subtle brand tint for highlighted KPI (Available Balance) |

**Teal-green (`168°` hue)** was chosen for cashback growth — distinct from Shopee orange and from generic fintech purple, while signaling money earned.

### 4.2 Neutral Foundation

| Token | Light Mode | Dark Mode | Usage |
|-------|------------|-----------|-------|
| `--background` | `oklch(0.985 0.002 168)` | `oklch(0.14 0.01 168)` | Page background — barely warm, not pure white |
| `--foreground` | `oklch(0.18 0.02 168)` | `oklch(0.96 0.005 168)` | Primary text — deep slate, not `#000` |
| `--muted` | `oklch(0.96 0.005 168)` | `oklch(0.22 0.01 168)` | Secondary surfaces, filter bars |
| `--muted-foreground` | `oklch(0.50 0.02 168)` | `oklch(0.65 0.02 168)` | Labels, descriptions, table headers |
| `--border` | `oklch(0.90 0.01 168)` | `oklch(1 0 0 / 12%)` | All dividers and card edges |
| `--input` | `oklch(0.90 0.01 168)` | `oklch(1 0 0 / 15%)` | Input borders |

### 4.3 Semantic Colors

Used **only** for status and feedback — never as page backgrounds.

| Token | Value (Light) | Meaning |
|-------|---------------|---------|
| `--success` | `oklch(0.55 0.14 155)` | Approved, completed, money credited |
| `--success-muted` | `oklch(0.94 0.04 155)` | Success badge background |
| `--warning` | `oklch(0.75 0.14 75)` | Pending, awaiting review |
| `--warning-muted` | `oklch(0.95 0.04 75)` | Pending badge background |
| `--info` | `oklch(0.55 0.12 240)` | In progress (e.g. withdrawal approved, not yet paid) |
| `--info-muted` | `oklch(0.94 0.03 240)` | Info badge background |
| `--destructive` | `oklch(0.55 0.18 25)` | Rejected, errors, irreversible actions |
| `--destructive-muted` | `oklch(0.94 0.04 25)` | Destructive badge background |

### 4.4 Status Badge Mapping (Platform-Wide)

| Status | Badge Color | Applies To |
|--------|-------------|------------|
| Pending | Warning | Orders, withdrawals |
| Approved (not yet paid) | Info | Withdrawals only |
| Approved (earned) | Success | Orders only |
| Completed | Success | Withdrawals |
| Rejected | Destructive | Orders, withdrawals |
| Active | Success | Users |
| Suspended | Destructive | Users |

### 4.5 Chart Colors

| Token | Value | Series |
|-------|-------|--------|
| `--chart-cashback` | `oklch(0.52 0.14 168)` | User earnings, cashback bars |
| `--chart-orders` | `oklch(0.55 0.12 240)` | Order volume |
| `--chart-revenue` | `oklch(0.45 0.02 168)` | Platform revenue (admin) — muted slate-teal |
| `--chart-grid` | `oklch(0.90 0.01 168)` | Cartesian grid lines |

No rainbow palettes. Maximum 2 series per chart in MVP.

### 4.6 Primary Action Mapping

| Role | Light | Usage |
|------|-------|-------|
| Primary button | `--brand` fill | Submit, Generate link, Approve |
| Secondary button | `outline` + `--border` | View, Cancel, filter chips (inactive) |
| Destructive button | `--destructive-muted` fill + `--destructive` text | Reject, Suspend |
| Ghost button | transparent | Header actions, tertiary links |

**Forbidden:** Purple/violet gradients, `box-shadow` elevation on buttons, gradient text.

---

## 5. Typography Scale

### 5.1 Font Families

| Token | Family | Usage |
|-------|--------|-------|
| `--font-sans` | **Geist Sans** | All UI text |
| `--font-mono` | **Geist Mono** | Order IDs, account numbers, API codes |
| `--font-heading` | Geist Sans | Headings — same family, weight differentiation only |

Implementation: map `--font-sans: var(--font-geist-sans)` on `:root`. Apply `font-variant-numeric: tabular-nums` on all currency and count displays.

### 5.2 Type Scale

Base size: **14px (`text-sm`)** — fintech apps prioritize density without sacrificing readability.

| Token | Size / Line Height | Weight | Usage |
|-------|-------------------|--------|-------|
| `display` | 30px / 36px | 600 | Landing hero only |
| `h1` | 24px / 32px | 600 | Page title (one per view) |
| `h2` | 18px / 28px | 600 | Section title, card title |
| `h3` | 16px / 24px | 500 | Subsection, modal title |
| `body` | 14px / 20px | 400 | Default body text |
| `body-medium` | 14px / 20px | 500 | Table cell emphasis, nav items |
| `caption` | 12px / 16px | 400 | Labels, timestamps, helper text |
| `caption-medium` | 12px / 16px | 500 | Form field labels, badge text |
| `metric-xl` | 28px / 32px | 600 | Primary KPI value (Available Balance) |
| `metric-lg` | 24px / 28px | 600 | Secondary KPI values |
| `metric-sm` | 18px / 24px | 600 | Inline stat blocks (admin metrics) |

### 5.3 Hierarchy Rules

```
Page title (h1)
  └── Page description (caption, muted)

Section card
  └── Card title (h2)
  └── Card description (caption, muted)
  └── Content

KPI card
  └── Label (caption-medium, muted)
  └── Value (metric-lg, foreground)
  └── Context (caption, muted)     ← description always BELOW value
```

- **Never** place description text above the metric label.
- Currency values always use `metric-*` tokens with `tabular-nums`.
- Page header and app chrome header must not both use `h1`.

---

## 6. Border Radius System

Base radius: **`--radius: 0.5rem` (8px)** — slightly tighter than default Shadcn for a fintech feel.

| Token | Value | Usage |
|-------|-------|-------|
| `--radius-sm` | 4px | Badges, small chips |
| `--radius-md` | 6px | Buttons (sm), inputs |
| `--radius-lg` | 8px | Buttons (default), inputs, filter bars |
| `--radius-xl` | 12px | Cards, modals, table wrappers |
| `--radius-full` | 9999px | Status pills, avatars |

**Rules:**
- Cards and modals: `rounded-xl` (12px)
- Buttons and inputs: `rounded-lg` (8px)
- Status badges: `rounded-full`
- Do not mix radius sizes within the same component group

---

## 7. Spacing System

Based on a **4px grid**. Use Tailwind spacing scale exclusively — no arbitrary pixel values.

### 7.1 Core Scale

| Token | Value | Usage |
|-------|-------|-------|
| `space-1` | 4px | Icon-to-text gap, badge padding |
| `space-2` | 8px | Inline element gaps, compact lists |
| `space-3` | 12px | Form field internal gap (label → input) |
| `space-4` | 16px | Card internal padding (compact), grid gaps (mobile) |
| `space-6` | 24px | Page section gap, card grid gap, card padding (default) |
| `space-8` | 32px | Major section separation |
| `space-12` | 48px | Empty state vertical padding |

### 7.2 Layout Spacing

| Context | Value |
|---------|-------|
| Page padding | `p-4 sm:p-6` |
| Page section gap | `space-y-6` |
| Card padding | `p-6` (via `--card-spacing: 24px`) |
| Card header → content | `gap-1` (title block) + `24px` section gap |
| Sidebar width | `256px (w-64)` |
| Header height | `64px (h-16)` |
| Table cell padding | `px-4 py-3` |
| Form field stack | `space-y-4` |
| Filter group internal | `gap-3` |
| Filter group → table | `space-y-4` |

### 7.3 Grid Patterns

| Pattern | Classes |
|---------|---------|
| KPI row (4-up) | `grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4` |
| Chart + sidebar | `grid grid-cols-1 gap-6 lg:grid-cols-3` (chart spans 2) |
| Admin stat sections | `grid grid-cols-1 gap-4 xl:grid-cols-2` |
| Form two-column | `grid gap-4 sm:grid-cols-2` |

---

## 8. Card Design System

### 8.1 Philosophy

Cards are **content containers**, not decorative frames. One card = one job. A card holds either a metric, a form, a table, or a chart — never another card.

### 8.2 Anatomy

```
┌─────────────────────────────────────────────┐
│  CardHeader                                 │
│    CardTitle (h2)                           │
│    CardDescription (caption, muted)         │
│    [CardAction — optional link/button]      │
├─────────────────────────────────────────────┤
│  CardContent                                │
│    primary content                          │
├─────────────────────────────────────────────┤
│  CardFooter (optional — actions, pagination)│
└─────────────────────────────────────────────┘
```

### 8.3 Visual Spec

| Property | Value |
|----------|-------|
| Background | `--card` (same as page in light mode — differentiation via border) |
| Border | `1px solid var(--border)` via `ring-1 ring-foreground/10` |
| Shadow | **None** — flat by default |
| Radius | `rounded-xl` (12px) |
| Padding | `24px` (`--card-spacing`) |

### 8.4 Card Variants

#### Standard Card
Default for forms, tables, charts, widgets.

#### KPI / Metric Card
```
Label          ← caption-medium, muted-foreground
₫1.250.000     ← metric-lg, foreground, tabular-nums
Ready to withdraw  ← caption, muted-foreground
```
- No icon required. If used, icon sits **left of label** at 16px, muted — not inside a nested circle container.
- **Featured KPI** (Available Balance): left border `3px solid var(--brand)` + `--brand-muted` background tint. No nested inner card.

#### Filter Card (not a nested card)
Filters live in the **CardHeader** of the table card, separated by a bottom border or `space-y-4` — not wrapped in their own card.

#### Empty State (inside CardContent)
Centered layout: 48px icon (muted) → title (h3) → description (caption) → CTA button. Minimum vertical padding: `py-12`.

### 8.5 Forbidden Patterns

- Card inside Card
- Shadow elevation (`shadow-md`, `shadow-lg`) on cards
- Gradient backgrounds on cards
- Metric displayed inside a bordered sub-box inside a KPI card (use flat layout)

---

## 9. Table Design System

### 9.1 Philosophy

Tables are the **primary data instrument** for orders, withdrawals, links, and admin records. They must be scannable, consistent, and usable on mobile via a responsive fallback — not raw horizontal scroll alone.

### 9.2 Desktop Table (≥ md)

**Wrapper:**
```
rounded-xl border border-border overflow-hidden
```

**Header row:**
| Property | Value |
|----------|-------|
| Background | `--muted` at 40% opacity |
| Text | `caption-medium`, `--muted-foreground` |
| Cell padding | `px-4 py-3` |
| Border | `border-b border-border` |
| Sticky | `sticky top-0 z-10` on full-page tables |

**Body row:**
| Property | Value |
|----------|-------|
| Text | `body`, `--foreground` |
| Cell padding | `px-4 py-3` |
| Row border | `border-b border-border` (last row: none) |
| Hover | `hover:bg-muted/30` |
| Emphasis cells | `body-medium` for IDs, amounts |
| Muted cells | `caption`, `--muted-foreground` for dates |

**Alignment:**
- Text: left
- Currency: left (VND symbol prefix, never right-align unless column is exclusively numeric comparison)
- Actions: right

### 9.3 Mobile Fallback (< md)

Replace table with **stacked row cards** — one card per record, not a scrollable table.

```
┌─────────────────────────────┐
│ Order #AT-12345    [Pending]│
│ Cashback      ₫80.000       │
│ Created       27 Jun 2026   │
│                    [View →] │
└─────────────────────────────┘
```

- Each field: label (caption, muted) + value (body-medium)
- Status badge top-right
- Primary action bottom-right
- Gap between row cards: `space-y-3`
- No horizontal scroll as the only mobile strategy

### 9.4 Widget Tables (dashboard previews)

Dashboard widget tables (Recent Orders) use the **same cell padding and header treatment** as full-page tables. Max 5 rows. "View all →" link in card header.

### 9.5 Row Actions

| Context | Pattern |
|---------|---------|
| User tables (1 action) | Single outline "View" button |
| Admin tables (2+ actions) | Primary action as button + overflow `⋯` menu for secondary/destructive |
| Destructive actions | Require confirmation dialog |

### 9.6 Pagination Bar

Below table, outside tbody — in `CardFooter` or bottom of `CardContent`.

```
Showing 1–20 of 143          [← Prev]  1  2  3  …  8  [Next →]
```

- Text: `caption`, muted
- Buttons: `size="sm"`, outline variant
- Disabled state during fetch; optional thin progress bar at top of table wrapper

### 9.7 Empty & Loading States

| State | Treatment |
|-------|-----------|
| Loading | 6 skeleton rows matching column layout — not centered spinner |
| Empty (no data) | Shared `EmptyState` with contextual CTA |
| Empty (filtered) | "No results" + "Clear filters" ghost button |
| Error | Inline alert above table area, `role="alert"` |

---

## 10. Dashboard Design System

### 10.1 User Dashboard

The user dashboard answers one question: **"How much money do I have, and what happened recently?"**

#### Layout Structure

```
┌──────────────────────────────────────────────────────────┐
│  Page title + description                                │
├──────────────────────────────────────────────────────────┤
│  [Available Balance] [Pending] [Total Cashback] [Withdrawn]  ← KPI row
├──────────────────────────────────────────────────────────┤
│  Quick actions: [Generate link]  [Request withdrawal]  ← action bar (optional)
├───────────────────────────────┬──────────────────────────┤
│  Earnings chart (6 months)    │  Recent activity feed    │
│  (2/3 width)                  │  (1/3 width)             │
├───────────────────────────────┴──────────────────────────┤
│  Recent orders table (max 5 rows)                        │
└──────────────────────────────────────────────────────────┘
```

#### KPI Row Rules

- Always show all 4 cards — even at `₫0` for new users
- **Available Balance** uses Featured KPI treatment (brand left border + tint)
- Values update independently; do not hide the grid behind a single empty-state card
- Show empty-state banner **below** KPI row when all values are zero

#### Chart Panel

- Single bar chart: cashback by month
- Height: `256px (h-64)`
- No chart junk: no legend if single series, minimal grid, no drop shadow on tooltip
- Tooltip: flat border card, `caption` text

#### Activity Feed vs Orders Table

Show **one** recent-order surface — prefer activity feed in sidebar column, full orders table below. Never duplicate the same data in both.

### 10.2 Admin Dashboard

Answers: **"Is the platform healthy and are there items needing action?"**

#### Layout Structure

```
┌──────────────────────────────────────────────────────────┐
│  Page title + description                                │
├────────────────────────────┬─────────────────────────────┤
│  User stats card           │  Order stats card           │
├────────────────────────────┼─────────────────────────────┤
│  Withdrawal stats card     │  Revenue stats card         │
├────────────────────────────┴─────────────────────────────┤
│  Orders chart              │  Revenue chart              │
├──────────────┬─────────────┴──────────────┬──────────────┤
│ Recent users │  Recent orders             │ Recent withdrawals │
└──────────────┴────────────────────────────┴──────────────┘
```

#### Admin Stat Cards

Each stat card contains a **flat grid of metric blocks** — not nested cards.

```
Metric block:
  label   ← caption, muted
  value   ← metric-sm, foreground
```

Metric blocks use `rounded-lg bg-muted/30 px-3 py-2` — tinted background only, no border, no shadow.

#### Action Priority

Admin dashboard should surface **pending withdrawals count** prominently — operational urgency belongs above passive charts.

### 10.3 Page Shell (User & Admin)

Shared chrome:

| Element | Spec |
|---------|------|
| Sidebar | `w-64`, `--sidebar` background, `border-r`. Active item: `--brand-muted` bg + `--brand` text |
| Header | `h-16`, `border-b`, dynamic page title (not static "Dashboard") |
| Main | `flex-1 p-4 sm:p-6`, `--background` |
| Mobile nav | Bottom tab bar (user) or hamburger sheet (admin) — sidebar alone is insufficient |

### 10.4 Auth & Public Surfaces

| Surface | Spec |
|---------|------|
| Auth card | Single centered card, `max-w-md`, no sidebar. Brand name + one-line description + Google sign-in |
| Landing page | Full-width sections on `--background`. Hero + 3-step flow + CTA. No card grid hero |
| Background | `--background` or `--muted/40` — never gradient |

---

## 11. Component Quick Reference

### Buttons

| Variant | When |
|---------|------|
| `default` (brand fill) | Primary action — one per visible section |
| `outline` | Secondary actions, inactive filters |
| `ghost` | Tertiary, navigation links, header actions |
| `destructive` | Reject, suspend — always paired with confirmation |

Sizes: default `h-9` for forms, `sm` for table actions and filters.

### Inputs

- Height: `h-9`, radius `rounded-lg`
- Always paired with visible `<label>` — `caption-medium`
- Helper text: `caption`, muted, below input
- Error: `--destructive` border + message

### Dialogs / Modals

- Max width: `max-w-lg` (user detail), `max-w-2xl` (admin detail with more fields)
- Flat: border + no shadow or `shadow-sm` only
- Structure: `DialogHeader` → `DialogContent` → `DialogFooter`
- Detail fields: label (caption) + value in `rounded-lg bg-muted/30 px-3 py-2` — not nested cards

### Status Badges

```
inline-flex rounded-full px-2.5 py-0.5 caption-medium
background: --{semantic}-muted
text: --{semantic} (darkened for contrast)
```

### Icons

- Library: Lucide
- Size: 16px (`size-4`) inline, 20px (`size-5`) in buttons, 24px (`size-6`) in empty states
- Color: inherit or `--muted-foreground` — never brand color on decorative icons

---

## 12. Motion & Feedback

| Interaction | Motion |
|-------------|--------|
| Button press | `active:translate-y-px` (already in button component) |
| Page transition | None in MVP |
| Modal open | Fade in only — `fade-in-0`, no scale bounce |
| Copy success | Icon swap Check ↔ Copy, 2s revert |
| Toast notifications | Slide in from bottom-right for success/error after mutations |
| Loading | Skeleton shimmer preferred over spinner for layout-bound content |

Keep motion functional. No decorative animations on financial numbers.

---

## 13. Accessibility

- Minimum contrast: WCAG AA (4.5:1 body text, 3:1 large text/UI components)
- Focus ring: `ring-2 ring-brand/50` on interactive elements
- Currency announced with locale: `vi-VN`, VND
- Status badges include text label — never color alone
- Tables: `<th scope="col">`, responsive cards use semantic headings
- Dialogs: trap focus, `aria-labelledby` on title

---

## 14. Implementation Notes

### CSS Variable Additions (target `globals.css`)

```css
:root {
  --brand: oklch(0.52 0.14 168);
  --brand-foreground: oklch(0.99 0 0);
  --brand-muted: oklch(0.95 0.03 168);
  --success: oklch(0.55 0.14 155);
  --success-muted: oklch(0.94 0.04 155);
  --warning: oklch(0.75 0.14 75);
  --warning-muted: oklch(0.95 0.04 75);
  --info: oklch(0.55 0.12 240);
  --info-muted: oklch(0.94 0.03 240);
  --font-sans: var(--font-geist-sans);
  --radius: 0.5rem;
  --primary: var(--brand);
  --primary-foreground: var(--brand-foreground);
}
```

### Shared Components to Introduce

| Component | Purpose |
|-----------|---------|
| `StatusBadge` | Unified status colors across orders, withdrawals, users |
| `MetricCard` | KPI card with correct label → value → description hierarchy |
| `EmptyState` | Icon + title + description + optional CTA |
| `DataTable` | Desktop table + mobile card fallback |
| `PageHeader` | Title + description + optional action slot |
| `FilterBar` | Grouped filters with label, muted background strip |

### Alignment with Existing Audit

This system directly addresses findings in `DESIGN_AUDIT.md`: mobile navigation, KPI hierarchy, font tokens, empty/loading states, table consistency, status semantics, and brand differentiation.

---

## 15. Do / Don't Summary

| Do | Don't |
|----|-------|
| Teal-green brand accent on money actions | Purple gradients or violet primary |
| Flat cards with hairline borders | Drop shadows and floating card stacks |
| One card per concept | Nested cards |
| Tabular nums on all currency | Approximate amounts ("~80k") |
| Label → value → description in KPIs | Description above metric name |
| Mobile row cards for tables | Horizontal scroll as only mobile pattern |
| Skeleton loaders for tables and KPIs | Full-area spinners that collapse layout |
| Semantic color for status only | Decorative color blocks |
| Geist Sans throughout | Mixed font families |
| Confirmation on reject/suspend | Instant destructive actions |

---

*This document is the source of truth for visual design. Update it when introducing new patterns, tokens, or components.*
