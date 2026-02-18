using AbstractFactory.Core.Factories;
using AbstractFactory.Core.Interfaces;
using AbstractFactory.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS for Next.js dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("NextJsDev", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Abstract Factory MTG API", Version = "v1" });
});

var app = builder.Build();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abstract Factory MTG API v1");
    c.RoutePrefix = string.Empty; // Swagger UI at root
});

app.UseCors("NextJsDev");

// GET /api/decks - List available deck types
app.MapGet("/api/decks", () =>
{
    return Results.Ok(new[]
    {
        new { Id = "red", Name = "Red", Description = "Aggressive" },
        new { Id = "blue", Name = "Blue", Description = "Control" }
    });
})
.WithName("GetDecks")
.WithTags("Decks")
.WithDescription("Returns available deck types (factory options)");

// POST /api/cards/creature - Create a creature card
app.MapPost("/api/cards/creature", (CreateCardRequest request) =>
{
    var factory = GetFactory(request.DeckColor);
    if (factory == null)
    {
        return Results.BadRequest(new { Error = $"Unknown deck color: {request.DeckColor}" });
    }

    var creature = factory.CreateCreature(request.CardName);

    return Results.Ok(new CreatureResponse
    {
        Name = creature.GetName(),
        ManaCost = creature.GetManaCost(),
        PowerToughness = creature.GetPowerToughness(),
        Keywords = creature.GetKeywords(),
        Text = creature.GetText(),
        DeckColor = request.DeckColor,
        ImageUrl = creature.GetImageUrl()
    });
})
.WithName("CreateCreature")
.WithTags("Cards")
.WithDescription("Creates a creature card using the specified deck factory");

// POST /api/cards/spell - Create a spell card
app.MapPost("/api/cards/spell", (CreateCardRequest request) =>
{
    var factory = GetFactory(request.DeckColor);
    if (factory == null)
    {
        return Results.BadRequest(new { Error = $"Unknown deck color: {request.DeckColor}" });
    }

    var spell = factory.CreateSpell(request.CardName);

    return Results.Ok(new SpellResponse
    {
        Name = spell.GetName(),
        ManaCost = spell.GetManaCost(),
        Keywords = spell.GetKeywords(),
        Text = spell.GetText(),
        DeckColor = request.DeckColor,
        ImageUrl = spell.GetImageUrl()
    });
})
.WithName("CreateSpell")
.WithTags("Cards")
.WithDescription("Creates a spell card using the specified deck factory");

// GET /api/cards/art - Get all unique art versions for a card
app.MapGet("/api/cards/art", async (string cardName) =>
{
    if (string.IsNullOrWhiteSpace(cardName))
    {
        return Results.BadRequest(new { Error = "cardName query parameter is required" });
    }

    var artVersions = await MtgCardLookup.GetArtVersions(cardName);

    return Results.Ok(new ArtVersionsResponse
    {
        CardName = cardName,
        TotalArt = artVersions.Count,
        Versions = artVersions.Select(v => new ArtVersionDto
        {
            ImageUrl = v.ImageUrl ?? "",
            ArtCropUrl = v.ArtCropUrl ?? "",
            SetName = v.SetName ?? "Unknown Set",
            SetCode = v.SetCode ?? "",
            CollectorNumber = v.CollectorNumber ?? "",
            Artist = v.Artist ?? "Unknown Artist"
        }).ToList()
    });
})
.WithName("GetCardArt")
.WithTags("Cards")
.WithDescription("Returns all unique art versions for a card name");

// GET /api/cards/search - Search for a card by name (no factory, no color)
app.MapGet("/api/cards/search", async (string cardName) =>
{
    if (string.IsNullOrWhiteSpace(cardName))
    {
        return Results.BadRequest(new { Error = "cardName query parameter is required" });
    }

    var cardData = await MtgCardLookup.GetCardByName(cardName);

    if (cardData == null)
    {
        return Results.NotFound(new { Error = $"Card not found: {cardName}" });
    }

    return Results.Ok(new CardSearchResponse
    {
        Name = cardData.Name ?? "",
        ManaCost = cardData.ManaCost ?? "",
        Type = cardData.Type ?? "",
        Text = cardData.Text ?? "",
        Power = cardData.Power ?? "",
        Toughness = cardData.Toughness ?? "",
        ImageUrl = cardData.ImageUrl ?? ""
    });
})
.WithName("SearchCard")
.WithTags("Cards")
.WithDescription("Searches for a card by name, bypasses factory");

app.Run();

// Helper to get the appropriate factory
static ICardFactory? GetFactory(string deckColor)
{
    return deckColor.ToLowerInvariant() switch
    {
        "red" => new RedDeckFactory(),
        "blue" => new BlueDeckFactory(),
        _ => null
    };
}

// Request/Response DTOs
record CreateCardRequest(string DeckColor, string CardName);

record CreatureResponse
{
    public string Name { get; init; } = "";
    public string ManaCost { get; init; } = "";
    public string PowerToughness { get; init; } = "";
    public string Keywords { get; init; } = "";
    public string Text { get; init; } = "";
    public string DeckColor { get; init; } = "";
    public string ImageUrl { get; init; } = "";
}

record SpellResponse
{
    public string Name { get; init; } = "";
    public string ManaCost { get; init; } = "";
    public string Keywords { get; init; } = "";
    public string Text { get; init; } = "";
    public string DeckColor { get; init; } = "";
    public string ImageUrl { get; init; } = "";
}

record ArtVersionDto
{
    public string ImageUrl { get; init; } = "";
    public string ArtCropUrl { get; init; } = "";
    public string SetName { get; init; } = "";
    public string SetCode { get; init; } = "";
    public string CollectorNumber { get; init; } = "";
    public string Artist { get; init; } = "";
}

record ArtVersionsResponse
{
    public string CardName { get; init; } = "";
    public int TotalArt { get; init; }
    public List<ArtVersionDto> Versions { get; init; } = new();
}

record CardSearchResponse
{
    public string Name { get; init; } = "";
    public string ManaCost { get; init; } = "";
    public string Type { get; init; } = "";
    public string Text { get; init; } = "";
    public string Power { get; init; } = "";
    public string Toughness { get; init; } = "";
    public string ImageUrl { get; init; } = "";
}
