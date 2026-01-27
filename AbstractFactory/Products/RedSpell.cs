using AbstractFactory.Interfaces;

namespace AbstractFactory.Products;

/// <summary>
/// Concrete Product: Red-aligned spell (Lightning Bolt)
/// Represents direct damage spells typical of red in MTG
/// </summary>
public class RedSpell : ISpell
{
    private readonly string _name = "Lightning Bolt";
    private readonly string _manaCost = "R";
    private readonly int _damage = 3;

    public void Cast()
    {
        Console.WriteLine($"[Red Spell] Casting {_name}! ⚡ Dealing {_damage} damage to target!");
    }

    public string GetEffect()
    {
        return $"Deals {_damage} damage to any target";
    }

    public string GetManaCost()
    {
        return _manaCost;
    }
}
