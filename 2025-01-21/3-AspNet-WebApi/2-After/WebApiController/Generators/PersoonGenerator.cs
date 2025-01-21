using WebApiController.Model;

namespace WebApiController.Generators;

public class PersoonGenerator
{
    private readonly NaamGenerator _naamGenerator;
    private readonly NummerGenerator _nummerGenerator;

    public PersoonGenerator(NaamGenerator naamGenerator, NummerGenerator nummerGenerator)
    {
        _naamGenerator = naamGenerator;
        _nummerGenerator = nummerGenerator;
    }

    public Persoon MaakPersoon()
    {
        return new(_naamGenerator.GenereerNaam(), _nummerGenerator.GenereerNummer());
    }
}