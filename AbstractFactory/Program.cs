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
        Console.WriteLine("║  Abstract Factory Pattern - MTG Card Creator Demo           ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

        // Demonstrate Red deck factory
        Console.WriteLine("=== RED DECK FACTORY ===");
        ICardFactory redFactory = new RedDeckFactory();
        DemonstrateFactory(redFactory);

        Console.WriteLine("\n" + new string('─', 60) + "\n");

        // Demonstrate Blue deck factory
        Console.WriteLine("=== BLUE DECK FACTORY ===");
        ICardFactory blueFactory = new BlueDeckFactory();
        DemonstrateFactory(blueFactory);

        Console.WriteLine("\n" + new string('═', 60));
        Console.WriteLine("\n🎯 Key Takeaway:");
        Console.WriteLine("   The DemonstrateFactory() method works with ANY factory");
        Console.WriteLine("   that implements ICardFactory. It doesn't know or care");
        Console.WriteLine("   whether it's creating Red or Blue cards!");
        Console.WriteLine("\n   This is the power of the Abstract Factory pattern! ✨");
        Console.WriteLine(new string('═', 60));
    }

    /// <summary>
    /// Client code that works with factories through the abstract interface.
    /// This method can use ANY concrete factory without knowing its specific type.
    /// This demonstrates the core benefit of the Abstract Factory pattern.
    /// </summary>
    static void DemonstrateFactory(ICardFactory factory)
    {
        // Create a creature using the factory
        Console.WriteLine("\n📦 Creating a creature...");
        ICreature creature = factory.CreateCreature();
        Console.WriteLine($"   {creature.GetStats()}");
        creature.Attack();
        creature.Defend();

        // Create a spell using the factory
        Console.WriteLine("\n📦 Creating a spell...");
        ISpell spell = factory.CreateSpell();
        Console.WriteLine($"   Mana Cost: {spell.GetManaCost()}");
        Console.WriteLine($"   Effect: {spell.GetEffect()}");
        spell.Cast();
    }
}
