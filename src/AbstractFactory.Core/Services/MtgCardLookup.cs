using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AbstractFactory.Core.Services;

/// <summary>
/// Helper class to lookup MTG cards from the Scryfall API
/// Uses https://api.scryfall.com for comprehensive, actively-maintained card data
/// Supports fuzzy name matching and all modern sets including Universes Beyond
/// </summary>
public class MtgCardLookup
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://api.scryfall.com/")
    };

    static MtgCardLookup()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AbstractFactoryMTG/1.0");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// Card data transfer object for product classes
    /// </summary>
    public class CardData
    {
        public string? Name { get; set; }
        public string? ManaCost { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
        public string? Power { get; set; }
        public string? Toughness { get; set; }
        public List<string>? Colors { get; set; }
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Internal DTO for deserializing Scryfall API responses
    /// </summary>
    private class ScryfallCardResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("mana_cost")]
        public string? ManaCost { get; set; }

        [JsonPropertyName("type_line")]
        public string? TypeLine { get; set; }

        [JsonPropertyName("oracle_text")]
        public string? OracleText { get; set; }

        [JsonPropertyName("power")]
        public string? Power { get; set; }

        [JsonPropertyName("toughness")]
        public string? Toughness { get; set; }

        [JsonPropertyName("colors")]
        public List<string>? Colors { get; set; }

        [JsonPropertyName("image_uris")]
        public ScryfallImageUris? ImageUris { get; set; }
    }

    private class ScryfallImageUris
    {
        [JsonPropertyName("normal")]
        public string? Normal { get; set; }

        [JsonPropertyName("large")]
        public string? Large { get; set; }
    }

    /// <summary>
    /// Fetches card data from Scryfall API using fuzzy name matching
    /// </summary>
    /// <param name="cardName">Name of the MTG card to search for</param>
    /// <returns>CardData object if found, null if not found or error occurs</returns>
    public static async Task<CardData?> GetCardByName(string cardName)
    {
        try
        {
            var url = $"cards/named?fuzzy={Uri.EscapeDataString(cardName)}";
            var httpResponse = await _httpClient.GetAsync(url);
            var body = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var response = System.Text.Json.JsonSerializer.Deserialize<ScryfallCardResponse>(body);

            if (response?.Object == "error" || response == null)
            {
                return null;
            }

            return new CardData
            {
                Name = response.Name,
                ManaCost = response.ManaCost,
                Type = response.TypeLine,
                Text = response.OracleText,
                Power = response.Power,
                Toughness = response.Toughness,
                Colors = response.Colors,
                ImageUrl = response.ImageUris?.Normal
            };
        }
        catch
        {
            return null;
        }
    }
}
