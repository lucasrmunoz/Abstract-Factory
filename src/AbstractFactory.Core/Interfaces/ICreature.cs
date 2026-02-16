namespace AbstractFactory.Core.Interfaces;

/// <summary>
/// Abstract Product: Defines the interface for Creature cards
/// All creatures must implement these methods regardless of their color
/// </summary>
public interface ICreature
{
    /// <summary>
    /// Get the creature's name
    /// </summary>
    string GetName();

    /// <summary>
    /// Get the creature's mana cost
    /// </summary>
    string GetManaCost();

    /// <summary>
    /// Get the creature's power and toughness
    /// </summary>
    string GetPowerToughness();

    /// <summary>
    /// Get the creature's keywords (e.g., Flying, Haste, Trample)
    /// </summary>
    string GetKeywords();

    /// <summary>
    /// Get the creature's card text
    /// </summary>
    string GetText();

    /// <summary>
    /// Get the URL for the creature's card image
    /// </summary>
    string GetImageUrl();
}
