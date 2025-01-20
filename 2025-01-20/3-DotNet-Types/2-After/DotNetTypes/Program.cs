

string name = "UniPartners";
Console.WriteLine($"Hallo {name}!");
Console.Write("Geef me een getal: ");

string input = Console.ReadLine();

int number = Convert.ToInt32(input);

Console.WriteLine($"Je hebt een getal kleiner dan {number + 1} ingegeven!");


Persoon persoon = new Werknemer { Naam = "Kenny", Functie = "Dingen doen!", Salaris = 2500M };
Console.WriteLine(persoon);

Console.ReadKey();



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