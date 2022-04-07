using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices((_, services) =>
    services.AddSingleton<IWindowManager, WindowManager>())
.Build();

string choise = "";
string name = "";

var windowManager = host.Services.GetRequiredService<IWindowManager>();

do
{
    Console.WriteLine("1-Nowy proces");
    Console.WriteLine("2-Zamknij proces");
    Console.WriteLine("3-Aktualne okno na wierzchu");
    Console.WriteLine("4-Przełącz się na okno poniżej wskazanego");
    Console.WriteLine("0-Wyjdź");

    choise = Console.ReadLine();

    try
    {
        switch (choise)
        {
            case "1":
                Console.WriteLine("Wpisz ścieżkę do procesu:");
                name = Console.ReadLine();
                windowManager.Open(name);
                break;
            case "2":
                Console.WriteLine("Wpisz ścieżkę do procesu:");
                name = Console.ReadLine();
                windowManager.Close(name);
                break;
            case "3":
                Console.WriteLine($"OKNO NA WIERZCHU: {windowManager.GetTopWindow()}");
                break;
            case "4":
                Console.WriteLine("Wpisz ścieżkę do procesu:");
                name = Console.ReadLine();
                windowManager.OpenPrevious(name);
                break;
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex}");
    }

} while (choise != "0");