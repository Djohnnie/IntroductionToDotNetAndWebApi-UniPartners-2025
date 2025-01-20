# IntroductionToDotNetAndWebApi-UniPartners-2025
Introductie tot het .NET platform (Een beetje algemene info om .NET Core/Framework, runtime, BCL, C# te kaderen)

## Een .NET Console applicatie maken

Gebruik de .NET CLI om een nieuw console-applicatie-project aan te maken:
```
dotnet new console
```

Open het gecreëerde project en wijzig het als volgt:
```csharp
var name = "UniPartners";

Console.WriteLine($"Hello, {name}!");
Console.ReadKey();
```

Gebruik opnieuw de .NET CLI om het project uit te voeren:
```
dotnet run
```

Gebruik de .NET CLI om het project te publiceren:
```
dotnet publish
```

Gebruik ILSpy om de gepubliceerde applicatie te decompilen:
[ILSpy 9.0](https://github.com/icsharpcode/ILSpy/releases/tag/v9.0-rc)

```csharp
using System;
using System.Runtime.CompilerServices;

[CompilerGenerated]
internal class Program
{
	private static void <Main>$(string[] args)
	{
		string name = "UniPartners";
		Console.WriteLine("Hello, " + name + "!");
		Console.ReadKey();
	}
}
```

Gebruik opnieuw de .NET CLI om het project te publiceren voor Windows x64:
```
dotnet publish --runtime win-x64 --self-contained
```

Gebruik opnieuw de .NET CLI om het project te publiceren als single executable voor Windows x64:
```
dotnet publish --runtime win-x64 --self-contained -p:PublishTrimmed=true -p:PublishSingleFile=true
```