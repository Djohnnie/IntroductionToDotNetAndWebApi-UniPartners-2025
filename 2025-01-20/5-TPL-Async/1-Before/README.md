# IntroductionToDotNetAndWebApi-UniPartners-2025
TPL en Async (Task, async/await)

## Tasks en TPL

Gebruik de .NET CLI om een nieuw console-applicatie-project aan te maken:

```
dotnet new console
```

Voeg de volgende code toe om 2 achtergrondtaken te starten.

```csharp
var task1 = new Task(() => SchrijfNaarConsole("Taak 1"));
var task2 = new Task(() => SchrijfNaarConsole("Taak 2"));

task1.Start();
task2.Start();

SchrijfNaarConsole("Buiten taak 1 en taak 2");

Console.ReadKey();


static void SchrijfNaarConsole(string tekst)
{
    Console.WriteLine($"Geschreven vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}");
}
```

Pas de code als volgt aan:

```csharp

var task1 = new Task<string>(() => SchrijfNaarConsole("Taak 1"));
var task2 = new Task<string>(() => SchrijfNaarConsole("Taak 2"));

task1.Start();
task2.Start();

SchrijfNaarConsole("Buiten taak 1 en taak 2");

while (!task1.IsCompleted && !task2.IsCompleted)
{
    Thread.Sleep(100);
}

Console.WriteLine("Taak 1 is voltooid: " + task1.Result);
Console.WriteLine("Taak 2 is voltooid: " + task2.Result);

Console.ReadKey();


static string SchrijfNaarConsole(string tekst)
{
    var volledigeTekst = $"Geschreven vanuit thread {Thread.CurrentThread.ManagedThreadId}: {tekst}";
    Console.WriteLine(volledigeTekst);
    return volledigeTekst;
}
```

Maak een nieuwe console applicatie, of hergebruik de vorige.

```
dotnet new console
```

Voeg de volgende code toe om in parallel naar priemgetallen te zoeken.

```csharp

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
```

## Async & Await

Maak een nieuwe console applicatie.

```
dotnet new console
```

Voeg de volgende code toe om op een andere manier priemgetallen te zoeken.

```csharp

var taken = Enumerable.Range(0, 10).Select(i => Task.Run(() => VoerEenLangeBerekeningUit(i * 100_000, (i + 1) * 100_000, $"Taak {i}")));

Console.WriteLine("Eerst een pauze...");

await Pauze();

Console.WriteLine("Alle taken gestart");

await Task.WhenAll(taken);

Console.WriteLine("Klaar met alle taken");
Console.ReadKey();

static async Task Pauze()
{
    Console.WriteLine("Pauze gestart");
    await Task.Delay(5000);
    Console.WriteLine("Pauze klaar");
}


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

        if (isPriemgetal)
        {
            aantalPriemgetallen++;
        }
    }

    return aantalPriemgetallen;
}
```

Pas de code als volgt aan:

```csharp
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
```