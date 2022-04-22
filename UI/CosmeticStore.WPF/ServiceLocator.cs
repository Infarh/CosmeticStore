using CosmeticStore.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CosmeticStore.WPF;

public class ServiceLocator
{
    public MainWindowViewModel MainModel => App.Services.GetRequiredService<MainWindowViewModel>();
}
