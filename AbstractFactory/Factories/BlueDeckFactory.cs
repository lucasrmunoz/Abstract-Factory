using AbstractFactory.Interfaces;
using AbstractFactory.Products;

namespace AbstractFactory.Factories;

/// <summary>
/// Concrete Factory: Creates Blue-themed MTG cards
/// All products created by this factory will be blue-aligned (control/evasion)
/// </summary>
public class BlueDeckFactory : ICardFactory
{
    public ICreature CreateCreature(string cardName)
    {
        // Returns a blue creature from MTG API
        return new BlueCreature(cardName);
    }

    public ISpell CreateSpell(string cardName)
    {
        // Returns a blue spell from MTG API
        return new BlueSpell(cardName);
    }
}
