"use client";

import { useState } from "react";
import Image from "next/image";

interface CardResult {
  name: string;
  manaCost: string;
  powerToughness?: string;
  keywords: string;
  text: string;
  deckColor: string;
  imageUrl: string;
}

export default function Home() {
  const [deckColor, setDeckColor] = useState("");
  const [cardName, setCardName] = useState("");
  const [cardType, setCardType] = useState("creature");
  const [result, setResult] = useState<CardResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  function handleDeckColor(color: string) {
    setDeckColor((prev) => (prev === color ? "" : color));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!deckColor || !cardName.trim()) return;

    setLoading(true);
    setError("");
    setResult(null);

    try {
      const response = await fetch(
        `http://localhost:5000/api/cards/${cardType}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            deckColor: deckColor,
            cardName: cardName.trim(),
          }),
        }
      );

      if (!response.ok) {
        const errData = await response.json();
        throw new Error(errData.error || "Failed to fetch card");
      }

      const data: CardResult = await response.json();
      setResult(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : "An error occurred");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="min-h-screen bg-background p-8">
      <div className="mx-auto max-w-4xl">
        {/* Header */}
        <h1 className="text-4xl font-bold text-orange mb-1">
          MTG Card Lookup
        </h1>
        <p className="text-purple-light mb-8 text-lg">
          Abstract Factory Pattern — Search for Magic: The Gathering cards
        </p>

        {/* Form */}
        <form
          onSubmit={handleSubmit}
          className="bg-surface rounded-lg border border-orange/30 p-6 mb-8"
        >
          <div className="flex flex-col md:flex-row gap-6 mb-6">
            {/* Deck Color */}
            <fieldset>
              <legend className="text-orange font-semibold text-sm uppercase tracking-wide mb-3">
                Deck Color
              </legend>
              <div className="flex gap-3">
                <label
                  className={`flex items-center gap-2 cursor-pointer px-4 py-2 rounded border transition-colors ${
                    deckColor === "red"
                      ? "border-red-deck bg-red-deck/20 text-red-deck"
                      : "border-foreground/20 text-foreground/60 hover:border-foreground/40"
                  }`}
                >
                  <input
                    type="checkbox"
                    checked={deckColor === "red"}
                    onChange={() => handleDeckColor("red")}
                    className="accent-orange w-4 h-4"
                  />
                  Red (Aggressive)
                </label>
                <label
                  className={`flex items-center gap-2 cursor-pointer px-4 py-2 rounded border transition-colors ${
                    deckColor === "blue"
                      ? "border-blue-deck bg-blue-deck/20 text-blue-deck"
                      : "border-foreground/20 text-foreground/60 hover:border-foreground/40"
                  }`}
                >
                  <input
                    type="checkbox"
                    checked={deckColor === "blue"}
                    onChange={() => handleDeckColor("blue")}
                    className="accent-orange w-4 h-4"
                  />
                  Blue (Control)
                </label>
              </div>
            </fieldset>

            {/* Card Name */}
            <div className="flex-1">
              <label className="text-orange font-semibold text-sm uppercase tracking-wide mb-3 block">
                Card Name
              </label>
              <input
                type="text"
                value={cardName}
                onChange={(e) => setCardName(e.target.value)}
                placeholder="e.g. Lightning Bolt, Snapcaster Mage"
                className="w-full bg-background border border-orange/30 rounded px-4 py-2 text-foreground placeholder:text-foreground/40 focus:outline-none focus:border-orange transition-colors"
              />
            </div>
          </div>

          {/* Card Type */}
          <fieldset className="mb-6">
            <legend className="text-orange font-semibold text-sm uppercase tracking-wide mb-3">
              Card Type
            </legend>
            <div className="flex gap-4">
              <label
                className={`flex items-center gap-2 cursor-pointer px-4 py-2 rounded border transition-colors ${
                  cardType === "creature"
                    ? "border-purple bg-purple/20 text-purple-light"
                    : "border-foreground/20 text-foreground/60 hover:border-foreground/40"
                }`}
              >
                <input
                  type="radio"
                  name="cardType"
                  value="creature"
                  checked={cardType === "creature"}
                  onChange={(e) => setCardType(e.target.value)}
                  className="accent-purple w-4 h-4"
                />
                Creature
              </label>
              <label
                className={`flex items-center gap-2 cursor-pointer px-4 py-2 rounded border transition-colors ${
                  cardType === "spell"
                    ? "border-purple bg-purple/20 text-purple-light"
                    : "border-foreground/20 text-foreground/60 hover:border-foreground/40"
                }`}
              >
                <input
                  type="radio"
                  name="cardType"
                  value="spell"
                  checked={cardType === "spell"}
                  onChange={(e) => setCardType(e.target.value)}
                  className="accent-purple w-4 h-4"
                />
                Spell
              </label>
            </div>
          </fieldset>

          {/* Submit */}
          <button
            type="submit"
            disabled={!deckColor || !cardName.trim() || loading}
            className="bg-orange hover:bg-orange-hover text-background font-semibold px-6 py-2 rounded transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {loading ? (
              <span className="flex items-center gap-2">
                <span className="inline-block h-4 w-4 border-2 border-background border-t-transparent rounded-full animate-spin" />
                Searching...
              </span>
            ) : (
              "Look Up Card"
            )}
          </button>
        </form>

        {/* Error */}
        {error && (
          <div className="bg-red-deck/20 border border-red-deck/50 text-red-deck rounded-lg p-4 mb-8">
            {error}
          </div>
        )}

        {/* Results */}
        {result && (
          <div className="bg-surface rounded-lg border border-purple/30 p-6">
            <h2 className="text-orange font-semibold text-sm uppercase tracking-wide mb-4">
              Results
            </h2>
            <div className="flex flex-col md:flex-row gap-8">
              {/* Card Image */}
              <div className="flex-shrink-0">
                {result.imageUrl ? (
                  <Image
                    src={result.imageUrl}
                    alt={result.name}
                    width={300}
                    height={418}
                    className="rounded-lg"
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
                  {result.name}
                </h3>

                <div className="space-y-3">
                  <div>
                    <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                      Mana Cost
                    </span>
                    <p className="text-foreground mt-1">{result.manaCost}</p>
                  </div>

                  {result.powerToughness && (
                    <div>
                      <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                        Power / Toughness
                      </span>
                      <p className="text-foreground mt-1">
                        {result.powerToughness}
                      </p>
                    </div>
                  )}

                  <div>
                    <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                      Keywords
                    </span>
                    <p className="text-foreground mt-1">{result.keywords}</p>
                  </div>

                  <div>
                    <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                      Card Text
                    </span>
                    <p className="text-foreground mt-1 whitespace-pre-line">
                      {result.text}
                    </p>
                  </div>

                  <div>
                    <span className="text-orange font-semibold text-sm uppercase tracking-wide">
                      Deck Color
                    </span>
                    <p
                      className={`mt-1 font-semibold capitalize ${
                        result.deckColor === "red"
                          ? "text-red-deck"
                          : "text-blue-deck"
                      }`}
                    >
                      {result.deckColor}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
