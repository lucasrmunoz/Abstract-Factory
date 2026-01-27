using AbstractFactory.Interfaces;
using AbstractFactory.Services;

namespace AbstractFactory.Products;

/// <summary>
/// Concrete Product: Blue-aligned creature
/// Represents evasive, controlling creatures typical of blue in MTG
/// Fetches real card data from MTG API
/// </summary>
public class BlueCreature : ICreature
{
    private readonly string _name;
    private readonly string _manaCost;
    private readonly string _power;
    private readonly string _toughness;
    private readonly string _text;

    public BlueCreature(string cardName)
    {
        _name = cardName;
        _manaCost = "N/A";
        _power = "?";
        _toughness = "?";
        _text = "Searching for card...";

        try
        {
            var cardData = MtgCardLookup.GetCardByName(cardName).Result;

            if (cardData != null)
            {
                _name = cardData.Name ?? cardName;
                _manaCost = cardData.ManaCost ?? "N/A";
                _power = cardData.Power ?? "?";
                _toughness = cardData.Toughness ?? "?";
                _text = cardData.Text ?? "No card text available";
            }
            else
            {
                _text = "Card not found in database";
            }
        }
        catch (Exception ex)
        {
            _text = $"Error fetching card: {ex.Message}";
        }
    }

    public string GetName()
    {
        return _name;
    }

    public string GetManaCost()
    {
        return _manaCost;
    }

    public string GetPowerToughness()
    {
        return $"{_power}/{_toughness}";
    }

    public string GetKeywords()
    {
        // Extract keywords from card text (Flying, etc.)
        if (string.IsNullOrEmpty(_text) || _text.Contains("not found") || _text.Contains("Error"))
            return "None";

        var keywords = new List<string>();
        var knownKeywords = new[] { "Haste", "Flying", "First strike", "Double strike", "Deathtouch",
                                     "Trample", "Vigilance", "Lifelink", "Menace", "Reach", "Hexproof", "Flash" };

        foreach (var keyword in knownKeywords)
        {
            if (_text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                keywords.Add(keyword);
            }
        }

        return keywords.Count > 0 ? string.Join(", ", keywords) : "None";
    }

    public string GetText()
    {
        return _text;
    }

    public void DisplayDetails()
    {
        Console.WriteLine($"\n╔═══ BLUE CREATURE ═══");
        Console.WriteLine($"║ Name: {GetName()}");
        Console.WriteLine($"║ Cost: {GetManaCost()}");
        Console.WriteLine($"║ Power/Toughness: {GetPowerToughness()}");
        Console.WriteLine($"║ Keywords: {GetKeywords()}");
        Console.WriteLine($"║ Text: {GetText()}");
        Console.WriteLine($"╚═════════════════════");
    }
}
