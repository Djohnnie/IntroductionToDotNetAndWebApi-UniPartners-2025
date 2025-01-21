# IntroductionToDotNetAndWebApi-UniPartners-2025
Oefening

## Een WebApi service maken met Dependency Injection

Maak een WebApi (je mag zelf kiezen tussen een minimal-API of een controller-based API) waarin je endpoints voorziet om een takenlijst te beheren.
Je moet dus de volgende zaken kunnen uitvoeren:
- Taken toevoegen
- Taken afvinken (en dus hun status aanpassen)
- Taken ophalen
- Taken verwijderen

Je mag zelf kiezen welke eigenschappen van taken jij belangrijk vindt. Een titel of naam en een vinkje om een taak af te vinken zijn natuurlijk het minste.

Gebruik hiervoor de correcte HTTP Verbs GET, POST, PUT & DELETE.

Gebruik Dependency Injection om te zorgen dat je de takenlijst in het geheugen kunt bijhouden via een Singleton (in de plaats van een echte database).

Gebruik een 'http' bestand om de verschillende endpoints te testen.