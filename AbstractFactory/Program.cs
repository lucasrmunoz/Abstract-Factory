using AbstractFactory.Interfaces;
using AbstractFactory.Factories;

namespace AbstractFactory;

/// <summary>
/// Abstract Factory Pattern Demo - Magic the Gathering Card Creator
///
/// This demonstrates how the Abstract Factory pattern allows us to create
/// families of related objects (creatures and spells) without specifying
/// their concrete classes. The client code works with factories through
/// the ICardFactory interface, making it easy to switch between different
/// card themes (Red vs Blue) without changing the client code.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Abstract Factory Pattern - MTG Card Lookup                 ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

        // Get user input for deck color
        Console.WriteLine("Choose your deck color:");
        Console.WriteLine("1. Red (Aggressive)");
        Console.WriteLine("2. Blue (Control)");
        Console.Write("\nEnter choice (1 or 2): ");
        string? choice = Console.ReadLine();

        ICardFactory factory = choice == "2"
            ? new BlueDeckFactory()
            : new RedDeckFactory();

        string deckColor = choice == "2" ? "Blue" : "Red";
        Console.WriteLine($"\n=== {deckColor.ToUpper()} DECK FACTORY ===\n");

        // Get creature name from user
        Console.Write("Enter creature card name (e.g., 'Goblin Guide', 'Snapcaster Mage'): ");
        string? creatureName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(creatureName))
        {
            creatureName = choice == "2" ? "Merfolk" : "Goblin";
        }

        // Get spell name from user
        Console.Write("Enter spell card name (e.g., 'Lightning Bolt', 'Counterspell'): ");
        string? spellName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(spellName))
        {
            spellName = choice == "2" ? "Counterspell" : "Lightning Bolt";
        }

        Console.WriteLine("\nFetching cards from MTG API...");
        DemonstrateFactory(factory, creatureName, spellName);

        Console.WriteLine("\n" + new string('═', 60));
        Console.WriteLine("\n📝 Abstract Factory Pattern Benefits:");
        Console.WriteLine("   ✓ Client code works with ANY factory via ICardFactory");
        Console.WriteLine("   ✓ Deck color determines which factory is used");
        Console.WriteLine("   ✓ Factory creates products matching its theme");
        Console.WriteLine("   ✓ Real MTG data fetched dynamically from API");
        Console.WriteLine(new string('═', 60));
    }

    /// <summary>
    /// Client code that works with factories through the abstract interface.
    /// This method can use ANY concrete factory without knowing its specific type.
    /// This demonstrates the core benefit of the Abstract Factory pattern.
    /// </summary>
    static void DemonstrateFactory(ICardFactory factory, string creatureName, string spellName)
    {
        // Create a creature using the factory
        Console.WriteLine($"\n📦 Looking up creature: '{creatureName}'");
        ICreature creature = factory.CreateCreature(creatureName);
        creature.DisplayDetails();

        // Create a spell using the factory
        Console.WriteLine($"\n📦 Looking up spell: '{spellName}'");
        ISpell spell = factory.CreateSpell(spellName);
        spell.DisplayDetails();
    }
}
