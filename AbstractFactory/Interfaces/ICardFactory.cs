namespace AbstractFactory.Interfaces;

/// <summary>
/// Abstract Factory: Declares methods for creating abstract products (Creature and Spell)
/// Each concrete factory will implement this to create products of a specific color/theme
/// </summary>
public interface ICardFactory
{
    /// <summary>
    /// Factory method to create a creature card
    /// </summary>
    ICreature CreateCreature();

    /// <summary>
    /// Factory method to create a spell card
    /// </summary>
    ISpell CreateSpell();
}
