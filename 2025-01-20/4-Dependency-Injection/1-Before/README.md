# IntroductionToDotNetAndWebApi-UniPartners-2025
Dependency Injection in .NET (Introductie, ServiceCollection, ServiceProvider, Singleton/Transient/Scoped)

Gebruik de volgende klassen als basis voor de volgende voorbeeldprojecten:

```csharp
class Basis
{
    static int _volgNummer = 0;
    private int _id = ++_volgNummer;

    public override string ToString()
    {
        return $"{GetType()} ({_id})";
    }
}

class Appel : Basis
{
}

class Banaan : Basis
{
    private readonly Appel _appel;

    public Banaan(Appel appel)
    {
        _appel = appel;
    }

    public override string ToString()
    {
        return $"{base.ToString()} met {_appel}";
    }
}

class Citroen : Basis
{
    private readonly Appel _appel;
    private readonly Banaan _banaan;

    public Citroen(Appel appel, Banaan banaan)
    {
        _appel = appel;
        _banaan = banaan;
    }

    public override string ToString()
    {
        return $"{base.ToString()} met {_appel} en {_banaan}";
    }
}
```

## Introductie

Maak een nieuwe console applicatie

```
dotnet new console
```

Voeg de volgende klassen toe aan je project:

```csharp
class Registratie
{
    public Type Type { get; set; }
    public DependencyModus Modus { get; set; }

    public Object Instance { get; set; }
}

enum DependencyModus
{
    Vergankelijk,
    Singleton
}
```

Voeg de volgende klasse toe aan je project:

```csharp
class DependencyInjection
{
    private List<Registratie> _registraties = new List<Registratie>();

    public void Registreer<T>(DependencyModus modus = DependencyModus.Vergankelijk)
    {
        _registraties.Add(new Registratie
        {
            Type = typeof(T),
            Modus = modus
        });
    }

    public T Maak<T>()
    {
        return (T)Maak(typeof(T));
    }

    public Object Maak(Type type)
    {
        var registratie = _registraties.FirstOrDefault(r => r.Type == type);
        if (registratie == null)
        {
            throw new Exception("Type niet geregistreerd");
        }

        if (registratie.Modus == DependencyModus.Singleton && registratie.Instance != null)
        {
            return registratie.Instance;
        }

        var constructorParameters = new List<Object>();
        var constructors = registratie.Type.GetConstructors();
        if (constructors.Length == 1)
        {
            foreach (var parameter in constructors[0].GetParameters())
            {
                var parameterObject = Maak(parameter.ParameterType);
                constructorParameters.Add(parameterObject);
            }
        }

        var instance = Activator.CreateInstance(registratie.Type, constructorParameters.ToArray());

        if (registratie.Modus == DependencyModus.Singleton)
        {
            registratie.Instance = instance;
        }

        return instance;
    }
}
```

Voeg de volgende code toe aan je project:

```csharp
var verzameling = new DependencyInjection();
verzameling.Registreer<Appel>();
verzameling.Registreer<Banaan>(DependencyModus.Singleton);
verzameling.Registreer<Citroen>();

var appel = verzameling.Maak<Appel>();
Console.WriteLine(appel);

var banaan = verzameling.Maak<Banaan>();
Console.WriteLine(banaan);

var citroen = verzameling.Maak<Citroen>();
Console.WriteLine(citroen);
```

## ServiceCollection & Singleton/Transient/Scoped

Maak een nieuwe console applicatie

```
dotnet new console
```

Voeg de volgende code toe:

```csharp
var services = new ServiceCollection();
services.AddTransient<Appel>();
services.AddSingleton<Banaan>();
services.AddTransient<Citroen>();
var provider = services.BuildServiceProvider();

var appel = provider.GetService<Appel>();
Console.WriteLine(appel);

var banaan = provider.GetService<Banaan>();
Console.WriteLine(banaan);

var citroen = provider.GetService<Citroen>();
Console.WriteLine(citroen);
```

## Scoped ServiceProvider

Maak een nieuwe console applicatie

```
dotnet new console
```

Voeg de volgende code toe:

```csharp
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();
services.AddTransient<Appel>();
services.AddScoped<Banaan>();
services.AddTransient<Citroen>();
var provider = services.BuildServiceProvider();


var appel = provider.GetService<Appel>();
Console.WriteLine(appel);

var banaan = provider.GetService<Banaan>();
Console.WriteLine(banaan);

using (var scope = provider.CreateScope())
{
    var citroen = scope.ServiceProvider.GetService<Citroen>();
    Console.WriteLine(citroen);
}
```