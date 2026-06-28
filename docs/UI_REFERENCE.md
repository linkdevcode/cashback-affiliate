# UI Reference — SmartCashback Premium Dashboard

This document is the single source of truth for all future UI work related to the SmartCashback premium dashboard concept. It is derived from the provided mockup and should be used as the baseline for layout, styling, interaction, and visual tone.

## 1. Design principles

### Core principles
- Clarity over decoration: financial UI should feel simple, trustworthy, and easy to scan.
- Premium but calm: the interface should feel polished and modern without being flashy or noisy.
- Trust and security first: use restrained visual language, stable structure, and clear confirmation states.
- Minimal friction: actions such as link conversion, wallet review, and withdrawal should feel immediate and guided.
- Consistency across states: landing, authenticated workspace, and transactional views all share the same visual system.

### Visual tone
- Clean, airy spacing with strong hierarchy.
- Neutral base surfaces with a single accent color for emphasis.
- Soft rounded corners and subtle shadowing for depth.
- Dark mode support is part of the system, not an afterthought.

## 2. Layout system

### Page structure
- Use a soft page background of slate-50 in light mode and darkBg-950 in dark mode.
- Wrap content in a centered max-width container with generous horizontal padding:
  - `max-w-7xl mx-auto px-4 sm:px-6 lg:px-8`
- The layout uses a vertical flow with a sticky/glass navigation bar at the top and a footer at the bottom.

### Grid system
- Use a 12-column grid for dashboard sections on large screens.
- Common patterns:
  - 2-column split for primary actions and summary panels.
  - 3-column grid for cards or trust metrics.
  - 1-column stack on mobile, 2-column or 3-column on tablet/desktop.

### Spacing scale
- Use Tailwind spacing values consistently:
  - `p-4`, `p-5`, `p-6` for card padding
  - `gap-3`, `gap-4`, `gap-6` for internal layout spacing
  - `py-12`, `py-16`, `py-20` for section spacing

### Container styling
- Cards should use:
  - `bg-white dark:bg-darkBg-900`
  - `border border-slate-200/50 dark:border-slate-800/50`
  - `rounded-2xl` or `rounded-3xl`
  - `shadow-sm`

## 3. Navigation structure

### Global navigation
- Top bar is fixed and glassmorphism-based.
- Brand icon and name remain on the left.
- Right-side actions include theme toggle and authentication state switch.

### Landing page navigation
- Public landing view shows sections:
  - Hero
  - Trust metrics
  - Merchant partners
  - How-it-works
- Primary call-to-action: “Bắt đầu nhận hoàn tiền”

### App navigation
- Once authenticated, the top navigation switches to an app-style tab system with three tabs:
  - `Không gian hoàn tiền`
  - `Tỷ lệ chiết khấu`
  - `Yêu cầu rút tiền`
- Tabs are visually highlighted when active with a white/dark surface and subtle shadow.

### Sub-view behavior
- Each tab loads a distinct workspace sub-view.
- Switching views should feel lightweight and fast, with a brief skeleton state before content appears.

## 4. Color palette

### Primary brand color
- Emerald green is the main accent color for actions, success indicators, and trust cues.
- Brand values:
  - `#10b981` (brand-500)
  - `#059669` (brand-600)
  - `#047857` (brand-700)
  - `#022c22` (brand-950)

### Core neutral palette
- Use slate as the foundation for text, backgrounds, borders, and surfaces.
- Recommended base values:
  - `#f8fafc` or `#f1f5f9` for light surfaces
  - `#0f172a` / `#020617` for dark surfaces
  - `#334155` for muted text and borders
  - `#64748b` for secondary text

### Supporting accent colors
- Amber: pending / caution states
  - `#f59e0b`
- Blue: informational states
  - `#3b82f6`
- Rose: destructive or rejected states
  - `#ef4444`
- Orange: e-commerce / Shopee identity
  - `#f97316`
- Sky: travel / content related states
  - `#0ea5e9`
- Purple: premium or SaaS utility states
  - `#8b5cf6`

### Usage rules
- Emerald = success, completion, positive financial outcomes.
- Slate = neutral surfaces and body text.
- Amber = pending or transitional states.
- Rose = errors, rejections, or caution.

## 5. Typography

### Font families
- Primary UI font: `Inter`
- Display / hero heading font: `Outfit`

