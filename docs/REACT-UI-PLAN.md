# MTG Card Lookup Frontend UI Plan

## Context
The API and frontend scaffold are in place but the frontend is still the default Next.js template. This plan builds the actual MTG card lookup UI and adds card image support through the full stack.

## Color Scheme
- **Background**: Dark grey/navy (`#1a1a2e`)
- **Surface**: Lighter grey for panels (`#2d2d44`)
- **Orange**: Primary accent for trim, buttons, borders (`#e67e22`)
- **Purple**: Secondary accent for highlights (`#9b59b6`)

## Changes (in order)

### 1. Add image URL support to Core library
**`src/AbstractFactory.Core/Services/MtgCardLookup.cs`**
- Add `ScryfallImageUris` nested class to deserialize `image_uris` from Scryfall (small, normal, large, etc.)
- Add `[JsonPropertyName("image_uris")]` property to `ScryfallCardResponse`
- Add `ImageUrl` property to `CardData`
- Map `response.ImageUris?.Normal` in `GetCardByName`

### 2. Add `GetImageUrl()` to interfaces
**`src/AbstractFactory.Core/Interfaces/ICreature.cs`** and **`ISpell.cs`**
- Add `string GetImageUrl();` to both interfaces

### 3. Implement in all 4 product classes
**`RedCreature.cs`**, **`BlueCreature.cs`**, **`RedSpell.cs`**, **`BlueSpell.cs`**
- Add `_imageUrl` field, assign from `cardData.ImageUrl ?? ""`, implement `GetImageUrl()`

### 4. Update API responses
**`src/AbstractFactory.Api/Program.cs`**
- Add `ImageUrl` to `CreatureResponse` and `SpellResponse` records
- Add `ImageUrl = creature.GetImageUrl()` / `spell.GetImageUrl()` in endpoint handlers

### 5. Frontend files

**`src/frontend/next.config.ts`** — Whitelist `cards.scryfall.io` for Next.js Image component

**`src/frontend/src/app/globals.css`** — Replace with dark theme using CSS custom properties + Tailwind v4 `@theme inline`

**`src/frontend/src/app/layout.tsx`** — Update metadata title to "MTG Card Lookup"

**`src/frontend/src/app/page.tsx`** — Replace entirely with:
- `"use client"` component with `useState` for form state
- **Form section**: deck color checkboxes (mutually exclusive toggle), card name text input, creature/spell radio buttons, submit button
- **Results section**: card image (Next.js `<Image>`) on left, card details on right (name, mana cost, power/toughness if creature, keywords, card text, deck color)
- Loading and error states
- All styled with Tailwind utility classes using the custom theme colors

## UI Layout
```
┌─────────────────────────────────────┐
│  MTG Card Lookup (header)           │
├─────────────────────────────────────┤
│  Form Panel (bg-surface)            │
│  ┌─────────┐ ┌──────────────────┐   │
│  │ [x] Red │ │ Card Name: _____ │   │
│  │ [ ] Blue│ │                  │   │
│  └─────────┘ └──────────────────┘   │
│  (o) Creature  ( ) Spell            │
│  [ Look Up Card ]                   │
├─────────────────────────────────────┤
│  Results Panel                      │
│  ┌──────────┐  ┌──────────────────┐ │
│  │          │  │ Name: ...        │ │
│  │  Card    │  │ Mana Cost: ...   │ │
│  │  Image   │  │ P/T: ...         │ │
│  │          │  │ Keywords: ...    │ │
│  │          │  │ Text: ...        │ │
│  └──────────┘  └──────────────────┘ │
└─────────────────────────────────────┘
```

## Verification
1. `dotnet build` from solution root — confirms Core + API compile
2. Start API: `dotnet run --project src/AbstractFactory.Api`
3. Test image URL in Swagger: POST `/api/cards/creature` with `{"deckColor":"red","cardName":"Goblin Guide"}` — response should include `imageUrl`
4. Start frontend: `cd src/frontend && npm run dev`
5. Open `http://localhost:3000`, select Red, type "Lightning Bolt", select Spell, click Look Up Card — verify card image + details display
