namespace AbstractFactory.Interfaces;

/// <summary>
/// Abstract Product: Defines the interface for Creature cards
/// All creatures must implement these methods regardless of their color
/// </summary>
public interface ICreature
{
    /// <summary>
    /// Display the creature's attack behavior
    /// </summary>
    void Attack();

    /// <summary>
    /// Display the creature's defense behavior
    /// </summary>
    void Defend();

    /// <summary>
    /// Get the creature's power and toughness stats
    /// </summary>
    string GetStats();
}
