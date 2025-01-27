
var verjaardagen = new List<Verjaardag>
{
    new Verjaardag { Naam = "Piet", Geboortedatum = new DateTime(1990, 1, 1), IsFamilie = true },
    new Verjaardag { Naam = "Klaas", Geboortedatum = new DateTime(1991, 2, 2), IsFamilie = false },
    new Verjaardag { Naam = "Jan", Geboortedatum = new DateTime(1992, 3, 3), IsFamilie = true },
    new Verjaardag { Naam = "Kees", Geboortedatum = new DateTime(1993, 4, 4), IsFamilie = false },
};

Console.WriteLine("-----------------------------");

var familieleden = verjaardagen.Where(v => v.IsFamilie).ToList();
familieleden.ForEach(f => Console.WriteLine($"{f.Naam} is familie."));

Console.WriteLine("-----------------------------");

var gesorteerd = verjaardagen.OrderByDescending(v => v.Geboortedatum).ToList();
gesorteerd.ForEach(g => Console.WriteLine($"{g.Naam} is geboren op {g.Geboortedatum.ToShortDateString()}."));

Console.WriteLine("-----------------------------");

var namen = string.Join(", ", verjaardagen.OrderBy(x => x.Naam).Select(v => v.Naam.ToUpper()));
Console.WriteLine($"De namen zijn: {namen}.");

Console.WriteLine("-----------------------------");



class Verjaardag
{
    public string Naam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public bool IsFamilie { get; set; }
}