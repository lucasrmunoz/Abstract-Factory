# Abstract Factory Pattern - MTG Card Lookup

A C# implementation of the **Abstract Factory** design pattern using Magic: The Gathering (MTG) cards as the domain model. This project demonstrates how to create families of related objects without specifying their concrete classes, with real-time card data fetched from the **Scryfall API**.

## What is the Abstract Factory Pattern?

The Abstract Factory pattern provides an interface for creating families of related or dependent objects without specifying their concrete classes. In this implementation:
- **Factory families**: Red deck and Blue deck factories
- **Product families**: Creatures and Spells for each color
- **Runtime selection**: User chooses which factory to use
- **Client decoupling**: Client code works with any factory through interfaces

## Why MTG Cards?

Magic: The Gathering provides a perfect domain model because:
- Cards naturally group into **color families** (Red, Blue, Black, Green, White)
- Each color has distinct **characteristics** (Red = aggressive, Blue = control)
- Real **API availability** for dynamic card lookup
- Rich **card data** (name, cost, power/toughness, keywords, text)

## Implementation Status

- **Scryfall API Integration**: Using HttpClient to fetch real card data
- **User Input**: Dynamic deck color and card name selection
- **API Endpoint**: https://api.scryfall.com/cards/named (with fuzzy search)
- **Card Details Displayed**: Name, Mana Cost, Power/Toughness, Keywords, Card Text
- **Modern Card Coverage**: Includes ALL sets including Universes Beyond (Final Fantasy, Lord of the Rings, etc.)
- **Available Factories**: Red (Aggressive) and Blue (Control) deck factories

---

## Project Structure

```
AbstractFactory/
├── Interfaces/
│   ├── ICardFactory.cs       # Abstract Factory interface
│   ├── ICreature.cs          # Abstract Product (Creatures)
│   └── ISpell.cs             # Abstract Product (Spells)
├── Factories/
│   ├── RedDeckFactory.cs     # Concrete Factory (Red)
│   └── BlueDeckFactory.cs    # Concrete Factory (Blue)
├── Products/
│   ├── RedCreature.cs        # Concrete Product
│   ├── RedSpell.cs           # Concrete Product
│   ├── BlueCreature.cs       # Concrete Product
│   └── BlueSpell.cs          # Concrete Product
├── Services/
│   └── MtgCardLookup.cs      # MTG API helper
└── Program.cs                # Client code
```

---

## How the Pattern Works

### Abstract Factory Pattern Components

| Component | Implementation | Purpose |
|-----------|---------------|---------|
| **Abstract Factory** | [ICardFactory.cs](AbstractFactory/Interfaces/ICardFactory.cs) | Declares methods for creating abstract products |
| **Concrete Factories** | [RedDeckFactory.cs](AbstractFactory/Factories/RedDeckFactory.cs), [BlueDeckFactory.cs](AbstractFactory/Factories/BlueDeckFactory.cs) | Creates specific product families |
| **Abstract Products** | [ICreature.cs](AbstractFactory/Interfaces/ICreature.cs), [ISpell.cs](AbstractFactory/Interfaces/ISpell.cs) | Defines interfaces for product types |
| **Concrete Products** | [RedCreature.cs](AbstractFactory/Products/RedCreature.cs), [BlueSpell.cs](AbstractFactory/Products/BlueSpell.cs), etc. | Implements actual card logic with API integration |

### Key Design Benefit

The client code in [Program.cs](AbstractFactory/Program.cs) (`DemonstrateFactory()` method) works with **ANY factory** through the `ICardFactory` interface. It doesn't know which concrete factory or products it's using - this is **runtime polymorphism** in action.

```csharp
// Client code doesn't know if it's using Red or Blue factory
ICardFactory factory = userChoice == "2" ? new BlueDeckFactory() : new RedDeckFactory();
DemonstrateFactory(factory, creatureName, spellName);
```

---

## User Interaction Flow

### Step 1: Choose Deck Color
```
Choose your deck color:
1. Red (Aggressive)
2. Blue (Control)

Enter choice (1 or 2): 1
```

### Step 2: Enter Creature Name
```
Enter creature card name (e.g., 'Goblin Guide', 'Snapcaster Mage'): Goblin Guide
```

### Step 3: Enter Spell Name
```
Enter spell card name (e.g., 'Lightning Bolt', 'Counterspell'): Lightning Bolt
```

