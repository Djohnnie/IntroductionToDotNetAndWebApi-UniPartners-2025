namespace Takenlijst.Dto;

public record TaakDto
{
    public Guid Code { get; set; }
    public string Titel { get; set; }
    public bool IsVoltooid { get; set; }
}