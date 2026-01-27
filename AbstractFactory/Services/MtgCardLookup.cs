using System.Net.Http.Json;

namespace AbstractFactory.Services;

/// <summary>
/// Helper class to lookup MTG cards from the API
/// Uses https://api.magicthegathering.io
/// </summary>
public class MtgCardLookup
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.magicthegathering.io/v1/")
    };

    public class CardData
    {
        public string? Name { get; set; }
        public string? ManaCost { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
        public string? Power { get; set; }
        public string? Toughness { get; set; }
    }

    public class CardResponse
    {
        public List<CardData>? Cards { get; set; }
    }

    public static async Task<CardData?> GetCardByName(string cardName)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CardResponse>($"cards?name={Uri.EscapeDataString(cardName)}");
            return response?.Cards?.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }
}
