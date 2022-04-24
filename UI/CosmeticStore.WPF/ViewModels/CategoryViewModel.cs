using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class CategoryViewModel : ViewModel
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
}
