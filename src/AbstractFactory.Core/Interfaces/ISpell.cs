namespace AbstractFactory.Core.Interfaces;

/// <summary>
/// Abstract Product: Defines the interface for Spell cards
/// All spells must implement these methods regardless of their color
/// </summary>
public interface ISpell
{
    /// <summary>
    /// Get the spell's name
    /// </summary>
    string GetName();

    /// <summary>
    /// Get the spell's mana cost
    /// </summary>
    string GetManaCost();

    /// <summary>
    /// Get the spell's keywords
    /// </summary>
    string GetKeywords();

    /// <summary>
    /// Get the spell's card text
    /// </summary>
    string GetText();

    /// <summary>
    /// Get the URL for the spell's card image
    /// </summary>
    string GetImageUrl();
}
