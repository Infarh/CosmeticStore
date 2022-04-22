using System;
using System.Windows;
using CosmeticStore.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CosmeticStore.WPF;

public partial class App
{
    private static IHost? __Host;

    public static IHost Hosting => __Host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IServiceProvider Services => Hosting.Services;

    public static IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
       .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var host = Hosting;

        base.OnStartup(e);

        await host.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using var host = Hosting;

        base.OnExit(e);

        await host.StopAsync();
    }
}
