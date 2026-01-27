using AbstractFactory.Interfaces;

namespace AbstractFactory.Products;

/// <summary>
/// Concrete Product: Blue-aligned spell (Counterspell)
/// Represents control/counter magic typical of blue in MTG
/// </summary>
public class BlueSpell : ISpell
{
    private readonly string _name = "Counterspell";
    private readonly string _manaCost = "UU";

    public void Cast()
    {
        Console.WriteLine($"[Blue Spell] Casting {_name}! 🌊 Negating opponent's spell!");
    }

    public string GetEffect()
    {
        return "Counter target spell";
    }

    public string GetManaCost()
    {
        return _manaCost;
    }
}
