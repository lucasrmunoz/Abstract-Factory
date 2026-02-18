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

        [JsonPropertyName("art_crop")]
        public string? ArtCrop { get; set; }
    }

    public class ArtVersion
    {
        public string? ImageUrl { get; set; }
        public string? ArtCropUrl { get; set; }
        public string? SetName { get; set; }
        public string? SetCode { get; set; }
        public string? CollectorNumber { get; set; }
        public string? Artist { get; set; }
    }

    private class ScryfallSearchResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("total_cards")]
        public int TotalCards { get; set; }

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("next_page")]
        public string? NextPage { get; set; }

        [JsonPropertyName("data")]
        public List<ScryfallSearchCard>? Data { get; set; }
    }

    private class ScryfallSearchCard
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("set_name")]
        public string? SetName { get; set; }

        [JsonPropertyName("set")]
        public string? Set { get; set; }

        [JsonPropertyName("collector_number")]
        public string? CollectorNumber { get; set; }

        [JsonPropertyName("artist")]
        public string? Artist { get; set; }

        [JsonPropertyName("image_uris")]
        public ScryfallImageUris? ImageUris { get; set; }
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

    /// <summary>
    /// Fetches all unique art versions for a card from Scryfall search API.
    /// Follows pagination to retrieve all results.
    /// </summary>
    public static async Task<List<ArtVersion>> GetArtVersions(string cardName)
    {
        try
        {
            var allVersions = new List<ArtVersion>();
            string? url = $"cards/search?q=!\"{Uri.EscapeDataString(cardName)}\"+unique:art";

            while (url != null)
            {
                var httpResponse = await _httpClient.GetAsync(url);

                if (!httpResponse.IsSuccessStatusCode)
                    break;

                var body = await httpResponse.Content.ReadAsStringAsync();
                var response = System.Text.Json.JsonSerializer.Deserialize<ScryfallSearchResponse>(body);

                if (response?.Data == null)
                    break;

                allVersions.AddRange(
                    response.Data
                        .Where(card => card.ImageUris != null)
                        .Select(card => new ArtVersion
                        {
                            ImageUrl = card.ImageUris?.Normal,
                            ArtCropUrl = card.ImageUris?.ArtCrop,
                            SetName = card.SetName,
                            SetCode = card.Set,
                            CollectorNumber = card.CollectorNumber,
                            Artist = card.Artist
                        })
                );

                // Follow pagination; Scryfall asks for 50-100ms between requests
                if (response.HasMore && response.NextPage != null)
                {
                    await Task.Delay(100);
                    url = response.NextPage;
                }
                else
                {
                    url = null;
                }
            }

            return allVersions;
        }
        catch
        {
            return new List<ArtVersion>();
        }
    }
}
