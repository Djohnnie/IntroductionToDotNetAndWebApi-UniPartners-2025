
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