### Step 4: View Results
```
Fetching cards from MTG API...

📦 Looking up creature: 'Goblin Guide'

╔═══ RED CREATURE ═══
║ Name: Goblin Guide
║ Cost: {R}
║ Power/Toughness: 2/2
║ Keywords: Haste
║ Text: Haste
Whenever Goblin Guide attacks, defending player reveals the top
card of their library. If it's a land card, that player puts it
into their hand.
╚════════════════════

📦 Looking up spell: 'Lightning Bolt'

╔═══ RED SPELL ═══
║ Name: Lightning Bolt
║ Cost: {R}
║ Type: Instant
║ Text: Lightning Bolt deals 3 damage to any target.
╚═════════════════
```

---

## How to Run

### Prerequisites
- .NET 10 SDK or later
- Internet connection (for MTG API calls)

### Build
```bash
dotnet build
```

### Run Interactively
```bash
dotnet run --project AbstractFactory
```

### Quick Test Examples

**Test Red deck:**
```bash
echo -e "1\nGoblin Guide\nLightning Bolt" | dotnet run --project AbstractFactory
```

**Test Blue deck:**
```bash
echo -e "2\nSnapcaster Mage\nCounterspell" | dotnet run --project AbstractFactory
```

---

## Sample Card Names to Try

### Red Creatures
- **Goblin Guide** - Classic 2/1 haste creature
- **Monastery Swiftspear** - Prowess aggro staple
- **Dragon's Rage Channeler** - Modern Horizons powerhouse
- **Ragavan, Nimble Pilferer** - Legendary monkey pirate

### Red Spells
- **Lightning Bolt** - The iconic 3 damage for 1 mana
- **Lava Spike** - Direct burn spell
- **Skullcrack** - Damage + can't gain life
- **Boros Charm** - Versatile multicolor spell

### Blue Creatures
- **Snapcaster Mage** - Modern staple with flashback
- **Delver of Secrets** - Iconic flip creature
- **Brazen Borrower** - Adventure creature/spell
- **Murktide Regent** - Delve flyer

### Blue Spells
- **Counterspell** - Classic counter magic
- **Mana Leak** - Conditional counter
- **Force of Will** - Free counter spell
- **Cryptic Command** - Modal powerhouse

### Universes Beyond (Final Fantasy, Lord of the Rings, etc.)
- **Emet-Selch of the Third Seat** - Final Fantasy Commander legendary creature
- **Gandalf the White** - Lord of the Rings legendary creature
- **Sauron, the Dark Lord** - Lord of the Rings legendary creature
- **The One Ring** - Powerful artifact from LotR set

---

## Scryfall API Integration

### API Details
- **Endpoint**: <https://api.scryfall.com/cards/named>
- **Method**: GET with query parameter `?fuzzy={cardName}`
- **Response**: Single JSON card object with comprehensive data
- **Documentation**: <https://scryfall.com/docs/api>
- **Rate Limit**: 10 requests per second (free, no authentication required)

### Benefits Over Previous API
- **Complete Coverage**: Includes ALL modern sets (Universes Beyond, Secret Lairs, etc.)
- **Fuzzy Search**: Handles typos and partial names automatically
- **Active Maintenance**: Regularly updated with new releases
- **Reliability**: Better uptime and performance
- **Rich Data**: Includes colors, legality, pricing, and more

### How It Works

1. User enters card name (e.g., "Goblin Guide" or even "Gobln Gide")
2. `MtgCardLookup.GetCardByName()` makes HTTP request to Scryfall's fuzzy endpoint
3. Scryfall returns single best match with complete card data (name, mana_cost, type_line, oracle_text, power, toughness, colors)
4. Service maps Scryfall's snake_case properties to CardData object
5. Product class (e.g., `RedCreature`) stores the fetched data in fields
6. `DisplayDetails()` method formats and displays the card information

### Code Flow
```csharp
// In RedCreature.cs constructor
var cardData = MtgCardLookup.GetCardByName(cardName).Result;
if (cardData != null)
{
    _name = cardData.Name ?? cardName;
    _manaCost = cardData.ManaCost ?? "N/A";
    _power = cardData.Power ?? "?";
    _toughness = cardData.Toughness ?? "?";
    _text = cardData.Text ?? "No card text available";
}
```

### Property Mapping
Scryfall uses snake_case, we map to PascalCase:
- `type_line` → `Type`
- `oracle_text` → `Text`
- `mana_cost` → `ManaCost`

### Error Handling
- **Card not found**: Displays "Card not found in database"
- **API error**: Shows "Error fetching card: [error message]"
- **Empty input**: Uses default values (Goblin for Red, Merfolk for Blue)
- **Network issues**: Gracefully degrades with error message
- **Ambiguous results**: Scryfall returns best fuzzy match