### Type scale
- Hero title: large, bold, tracking-tight, high-contrast
- Section titles: medium-large, bold, display font
- Card titles: small uppercase labels with strong weight
- Body text: readable, compact, muted

### Font weights
- Use `font-light`, `font-normal`, `font-medium`, `font-semibold`, `font-bold`, `font-extrabold` intentionally.
- Keep headings bold and text lighter to preserve hierarchy.

### Text treatment
- Use uppercase small labels for metadata and section headers.
- Keep body copy concise and direct; financial UI should not feel verbose.
- Use monospace for IDs, account numbers, and generated links where needed.

## 6. Card styles

### Default card
- Use a rounded container with:
  - light or dark background
  - subtle border
  - soft shadow
  - padding `p-5` or `p-6`

### Card behavior
- Hover states should be subtle, not heavy.
- Border color can shift slightly on hover for interactive emphasis.
- Use card sections to group actions, summaries, and status blocks.

### Card variants
- Summary card: compact, single-purpose block for KPI or wallet value.
- Action card: used for form entry or input-driven workflows.
- Feature card: used on marketing and partner sections with a strong icon and supporting copy.

### Visual pattern
- Cards should feel elevated but not “boxed” aggressively.
- Prefer `rounded-2xl`, `border`, and `shadow-sm` over hard shadows or deep bevels.

## 7. Table styles

### Table structure
- Use lightweight tables for transaction logs and payment history.
- Tables should be horizontally scrollable on smaller screens.

### Header styling
- Header rows use muted slate text, slight background tint, and uppercase labels.
- Align text thoughtfully; numeric and status columns should be visually distinct.

### Row styling
- Rows have subtle hover states.
- Divider lines between rows should remain soft and understated.

### Status indicators
- Status values appear as compact pill badges.
- Common statuses:
  - Approved = emerald text / emerald background tint
  - Pending = amber text / amber background tint
  - Rejected = rose text / rose background tint

## 8. Form styles

### Inputs and selects
- Use rounded input surfaces with a light neutral background and subtle border.
- Focus states should use a thin emerald ring and visible focus outline.
- Fields should feel clean, minimal, and consistent with the rest of the dashboard.

### Form layout
- Group related controls vertically with small spacing between them.
- Labels should be concise and descriptive.
- Use a strong primary button for the main action and a secondary surface for supporting actions.

### Button patterns
- Primary action: emerald background with dark text for contrast.
- Secondary action: white or slate surface with border.
- Destructive or caution actions should use rose-colored emphasis.

## 9. Empty states

Empty or no-result states should feel intentional and calm.

### Empty-state pattern
- Use a centered message with neutral text.
- Keep the copy concise and non-technical.
- Avoid empty white space; provide a helpful explanation and optional next action.

### Example tone
- “Không tìm thấy đơn hàng đối soát nào.”
- Use friendly, system-like language rather than blunt error phrasing.

## 10. Loading states

### Skeleton loading
- Use simple pulsing placeholders for content that is loading.
- Skeleton panels should match the size and shape of the actual content.
- Apply `animate-pulse` to cards and panel blocks.

### Transition behavior
- Switching workspace views should show a brief skeleton state before content appears.
- Loading should feel fast and lightweight, around 400ms for mock interactions.

## 11. Animation guidelines

### Motion principles
- Motion should be subtle, purposeful, and short.
- Prefer smooth transitions over dramatic movement.
- Animation should support hierarchy, not distract from the content.

### Recommended transitions
- Use `transition-all duration-300` for general UI interactions.
- Use `transition-colors` for hover and focus states.
- Use `animate-pulse` for skeleton loading.
- Use short fade or translate-based transitions for toast notifications and panel reveals.

### Motion rules
- Keep transitions fast: roughly 150–300ms.
- Avoid large movement or bouncing effects.
- Respect reduced-motion preferences where possible in future implementation.

## 12. Implementation guidance for future UI work

- Prefer Tailwind utility classes over custom CSS when matching the mockup.
- Preserve the current visual hierarchy: strong headings, calm surfaces, emerald emphasis, and clear labels.
- Treat the dashboard as a trust-first fintech interface, not a generic SaaS product.
- Reuse existing patterns for cards, forms, tables, and status badges rather than introducing new visual systems.
- Keep dark mode support consistent in every new component.
