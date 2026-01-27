using AbstractFactory.Interfaces;

namespace AbstractFactory.Products;

/// <summary>
/// Concrete Product: Blue-aligned creature (Merfolk Wizard)
/// Represents evasive, controlling creatures typical of blue in MTG
/// </summary>
public class BlueCreature : ICreature
{
    private readonly string _name = "Merfolk Wizard";
    private readonly int _power = 1;
    private readonly int _toughness = 2;
    private readonly string _ability = "Flying";

    public void Attack()
    {
        Console.WriteLine($"[Blue Creature] {_name} attacks through the air! ({_power} damage with evasion)");
    }

    public void Defend()
    {
        Console.WriteLine($"[Blue Creature] {_name} defends with {_toughness} toughness (solid blocker)");
    }

    public string GetStats()
    {
        return $"{_name} ({_power}/{_toughness}) - \"{_ability}: Can't be blocked except by creatures with flying or reach\"";
    }
}
