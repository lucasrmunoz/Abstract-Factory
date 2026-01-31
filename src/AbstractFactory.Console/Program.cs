using Spectre.Console;
using AbstractFactory.Core.Interfaces;
using AbstractFactory.Core.Factories;

namespace AbstractFactory.Console;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Write(
            new FigletText("MTG Card Factory")
                .Centered()
                .Color(Color.Gold1));

        AnsiConsole.Write(new Rule("[yellow]Abstract Factory Pattern Demo[/]").RuleStyle("grey").Centered());
        AnsiConsole.WriteLine();

        bool continueRunning = true;

        while (continueRunning)
        {
            // Select deck color
            var deckChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your [green]deck color[/]:")
                    .PageSize(5)
                    .AddChoices(new[] { "Red (Aggressive)", "Blue (Control)" }));

            ICardFactory factory = deckChoice.StartsWith("Red")
                ? new RedDeckFactory()
                : new BlueDeckFactory();

            string deckColor = deckChoice.StartsWith("Red") ? "Red" : "Blue";
            Color themeColor = deckChoice.StartsWith("Red") ? Color.Red : Color.Blue;

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[{themeColor}]{deckColor.ToUpper()} DECK FACTORY[/]").RuleStyle("grey"));
            AnsiConsole.WriteLine();

            // Get creature name
            var creatureName = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter creature card name[/] (e.g., 'Goblin Guide', 'Snapcaster Mage'):")
                    .DefaultValue(deckChoice.StartsWith("Red") ? "Goblin Guide" : "Snapcaster Mage")
                    .DefaultValueStyle(new Style(Color.Grey)));

            // Get spell name
            var spellName = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter spell card name[/] (e.g., 'Lightning Bolt', 'Counterspell'):")
                    .DefaultValue(deckChoice.StartsWith("Red") ? "Lightning Bolt" : "Counterspell")
                    .DefaultValueStyle(new Style(Color.Grey)));

            AnsiConsole.WriteLine();

            // Fetch and display cards with spinner
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse(deckColor.ToLower()))
                .Start("Fetching cards from MTG API...", ctx =>
                {
                    var creature = factory.CreateCreature(creatureName);
                    var spell = factory.CreateSpell(spellName);

                    ctx.Status("Rendering cards...");

                    AnsiConsole.WriteLine();
                    DisplayCreature(creature, themeColor);
                    AnsiConsole.WriteLine();
                    DisplaySpell(spell, themeColor);
                });

            AnsiConsole.WriteLine();

            // Pattern benefits
            var benefitsPanel = new Panel(
                new Markup(
                    "[grey]Abstract Factory Pattern Benefits:[/]\n" +
                    "  [green]✓[/] Client code works with ANY factory via ICardFactory\n" +
                    "  [green]✓[/] Deck color determines which factory is used\n" +
                    "  [green]✓[/] Factory creates products matching its theme\n" +
                    "  [green]✓[/] Real MTG data fetched dynamically from API"))
                .Header("[yellow]Pattern Info[/]")
                .Border(BoxBorder.Rounded)
                .BorderStyle(new Style(Color.Grey));

            AnsiConsole.Write(benefitsPanel);
            AnsiConsole.WriteLine();

            // Continue?
            continueRunning = AnsiConsole.Confirm("Look up more cards?", defaultValue: false);
            AnsiConsole.WriteLine();
        }

        AnsiConsole.Write(new Rule("[grey]Thanks for using MTG Card Factory![/]").RuleStyle("grey"));
    }

    static void DisplayCreature(ICreature creature, Color themeColor)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(themeColor)
            .Title($"[{themeColor}]CREATURE[/]")
            .AddColumn(new TableColumn("[grey]Property[/]").Width(15))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("[bold]Name[/]", creature.GetName());
        table.AddRow("[bold]Mana Cost[/]", creature.GetManaCost());
        table.AddRow("[bold]Power/Toughness[/]", creature.GetPowerToughness());
        table.AddRow("[bold]Keywords[/]", creature.GetKeywords());
        table.AddRow("[bold]Text[/]", WrapText(creature.GetText(), 50));

        AnsiConsole.Write(table);
    }

    static void DisplaySpell(ISpell spell, Color themeColor)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(themeColor)
            .Title($"[{themeColor}]SPELL[/]")
            .AddColumn(new TableColumn("[grey]Property[/]").Width(15))
            .AddColumn(new TableColumn("[grey]Value[/]"));

        table.AddRow("[bold]Name[/]", spell.GetName());
        table.AddRow("[bold]Mana Cost[/]", spell.GetManaCost());
        table.AddRow("[bold]Type[/]", spell.GetKeywords());
        table.AddRow("[bold]Text[/]", WrapText(spell.GetText(), 50));

        AnsiConsole.Write(table);
    }

    static string WrapText(string text, int maxWidth)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxWidth)
            return text;

        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";

        foreach (var word in words)
        {
            if (currentLine.Length + word.Length + 1 <= maxWidth)
            {
                currentLine += (currentLine.Length > 0 ? " " : "") + word;
            }
            else
            {
                if (currentLine.Length > 0)
                    lines.Add(currentLine);
                currentLine = word;
            }
        }

        if (currentLine.Length > 0)
            lines.Add(currentLine);

        return string.Join("\n", lines);
    }
}