---

## Abstract Factory Pattern Benefits

This implementation demonstrates key benefits of the Abstract Factory pattern:

**Client Decoupling**
- `DemonstrateFactory()` works with ANY factory through `ICardFactory`
- No dependency on concrete `RedDeckFactory` or `BlueDeckFactory` classes
- Easy to swap factories at runtime

**Runtime Selection**
- Factory chosen dynamically based on user input
- No compile-time binding to specific factory
- Demonstrates true polymorphism

**Family Consistency**
- Red factory **only** creates Red products (RedCreature, RedSpell)
- Blue factory **only** creates Blue products (BlueCreature, BlueSpell)
- Prevents mixing incompatible product families

**Extensibility**
- New factories (Black, Green, White) can be added without modifying existing code
- No changes needed to interfaces or client code
- Follows Open/Closed Principle (open for extension, closed for modification)

---

## Future Extensions

### Adding a New Color (Example: Black)

To add a Black deck factory:

1. **Create concrete products**:
   - `BlackCreature.cs` implementing `ICreature`
   - `BlackSpell.cs` implementing `ISpell`

2. **Create concrete factory**:
   - `BlackDeckFactory.cs` implementing `ICardFactory`

3. **Update client code**:
   - Add option "3. Black (Removal)" in `Program.cs`
   - No changes to interfaces or existing factories!

### Advanced Features

**Caching**
- Cache API responses to reduce network calls
- Implement `IMemoryCache` for frequently looked-up cards

**Configuration**
- Use `appsettings.json` for factory selection
- Environment-based factory selection

**Factory Registry**
- Dictionary-based factory lookup: `factories["Red"]`
- Load factories dynamically via reflection

**Dependency Injection**
- Register factories in DI container
- Inject `ICardFactory` into services

**Multi-color Support**
- Create composite factories for multicolor decks
- Support hybrid mana costs

**Deck Building**
- Build full 60-card decks
- Validate deck legality (format, card limits)

---

## Design Patterns Applied

### Primary Pattern: Abstract Factory
- **Purpose**: Create families of related objects without specifying concrete classes
- **Benefits**:
  - Decouples client from concrete implementations
  - Ensures product family consistency
  - Makes code extensible and maintainable

### Supporting Pattern: Repository Pattern
- **Location**: [Services/MtgCardLookup.cs](AbstractFactory/Services/MtgCardLookup.cs)
- **Purpose**: Separates data access logic from business logic
- **Benefits**:
  - Abstracts API communication
  - Makes testing easier (can mock API calls)
  - Centralizes data retrieval logic

---

## Technical Details

### Dependencies
- **No external NuGet packages required** for core pattern
- Uses built-in `System.Net.Http.Json` for API calls
- Uses `System.Text.Json` for JSON deserialization
- .NET 10 SDK

### C# Features Demonstrated

**Interfaces and Polymorphism**
- `ICardFactory`, `ICreature`, `ISpell` interfaces
- Runtime polymorphism with factory selection

**Async/Await**
- `MtgCardLookup.GetCardByName()` is async
- Uses `.Result` for synchronous execution (simplified for demo)

**Null-Coalescing Operator**
- `cardData.Name ?? cardName` - provides fallback values
- Graceful handling of missing API data

**String Interpolation**
- `$"Error fetching card: {ex.Message}"`
- Clean, readable string formatting

**LINQ**
- Keyword extraction from card text
- `keywords.Distinct()` to remove duplicates

**Exception Handling**
- Try-catch blocks for API failures
- Graceful error messages to user

---

## Learning Resources

This implementation follows the Abstract Factory pattern as described in:
- **"Dive Into Design Patterns"** by Alexander Shvets
- **"Design Patterns: Elements of Reusable Object-Oriented Software"** (Gang of Four)

### Key Takeaways

1. **Simple ≠ Wrong**: Factory methods that "just instantiate and return" ARE correct. The pattern is about **abstraction and decoupling**, not complexity.

2. **Interface Abstraction**: The power is in the `ICardFactory` interface, which allows client code to work with any factory implementation.

3. **Product Families**: Abstract Factory ensures related products (Red creatures + Red spells) are created together consistently.

4. **Extensibility**: Adding new factories doesn't require changing existing code - this is the Open/Closed Principle in action.

---

## License

This project is for educational purposes to demonstrate the Abstract Factory design pattern.

MTG card data provided by [Scryfall](https://scryfall.com/) (<https://api.scryfall.com>).

Magic: The Gathering is a trademark of Wizards of the Coast LLC.
