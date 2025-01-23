using TakenLijstControllerWebApi.Model;

namespace TakenLijstControllerWebApi.Beheer;

public class TakenBeheer
{
    private readonly List<Taak> _taken = new();

    public TakenBeheer()
    {
        // Voeg een standaard taak toe aan de lijst van taken.
        _taken.Add(new Taak(Guid.NewGuid(), "Starten met het gebruiken van taken", false));
    }

    public List<Taak> GeefAlleTaken()
    {
        // Geef een kopie van de lijst van taken terug.
        return _taken;
    }

    public Taak GeefTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        return _taken.FirstOrDefault(t => t.Code == code);
    }

    public Taak VoegTaakToe(Taak taak)
    {
        // Maak een nieuwe taak aan met een unieke code.
        var nieuweTaak = taak with { Code = Guid.NewGuid() };

        // Als er nog geen taak met dezelfde titel bestaan...
        if (!_taken.Any(t => t.Titel == taak.Titel))
        {
            // Voeg de nieuwe taak toe.
            _taken.Add(nieuweTaak);
        }

        return nieuweTaak;
    }

    public Taak WijzigTaak(Taak taak)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == taak.Code);

        // Als de taak bestaat, wijzig deze met de nieuwe gegevens.
        if (bestaandeTaak != null)
        {
            // Verwijder de oude taak en voeg de gewijzigde taak toe.
            _taken.Remove(bestaandeTaak);
            _taken.Add(taak);

            return taak;
        }

        return bestaandeTaak;
    }

    public Taak VoltooiTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == code);

        // Als de taak bestaat, wijzig de status van de taak naar voltooid.
        if (bestaandeTaak != null)
        {
            var voltooideTaak = bestaandeTaak with { IsVoltooid = true };

            // Verwijder de oude taak en voeg de gewijzigde taak toe.
            _taken.Remove(bestaandeTaak);
            _taken.Add(voltooideTaak);

            return voltooideTaak;
        }

        return bestaandeTaak;
    }

    public Taak VerwijderTaak(Guid code)
    {
        // Zoek naar de taak met de gegeven code.
        var bestaandeTaak = _taken.FirstOrDefault(t => t.Code == code);

        // Als de taak bestaat, verwijder deze uit de lijst van taken.
        if (bestaandeTaak != null)
        {
            _taken.Remove(bestaandeTaak);
        }

        return bestaandeTaak;
    }
}