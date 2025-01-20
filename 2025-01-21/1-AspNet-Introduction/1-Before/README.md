# IntroductionToDotNetAndWebApi-UniPartners-2025
Introductie tot ASP.NET (Webapplicaties, Kestrel, configuratie)

## Een worker service maken

Een worker services is een langdurende achtergrondtaak-applicatie die in de context van een service, daemon, webapplicatie of container kan draaien.

```
dotnet new worker --name WorkerService
```

## Een lege webapplicatie maken

Een lege webapplicatie geeft je de flexibiliteit om te starten met de basis van HTTP en de hele webapplicatie zelf uit te bouwen.

```
dotnet new web --name EmptyWeb
```

## Een MVC webapplicatie maken

Een MVC webapplicatie is een server-side rendered webapplicatie met een UI die opgebouwd is volgens het principe van "Model View Controller".

```
dotnet new mvc --name WebMvc
```

## Een Razor Pages webapplicatie maken

Een Razor webapplicatie is een server-side rendered webapplicatie met een UI die gebruik maakt van Razor Pages.

```
dotnet new razor --name WebRazor
```

## Een Blazor Server webapplicatie maken

Een Blazor Server webapplicatie is een server-side rendered webapplicatie met een UI die gebruik maakt van de nieuwe Blazor engine.

```
dotnet new blazor --name WebBlazor
```

## Een Blazor WebAssembly webapplicatie maken

Een Blazor WebAssembly webapplicatie is een client-side rendered webapplicatie met een UI die gebruik maakt van de nieuwe Blazor engine.

```
dotnet new blazorwasm --name WebWasm
```

## Een WebApi WebService maken

Een WebApi webservice is een HTTP service zonder UI die gebruikt wordt om data uit te wisselen op basis van HTTP en tekstformaten zoals JSON en XML.

```
dotnet new webapi --name WebApi
```

## Een Grpc WebService maken

Een Grpc webservice is een HTTP 2 service zonder UI die gebruikt wordt om data uit te wisselen op basis van HTTP 2 een binair formaat volgens de protobuf specificaties.

```
dotnet new grpc --name WebGrpc
```

## Kestrel

.NET webapplicaties gebruiken een interne in-memory cross-platform webserver onder de naam Kestrel.
Op deze manier kunnen .NET webapplicatie op zich zelf staand opgestart worden en kunnen ze ook eenvoudig in de cloud en/of via containers gehost worden.