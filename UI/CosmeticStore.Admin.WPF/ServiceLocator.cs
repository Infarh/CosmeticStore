using CosmeticStore.Admin.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CosmeticStore.Admin.WPF;

public class ServiceLocator
{
    public MainWindowViewModel MainModel => App.Services.GetRequiredService<MainWindowViewModel>();
}
