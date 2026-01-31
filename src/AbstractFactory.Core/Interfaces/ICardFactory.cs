namespace AbstractFactory.Core.Interfaces;

/// <summary>
/// Abstract Factory: Declares methods for creating abstract products (Creature and Spell)
/// Each concrete factory will implement this to create products of a specific color/theme
/// </summary>
public interface ICardFactory
{
    /// <summary>
    /// Factory method to create a creature card by name
    /// </summary>
    /// <param name="cardName">The name of the creature card to create</param>
    ICreature CreateCreature(string cardName);

    /// <summary>
    /// Factory method to create a spell card by name
    /// </summary>
    /// <param name="cardName">The name of the spell card to create</param>
    ISpell CreateSpell(string cardName);
}
