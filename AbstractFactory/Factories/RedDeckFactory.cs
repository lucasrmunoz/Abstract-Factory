using AbstractFactory.Interfaces;
using AbstractFactory.Products;

namespace AbstractFactory.Factories;

/// <summary>
/// Concrete Factory: Creates Red-themed MTG cards
/// All products created by this factory will be red-aligned (aggressive/direct damage)
/// </summary>
public class RedDeckFactory : ICardFactory
{
    public ICreature CreateCreature()
    {
        // Returns a red creature (Goblin Warrior)
        return new RedCreature();
    }

    public ISpell CreateSpell()
    {
        // Returns a red spell (Lightning Bolt)
        return new RedSpell();
    }
}
