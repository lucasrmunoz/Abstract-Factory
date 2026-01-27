using AbstractFactory.Interfaces;
using AbstractFactory.Products;

namespace AbstractFactory.Factories;

/// <summary>
/// Concrete Factory: Creates Red-themed MTG cards
/// All products created by this factory will be red-aligned (aggressive/direct damage)
/// </summary>
public class RedDeckFactory : ICardFactory
{
    public ICreature CreateCreature(string cardName)
    {
        // Returns a red creature from MTG API
        return new RedCreature(cardName);
    }

    public ISpell CreateSpell(string cardName)
    {
        // Returns a red spell from MTG API
        return new RedSpell(cardName);
    }
}
