namespace AbstractFactory.Interfaces;

/// <summary>
/// Abstract Product: Defines the interface for Spell cards
/// All spells must implement these methods regardless of their color
/// </summary>
public interface ISpell
{
    /// <summary>
    /// Cast the spell and display its effect
    /// </summary>
    void Cast();

    /// <summary>
    /// Get the spell's effect description
    /// </summary>
    string GetEffect();

    /// <summary>
    /// Get the mana cost of the spell
    /// </summary>
    string GetManaCost();
}
