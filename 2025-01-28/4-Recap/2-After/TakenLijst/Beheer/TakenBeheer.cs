using Mapster;
using Takenlijst.Dto;
using Takenlijst.Models;
using Takenlijst.Repositories;

namespace Takenlijst.Beheer;

// Declareer een klasse die verantwoordelijk is voor het beheer van taken.
public class TakenBeheer
{
    private readonly IRepository<Taak> _repository;

    public TakenBeheer(IRepository<Taak> repository)
    {
        _repository = repository;
    }

    public async Task<List<TaakDto>> GeefAlleTaken()
    {
        var taken = await _repository.HaalAllesOp();
        return taken.Adapt<List<TaakDto>>();
    }

    public async Task<TaakDto> GeefTaak(Guid code)
    {
        var taak = await _repository.HaalOp(code);
        return taak.Adapt<TaakDto>();
    }

    public async Task<TaakDto> VoegTaakToe(ToeTeVoegenTaakDto taak)
    {
        var toeTeVoegenTaak = taak.Adapt<Taak>();
        var toegevoegdeTaak = await _repository.VoegToe(toeTeVoegenTaak);
        return toegevoegdeTaak.Adapt<TaakDto>();
    }

    public async Task<TaakDto> WijzigTaak(Guid code, TeWijzigenTaakDto taak)
    {
        var teWijzigenTaak = taak.Adapt<Taak>();
        var gewijzigdeTaak = await _repository.Wijzig(teWijzigenTaak);
        return gewijzigdeTaak.Adapt<TaakDto>();
    }

    public async Task<TaakDto> VoltooiTaak(Guid code)
    {
        var taak = await _repository.HaalOp(code);

        if (taak != null)
        {
            taak.IsVoltooid = true;
            await _repository.Wijzig(taak);
        }

        return taak.Adapt<TaakDto>();
    }

    public async Task<TaakDto> VerwijderTaak(Guid code)
    {
        var taak = await _repository.Verwijder(code);
        return taak.Adapt<TaakDto>();
    }
}