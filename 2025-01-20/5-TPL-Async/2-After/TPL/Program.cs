
Parallel.For(0, 10, i => VoerEenLangeBerekeningUit(i * 100_000, (i+1) * 100_000, $"Taak {i}"));

Console.WriteLine("Klaar met alle taken");
Console.ReadKey();


static void VoerEenLangeBerekeningUit(int van, int tot, string tekst)
{
    var aantal = BerekenPriemgetallen(van, tot);

    Console.WriteLine($"{aantal} priemgetallen vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}");
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

        if(isPriemgetal)
        {
            aantalPriemgetallen++;
        }
    }

    return aantalPriemgetallen;
}