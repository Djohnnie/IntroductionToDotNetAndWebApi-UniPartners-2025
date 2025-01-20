# IntroductionToDotNetAndWebApi-UniPartners-2025
.NET Types (Static Typed language, Generics, Reflection)

## C# is een Static Typed programmeertaal

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende klassen toe aan je project:

```csharp
class Persoon
{
    private string _naam;
    public string Naam
    {
        get => _naam;
        set => _naam = value;
    }

    public override string ToString()
    {
        return Naam;
    }
}

class Werknemer : Persoon
{
    public string Functie { get; set; }
    public decimal Salaris { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()} werkt als {Functie} en verdient {Salaris} euro.";
    }
}
```

Voeg de volgende code toe aan je project:

```csharp
string name = "UniPartners";
Console.WriteLine($"Hallo {name}!");
Console.Write("Geef me een getal: ");

string input = Console.ReadLine();

int number = Convert.ToInt32(input);

Console.WriteLine($"Je hebt een getal kleiner dan {number + 1} ingegeven!");


Persoon persoon = new Werknemer { Naam = "Kenny", Functie = "Dingen doen!", Salaris = 2500M };
Console.WriteLine(persoon);

Console.ReadKey();
```

## Value Types vs. Reference Types

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende klasse en struct toe aan je project:

```csharp
class Persoon
{
    public required string Naam {get; set;}

    override public string ToString()
    {
        return Naam;
    }
}

struct Hond
{
    public required string Naam {get; set;}

    override public string ToString()
    {
        return Naam;
    }
}
```

Voeg de volgende code toe aan je project:

```csharp
var getal = 5;
Console.WriteLine(getal);
Helper.DoeIets(getal);
Console.WriteLine(getal);

var persoon = new Persoon { Naam = "Kenny" };
Console.WriteLine(persoon);
Helper.DoeIets(persoon);
Console.WriteLine(persoon);

var hond = new Hond { Naam = "Bobby" };
Console.WriteLine(hond);
Helper.DoeIets(hond);
Console.WriteLine(hond);

Console.WriteLine(hond);
Helper.DoeIets(ref hond);
Console.WriteLine(hond);


class Helper
{
    public static void DoeIets(int getal)
    {
        getal += 1;
    }

    public static void DoeIets(Persoon persoon)
    {
        persoon.Naam = "Iets anders";
    }

    public static void DoeIets(Hond hond)
    {
        hond.Naam = "Iets anders";
    }

    public static void DoeIets(ref Hond hond)
    {
        hond.Naam = "Iets anders";
    }
}
```

## Struct vs. Class vs. Record

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende types toe aan je project:

```csharp
struct Persoon
{
    public required string Naam { get; set; }

    public override string ToString()
    {
        return Naam;
    }
}

class Dier
{
    public required string Naam { get; set; }

    public override string ToString()
    {
        return Naam;
    }
}

record Voertuig
{
    public required string Naam { get; init; }
}
```

Voeg de volgende code toe aan je project:

```csharp
var persoon = new Persoon { Naam = "Kenny" };
persoon.Naam = "Iets anders";
Console.WriteLine(persoon);

var dier = new Dier { Naam = "Bobby" };
dier.Naam = "Iets anders";
Console.WriteLine(dier);

var voertuig = new Voertuig { Naam = "Auto" };
voertuig = voertuig with { Naam = "Iets anders" };
Console.WriteLine(voertuig);


Console.ReadKey();
```

## Generics

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende code toe aan je console project:

```csharp
var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

foreach( var number in array)
{
    Console.WriteLine(number);
}

var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

foreach( var number in list)
{
    Console.WriteLine(number);
}
```

Voeg de volgende code toe aan je console project:

```csharp
var resultaat = DoeIets();
Console.WriteLine(resultaat.Item1);
Console.WriteLine(resultaat.Item2);

var (nummer, tekst) = DoeIetsNieuws();
Console.WriteLine(nummer);
Console.WriteLine(tekst);


static Tuple<int, string> DoeIets()
{
    return new Tuple<int, string>(1, "Hallo");
}

static (int, string) DoeIetsNieuws()
{
    return (1, "Hallo");
}
```

Voeg de volgende klasse toe aan je project:

```csharp
public class Vector<T> where T : struct
{
    public T X { get; set; }
    public T Y { get; set; }

    public override string ToString()
    {
        return $"{{{X};{Y}}}";
    }
}
```

Gebruik deze klasse als volgt:

```csharp
var vektor = new Vector<decimal> { X = 3.0M, Y = 4.5M };
Console.WriteLine(vektor);
```

## Delegates

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende code toe aan je project:

```csharp
var som = Hulp.Som(5, 4);
var somDelegate = new Hulp.SomDelegate(Hulp.Som);

som = somDelegate(81, 19);

som = DubbeleSom(1, 2, Hulp.Som);
som = DubbeleSom(1, 2, (a, b) => a + b);

static int DubbeleSom(int a, int b, Hulp.SomDelegate somDelegate)
{
    return somDelegate(a, b) + somDelegate(a, b);
}

class Hulp
{
    public delegate int SomDelegate(int a, int b);

    public static int Som(int a, int b)
    {
        return a + b;
    }
}
```

Voeg de volgende code toe aan je project:

```csharp
som = NieuweDubbeleSom(1, 2, (a, b) => a + b);

static int NieuweDubbeleSom(int a, int b, Func<int, int, int> somDelegate)
{
    return somDelegate(a, b) + somDelegate(a, b);
}
```

## Extension Methods

Maak een nieuwe console applicatie:

```
dotnet new console
```

Voeg de volgende code toe:

```csharp
Console.Write("Geef een stukje tekst in kleine letters: ");
string invoer = Console.ReadLine();

string resultaat = GeefElkWoordHoofdletters(invoer);
Console.WriteLine($"Elk woord met hoofletter: {resultaat}");

Console.ReadKey();

static string GeefElkWoordHoofdletters(string invoer)
{
    string[] woorden = invoer.Split(' ');
    string resultaat = "";

    foreach (string woord in woorden)
    {
        string eersteLetter = woord.Substring(0, 1).ToUpper();
        string restVanHetWoord = woord.Substring(1);
        resultaat += eersteLetter + restVanHetWoord + " ";
    }

    return resultaat;
}
```

Maak een nieuwe klasse met de 'GeefElkWoordHoofdletters' methode als volgt:

```csharp
static class StringExtensions
{
    public static string GeefElkWoordHoofdletters(this string invoer)
    {
        string[] woorden = invoer.Split(' ');
        string resultaat = "";

        foreach (string woord in woorden)
        {
            string eersteLetter = woord.Substring(0, 1).ToUpper();
            string restVanHetWoord = woord.Substring(1);
            resultaat += eersteLetter + restVanHetWoord + " ";
        }

        return resultaat;
    }
}
```

Pas de code zo aan dat we de extension method kunnen gebruiken:

```csharp
Console.Write("Geef een stukje tekst in kleine letters: ");
string invoer = Console.ReadLine();

string resultaat = invoer.GeefElkWoordHoofdletters();
Console.WriteLine($"Elk woord met hoofletter: {resultaat}");

Console.ReadKey();
```