# IntroductionToDotNetAndWebApi-UniPartners-2025
.NET projectstructuur (Solution, Project, Assembly/Class Library, ...)

## Een .NET applicatie maken

Gebruik de .NET CLI om een nieuwe solution file aan te maken:
```
dotnet new sln --name Solution
```

Gebruik de .NET CLI om alle projecten in een solution file op te lijsten:
```
dotnet sln list
```

Gebruik de .NET CLI om een console-applicatie-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new console --name ConsoleProject --output ConsoleProject
dotnet sln add .\ConsoleProject\ConsoleProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een class-library-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new classlib --name LibraryProject --output LibraryProject
dotnet sln add .\LibraryProject\LibraryProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een class-library-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new classlib --name LibraryProject --output LibraryProject
dotnet sln add .\LibraryProject\LibraryProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een WPF-UI-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new wpf --name WpfProject --output WpfProject
dotnet sln add .\WpfProject\WpfProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een WinForms-UI-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new winforms --name WinFormsProject --output WinFormsProject
dotnet sln add .\WinFormsProject\WinFormsProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een Worker-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new worker --name WorkerProject --output WorkerProject
dotnet sln add .\WorkerProject\WorkerProject.csproj
dotnet sln list
```

Gebruik de .NET CLI om een WebApi-project aan te maken en deze aan de solution toe te voegen:
```
dotnet new webapi --name WebApiProject --output WebApiProject
dotnet sln add .\WebApiProject\WebApiProject.csproj
dotnet sln list
```

Kijk [hier](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new) voor een overzicht van alle mogelijke templates!