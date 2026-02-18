"use client";

import { useState } from "react";
import Image from "next/image";

interface CardSearchResult {
  name: string;
  manaCost: string;
  type: string;
  text: string;
  power: string;
  toughness: string;
  imageUrl: string;
}

interface DeckCard {
  name: string;
  manaCost: string;
  type: "creature" | "spell";
  powerToughness?: string;
  keywords: string;
  text: string;
  imageUrl: string;
  count: number;
}

interface ArtVersion {
  imageUrl: string;
  artCropUrl: string;
  setName: string;
  setCode: string;
  collectorNumber: string;
  artist: string;
}

interface ArtVersionsResponse {
  cardName: string;
  totalArt: number;
  versions: ArtVersion[];
}

export default function Home() {
  // Search state
  const [cardName, setCardName] = useState("");
  const [searchResult, setSearchResult] = useState<CardSearchResult | null>(
    null
  );
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Art state
  const [artVersions, setArtVersions] = useState<ArtVersion[]>([]);
  const [selectedArtUrl, setSelectedArtUrl] = useState<string>("");
  const [loadingArt, setLoadingArt] = useState(false);

  // Deck state
  const [redDeck, setRedDeck] = useState<DeckCard[]>([]);
  const [blueDeck, setBlueDeck] = useState<DeckCard[]>([]);
  const [activeDeckTab, setActiveDeckTab] = useState<"red" | "blue">("red");
  const [addingToDeck, setAddingToDeck] = useState<string | null>(null);
  const [hoveredArtUrl, setHoveredArtUrl] = useState<string | null>(null);

  function isCreature(typeLine: string): boolean {
    return typeLine.toLowerCase().includes("creature");
  }

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    if (!cardName.trim()) return;

    setLoading(true);
    setError("");
    setSearchResult(null);
    setArtVersions([]);
    setSelectedArtUrl("");

    try {
      const response = await fetch(
        `http://localhost:5000/api/cards/search?cardName=${encodeURIComponent(cardName.trim())}`
      );

      if (response.status === 404) {
        throw new Error("Card not found. Check the spelling and try again.");
      }
      if (!response.ok) {
        const errData = await response.json();
        throw new Error(errData.error || "Failed to search for card");
      }

      const data: CardSearchResult = await response.json();
      setSearchResult(data);

      // Fetch art versions
      if (data.name) {
        setLoadingArt(true);
        try {
          const artResponse = await fetch(
            `http://localhost:5000/api/cards/art?cardName=${encodeURIComponent(data.name)}`
          );
          if (artResponse.ok) {
            const artData: ArtVersionsResponse = await artResponse.json();
            setArtVersions(artData.versions);
            setSelectedArtUrl(data.imageUrl);
          }
        } catch {
          setArtVersions([]);
        } finally {
          setLoadingArt(false);
        }
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "An error occurred");
    } finally {
      setLoading(false);
    }
  }

  async function handleAddToDeck(color: "red" | "blue") {
    if (!searchResult) return;

    setAddingToDeck(color);
    setError("");

    try {
      const cardType = isCreature(searchResult.type) ? "creature" : "spell";
      const response = await fetch(
        `http://localhost:5000/api/cards/${cardType}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            deckColor: color,
            cardName: searchResult.name,
          }),
        }
      );

      if (!response.ok) {
        const errData = await response.json();
        throw new Error(errData.error || "Failed to add card to deck");
      }

      const data = await response.json();

      const imageUrl = selectedArtUrl || data.imageUrl;
      const setter = color === "red" ? setRedDeck : setBlueDeck;

      setter((prev) => {
        const existing = prev.find(
          (c) => c.name === data.name && c.imageUrl === imageUrl
        );
        if (existing) {
          return prev.map((c) =>
            c === existing ? { ...c, count: c.count + 1 } : c
          );
        }
        return [
          ...prev,
          {
            name: data.name,
            manaCost: data.manaCost,
            type: cardType,
            powerToughness: data.powerToughness,
            keywords: data.keywords,
            text: data.text,
            imageUrl,
            count: 1,
          },
        ];
      });

      setActiveDeckTab(color);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add card");
    } finally {
      setAddingToDeck(null);
    }
  }

  function handleRemoveFromDeck(card: DeckCard) {
    const setter = activeDeckTab === "red" ? setRedDeck : setBlueDeck;
    setter((prev) => {
      const existing = prev.find(
        (c) => c.name === card.name && c.imageUrl === card.imageUrl
      );
      if (!existing) return prev;
      if (existing.count > 1) {
        return prev.map((c) =>
          c === existing ? { ...c, count: c.count - 1 } : c
        );
      }
      return prev.filter((c) => c !== existing);
    });
  }

  const activeDeck = activeDeckTab === "red" ? redDeck : blueDeck;
  const redTotal = redDeck.reduce((sum, c) => sum + c.count, 0);
  const blueTotal = blueDeck.reduce((sum, c) => sum + c.count, 0);

  return (
    <div className="min-h-screen bg-background p-8">
      <div className="mx-auto max-w-7xl">
        {/* Header */}
        <h1 className="text-4xl font-bold text-orange mb-1">
          MTG Deck Builder
        </h1>
        <p className="text-purple-light mb-8 text-lg">
          Abstract Factory Pattern — Search cards and build your decks
        </p>

        <div className="flex gap-8">
          {/* LEFT COLUMN: Search + Results */}
          <div className="flex-1 min-w-0">
            {/* Search Form */}
            <form
              onSubmit={handleSearch}
              className="bg-surface rounded-lg border border-orange/30 p-6 mb-8"
            >
              <label className="text-orange font-semibold text-sm uppercase tracking-wide mb-3 block">
                Card Name
              </label>
              <div className="flex gap-3">
                <input
                  type="text"
                  value={cardName}
                  onChange={(e) => setCardName(e.target.value)}
                  placeholder="e.g. Lightning Bolt, Snapcaster Mage"
                  className="flex-1 bg-background border border-orange/30 rounded px-4 py-2 text-foreground placeholder:text-foreground/40 focus:outline-none focus:border-orange transition-colors"
                />
                <button
                  type="submit"
                  disabled={!cardName.trim() || loading}
                  className="bg-orange hover:bg-orange-hover text-background font-semibold px-6 py-2 rounded transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {loading ? (
                    <span className="flex items-center gap-2">
                      <span className="inline-block h-4 w-4 border-2 border-background border-t-transparent rounded-full animate-spin" />
                      Searching...
                    </span>
                  ) : (
                    "Search"
                  )}
                </button>
              </div>
            </form>

            {/* Error */}
            {error && (
              <div className="bg-red-deck/20 border border-red-deck/50 text-red-deck rounded-lg p-4 mb-8">
                {error}
              </div>
            )}

            {/* Results */}
            {searchResult && (
              <div className="bg-surface rounded-lg border border-purple/30 p-6">
                <h2 className="text-orange font-semibold text-sm uppercase tracking-wide mb-4">
                  Search Result
                </h2>
                <div className="flex flex-col md:flex-row gap-8">
                  {/* Card Image */}
                  <div className="flex-shrink-0">
                    {(selectedArtUrl || searchResult.imageUrl) ? (
                      <Image
                        src={selectedArtUrl || searchResult.imageUrl}
                        alt={searchResult.name}
                        width={300}
                        height={418}
                        className="rounded-lg"
                        key={selectedArtUrl || searchResult.imageUrl}
                      />
                    ) : (
                      <div className="w-[300px] h-[418px] bg-background/50 rounded-lg flex items-center justify-center border border-foreground/10">
                        <span className="text-foreground/40 text-sm">
                          No image available
                        </span>
                      </div>
                    )}
                  </div>

                  {/* Card Details */}
                  <div className="flex-1 space-y-4">
                    <h3 className="text-2xl font-bold text-purple-light">
                      {searchResult.name}
                    </h3>

                    <div className="space-y-3">
                      <div>
                        <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                          Type
                        </span>
                        <p className="text-foreground mt-1">
                          {searchResult.type}
                        </p>
                      </div>

                      <div>
                        <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                          Mana Cost
                        </span>
                        <p className="text-foreground mt-1">
                          {searchResult.manaCost}
                        </p>
                      </div>

                      {isCreature(searchResult.type) && searchResult.power && (
                        <div>
                          <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                            Power / Toughness
                          </span>
                          <p className="text-foreground mt-1">
                            {searchResult.power}/{searchResult.toughness}
                          </p>
                        </div>
                      )}

                      <div>
                        <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                          Card Text
                        </span>
                        <p className="text-foreground mt-1 whitespace-pre-line">
                          {searchResult.text}
                        </p>
                      </div>
                    </div>

                    {/* Add to Deck Buttons */}
                    <div className="pt-4 border-t border-purple/20">
                      <span className="text-orange font-semibold text-sm uppercase tracking-wide block mb-3">
                        Add to Deck
                      </span>
                      <div className="flex gap-3">
                        <button
                          onClick={() => handleAddToDeck("red")}
                          disabled={addingToDeck !== null}
                          className="flex-1 bg-red-deck/20 border border-red-deck text-red-deck hover:bg-red-deck/30 font-semibold px-4 py-2 rounded transition-colors disabled:opacity-50 disabled:cursor-not-allowed cursor-pointer"
                        >
                          {addingToDeck === "red" ? (
                            <span className="flex items-center justify-center gap-2">
                              <span className="inline-block h-4 w-4 border-2 border-red-deck border-t-transparent rounded-full animate-spin" />
                              Adding...
                            </span>
                          ) : (
                            "Add to Red Deck"
                          )}
                        </button>
                        <button
                          onClick={() => handleAddToDeck("blue")}
                          disabled={addingToDeck !== null}
                          className="flex-1 bg-blue-deck/20 border border-blue-deck text-blue-deck hover:bg-blue-deck/30 font-semibold px-4 py-2 rounded transition-colors disabled:opacity-50 disabled:cursor-not-allowed cursor-pointer"
                        >
                          {addingToDeck === "blue" ? (
                            <span className="flex items-center justify-center gap-2">
                              <span className="inline-block h-4 w-4 border-2 border-blue-deck border-t-transparent rounded-full animate-spin" />
                              Adding...
                            </span>
                          ) : (
                            "Add to Blue Deck"
                          )}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Art Selector */}
                {artVersions.length > 1 && (
                  <div className="mt-6 pt-6 border-t border-purple/20">
                    <h3 className="text-orange font-semibold text-sm uppercase tracking-wide mb-4">
                      Art Versions ({artVersions.length})
                    </h3>
                    {loadingArt ? (
                      <div className="flex items-center gap-2 text-foreground/60">
                        <span className="inline-block h-4 w-4 border-2 border-foreground/40 border-t-transparent rounded-full animate-spin" />
                        Loading art versions...
                      </div>
                    ) : (
                      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-3">
                        {artVersions.map((art, index) => (
                          <button
                            key={`${art.setCode}-${art.collectorNumber}-${index}`}
                            onClick={() => setSelectedArtUrl(art.imageUrl)}
                            onMouseEnter={() => setHoveredArtUrl(art.imageUrl)}
                            onMouseLeave={() => setHoveredArtUrl(null)}
                            className={`rounded-lg border-2 p-2 transition-all cursor-pointer hover:border-orange ${
                              (selectedArtUrl || searchResult.imageUrl) ===
                              art.imageUrl
                                ? "border-orange bg-orange/10"
                                : "border-foreground/10 hover:bg-surface"
                            }`}
                          >
                            <Image
                              src={art.artCropUrl || art.imageUrl}
                              alt={`${searchResult.name} - ${art.setName}`}
                              width={150}
                              height={100}
                              className="rounded w-full h-auto"
                            />
                            <p className="text-xs text-purple-light mt-1 font-medium truncate">
                              {art.setName}
                            </p>
                            <p className="text-xs text-foreground/50 truncate">
                              {art.artist}
                            </p>
                          </button>
                        ))}
                      </div>
                    )}
                  </div>
                )}
              </div>
            )}
          </div>

          {/* RIGHT COLUMN: Deck Panel */}
          <div className="w-80 flex-shrink-0">
            <div className="bg-surface rounded-lg border border-purple/30 p-4 sticky top-8">
              <h2 className="text-orange font-semibold text-sm uppercase tracking-wide mb-4">
                Your Decks
              </h2>

              {/* Deck Tabs */}
              <div className="flex mb-4 rounded overflow-hidden border border-foreground/20">
                <button
                  onClick={() => setActiveDeckTab("red")}
                  className={`flex-1 py-2 text-sm font-semibold transition-colors cursor-pointer ${
                    activeDeckTab === "red"
                      ? "bg-red-deck/20 text-red-deck border-b-2 border-red-deck"
                      : "text-foreground/60 hover:text-foreground/80"
                  }`}
                >
                  Red ({redTotal})
                </button>
                <button
                  onClick={() => setActiveDeckTab("blue")}
                  className={`flex-1 py-2 text-sm font-semibold transition-colors cursor-pointer ${
                    activeDeckTab === "blue"
                      ? "bg-blue-deck/20 text-blue-deck border-b-2 border-blue-deck"
                      : "text-foreground/60 hover:text-foreground/80"
                  }`}
                >
                  Blue ({blueTotal})
                </button>
              </div>

              {/* Card List */}
              <div className="space-y-2 max-h-[calc(100vh-24rem)] overflow-y-auto">
                {activeDeck.length === 0 ? (
                  <p className="text-foreground/40 text-sm text-center py-4">
                    No cards yet. Search and add cards above.
                  </p>
                ) : (
                  activeDeck.map((card, index) => (
                    <div
                      key={`${card.name}-${card.imageUrl}-${index}`}
                      className="flex items-center gap-3 p-2 rounded bg-background/50 border border-foreground/10"
                    >
                      {card.imageUrl && (
                        <Image
                          src={card.imageUrl}
                          alt={card.name}
                          width={40}
                          height={56}
                          className="rounded flex-shrink-0"
                        />
                      )}
                      <div className="min-w-0 flex-1">
                        <p className="text-sm font-semibold text-foreground truncate">
                          {card.name}
                        </p>
                        <p className="text-xs text-foreground/60">
                          {card.manaCost} &middot;{" "}
                          {card.type === "creature" ? "Creature" : "Spell"}
                        </p>
                      </div>
                      {card.count > 1 && (
                        <span className="text-sm font-bold text-orange flex-shrink-0">
                          x{card.count}
                        </span>
                      )}
                      <button
                        onClick={() => handleRemoveFromDeck(card)}
                        className="text-foreground/30 hover:text-red-deck flex-shrink-0 transition-colors cursor-pointer"
                        title={card.count > 1 ? "Remove one" : "Remove"}
                      >
                        &times;
                      </button>
                    </div>
                  ))
                )}
              </div>

              {/* Art Hover Preview */}
              {hoveredArtUrl && (
                <div className="mt-4 pt-4 border-t border-purple/20">
                  <p className="text-orange font-semibold text-xs uppercase tracking-wide mb-2">
                    Art Preview
                  </p>
                  <Image
                    src={hoveredArtUrl}
                    alt="Art preview"
                    width={288}
                    height={401}
                    className="rounded-lg w-full h-auto"
                    key={hoveredArtUrl}
                  />
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
