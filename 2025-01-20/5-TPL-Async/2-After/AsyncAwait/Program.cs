
var taken = Enumerable.Range(0, 10).Select(i => Task.Run(() => VoerEenLangeBerekeningUit(i * 100_000, (i + 1) * 100_000, $"Taak {i}")));

Console.WriteLine("Eerst een pauze...");

var delay = await Pauze();

Console.WriteLine($"Pauze van {delay}ms afgelopen. Alle taken gestart!");

var resultaten = await Task.WhenAll(taken);

Console.WriteLine("Klaar met alle taken");
Console.WriteLine($"Totaal aantal priemgetallen: {resultaten.Sum()}");

Console.ReadKey();

static async Task<int> Pauze()
{
    var delay = 5000;

    Console.WriteLine("Pauze gestart");
    await Task.Delay(delay);
    Console.WriteLine("Pauze klaar");

    return delay;
}


static int VoerEenLangeBerekeningUit(int van, int tot, string tekst)
{
    var aantal = BerekenPriemgetallen(van, tot);

    Console.WriteLine($"{aantal} priemgetallen vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}");

    return aantal;
}

static int BerekenPriemgetallen(int van, int tot)
{
    var aantalPriemgetallen = 0;

    for (int i = van; i < tot; i++)
    {
        bool isPriemgetal = true;

        for (int j = 2; j < i; j++)
        {
            if (i % j == 0)
            {
                isPriemgetal = false;
                break;
            }
        }

        if (isPriemgetal)
        {
            aantalPriemgetallen++;
        }
    }

    return aantalPriemgetallen;
}