# Plan: Multi-Project Refactor with Spectre.Console + Future API/Next.js

> **Status**: Phase 1, 2 & 3.1 Complete

## Goal
Restructure the project to support multiple frontends (Console, Web API, Next.js) sharing the same core logic.

## Target Structure
```
Abstract-Factory/
├── Abstract Factory.sln              (updated to include all projects)
├── docs/
│   └── PLAN.md                       (this file - for tracking)
├── src/
│   ├── AbstractFactory.Core/         (class library - all business logic)
│   │   ├── Interfaces/
│   │   ├── Factories/
│   │   ├── Products/
│   │   ├── Services/
│   │   └── AbstractFactory.Core.csproj
│   ├── AbstractFactory.Console/      (console app with Spectre.Console)
│   │   ├── Program.cs
│   │   └── AbstractFactory.Console.csproj
│   ├── AbstractFactory.Api/          (future - ASP.NET Core minimal API)
│   └── frontend/                     (future - Next.js app)
├── AbstractFactory/                  (old folder - can be removed)
```

## Phase 1: Create Core Class Library - COMPLETE

### Step 1.1: Create `src/AbstractFactory.Core/` project
- [x] Create new class library targeting net9.0
- [x] Move these folders/files:
  - [x] `Interfaces/` (ICardFactory.cs, ICreature.cs, ISpell.cs)
  - [x] `Factories/` (RedDeckFactory.cs, BlueDeckFactory.cs)
  - [x] `Products/` (RedCreature.cs, BlueCreature.cs, RedSpell.cs, BlueSpell.cs)
  - [x] `Services/` (MtgCardLookup.cs)
- [x] Update namespace to `AbstractFactory.Core.*`

## Phase 2: Create Console App with Spectre.Console - COMPLETE

### Step 2.1: Create `src/AbstractFactory.Console/` project
- [x] Console app targeting net9.0
- [x] Add NuGet reference: `Spectre.Console`
- [x] Reference `AbstractFactory.Core`

### Step 2.2: Implement Interactive Console UI
Features:
- [x] Selection prompt for deck color (arrow keys)
- [x] Text prompts for card names with validation
- [x] Rich table output for card details
- [x] Color-coded output matching deck theme (red/blue)
- [x] Option to create multiple cards in a loop

## Phase 3: Future - API + Next.js (not implemented yet)

### Step 3.1: Create `src/AbstractFactory.Api/` - COMPLETE
- [x] ASP.NET Core minimal API
- Endpoints:
  - `GET /api/decks` - list available deck types
  - `POST /api/cards/creature` - create creature card
  - `POST /api/cards/spell` - create spell card
- [x] CORS configured for Next.js dev server

### Step 3.2: Create `src/frontend/` (Next.js)
- [ ] Next.js 14+ with App Router
- [ ] TypeScript
- [ ] Tailwind CSS for styling
- [ ] Pages: deck selection, card creator, card display

---

## How to Run

### Console App
```bash
dotnet run --project src/AbstractFactory.Console
```

### Build All
```bash
dotnet build
```

---

## Verification

1. **Build**: `dotnet build` from solution root
2. **Run Console**: `dotnet run --project src/AbstractFactory.Console`
3. **Test flow**:
   - Select Red deck with arrow keys
   - Enter "Goblin Guide" for creature
   - Enter "Lightning Bolt" for spell
   - Verify card data displays in formatted table

---

## Dependencies

### AbstractFactory.Core
- No external dependencies (uses built-in System.Net.Http.Json)

### AbstractFactory.Console
- `Spectre.Console` v0.49.1
- Project reference to `AbstractFactory.Core`

### AbstractFactory.Api (future)
- Project reference to `AbstractFactory.Core`
- Built-in ASP.NET Core (no extra packages)

### frontend (future)
- Next.js 14+
- TypeScript
- Tailwind CSS
