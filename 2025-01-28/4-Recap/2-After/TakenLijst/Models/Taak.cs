namespace Takenlijst.Models;

// Declareer een klasse waarin de gegevens van een taak kunnen worden opgeslagen.
public class Taak : Basis
{
    public string Titel { get; set; }
    public bool IsVoltooid { get; set; }
}