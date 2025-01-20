using Microsoft.Extensions.DependencyInjection;


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