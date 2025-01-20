

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