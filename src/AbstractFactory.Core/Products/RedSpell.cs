using AbstractFactory.Core.Interfaces;
using AbstractFactory.Core.Services;

namespace AbstractFactory.Core.Products;

/// <summary>
/// Concrete Product: Red-aligned spell
/// Represents direct damage spells typical of red in MTG
/// Fetches real card data from MTG API
/// </summary>
public class RedSpell : ISpell
{
    private readonly string _name;
    private readonly string _manaCost;
    private readonly string _type;
    private readonly string _text;

    public RedSpell(string cardName)
    {
        _name = cardName;
        _manaCost = "N/A";
        _type = "Unknown";
        _text = "Searching for card...";

        try
        {
            var cardData = MtgCardLookup.GetCardByName(cardName).Result;

            if (cardData != null)
            {
                _name = cardData.Name ?? cardName;
                _manaCost = cardData.ManaCost ?? "N/A";
                _type = cardData.Type ?? "Unknown";
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

    public string GetKeywords()
    {
        if (string.IsNullOrEmpty(_text) || _text.Contains("not found") || _text.Contains("Error"))
            return _type;

        var keywords = new List<string>();
        var knownKeywords = new[] { "Instant", "Sorcery", "Flash", "Split second", "Storm" };

        foreach (var keyword in knownKeywords)
        {
            if (_text.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                _type.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                keywords.Add(keyword);
            }
        }

        return keywords.Count > 0 ? string.Join(", ", keywords.Distinct()) : _type;
    }

    public string GetText() => _text;
}
