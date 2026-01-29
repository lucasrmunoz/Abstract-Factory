using AbstractFactory.Core.Interfaces;
using AbstractFactory.Core.Services;

namespace AbstractFactory.Core.Products;

/// <summary>
/// Concrete Product: Red-aligned creature
/// Represents aggressive, fast creatures typical of red in MTG
/// Fetches real card data from MTG API
/// </summary>
public class RedCreature : ICreature
{
    private readonly string _name;
    private readonly string _manaCost;
    private readonly string _power;
    private readonly string _toughness;
    private readonly string _text;

    public RedCreature(string cardName)
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

    public string GetName() => _name;

    public string GetManaCost() => _manaCost;

    public string GetPowerToughness() => $"{_power}/{_toughness}";

    public string GetKeywords()
    {
        if (string.IsNullOrEmpty(_text) || _text.Contains("not found") || _text.Contains("Error"))
            return "None";

        var keywords = new List<string>();
        var knownKeywords = new[] { "Haste", "Flying", "First strike", "Double strike", "Deathtouch",
                                     "Trample", "Vigilance", "Lifelink", "Menace", "Reach" };

        foreach (var keyword in knownKeywords)
        {
            if (_text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                keywords.Add(keyword);
            }
        }

        return keywords.Count > 0 ? string.Join(", ", keywords) : "None";
    }

    public string GetText() => _text;
}
