using AbstractFactory.Interfaces;

namespace AbstractFactory.Products;

/// <summary>
/// Concrete Product: Red-aligned creature (Goblin Warrior)
/// Represents aggressive, fast creatures typical of red in MTG
/// </summary>
public class RedCreature : ICreature
{
    private readonly string _name = "Goblin Warrior";
    private readonly int _power = 2;
    private readonly int _toughness = 1;
    private readonly string _ability = "Haste";

    public void Attack()
    {
        Console.WriteLine($"[Red Creature] {_name} attacks aggressively! ({_power} damage)");
    }

    public void Defend()
    {
        Console.WriteLine($"[Red Creature] {_name} blocks with {_toughness} toughness (not ideal for defense)");
    }

    public string GetStats()
    {
        return $"{_name} ({_power}/{_toughness}) - \"{_ability}: Can attack immediately!\"";
    }
}
