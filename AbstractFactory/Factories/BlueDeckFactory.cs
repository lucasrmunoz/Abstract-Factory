using AbstractFactory.Interfaces;
using AbstractFactory.Products;

namespace AbstractFactory.Factories;

/// <summary>
/// Concrete Factory: Creates Blue-themed MTG cards
/// All products created by this factory will be blue-aligned (control/evasion)
/// </summary>
public class BlueDeckFactory : ICardFactory
{
    public ICreature CreateCreature()
    {
        // Returns a blue creature (Merfolk Wizard)
        return new BlueCreature();
    }

    public ISpell CreateSpell()
    {
        // Returns a blue spell (Counterspell)
        return new BlueSpell();
    }
}
