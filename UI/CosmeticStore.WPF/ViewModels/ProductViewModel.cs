using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CosmeticStore.WebAPI.Clients.Repositories;
using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class ProductViewModel : ViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }

    #region Image : ImageSource - Изображение

    /// <summary>Изображение</summary>
    private ImageSource? _Image;

    /// <summary>Изображение</summary>
    public ImageSource? Image { get => _Image; private set => Set(ref _Image, value); }

    #endregion

    public Task LoadImageAsync(ImagesClient Client, string? Address) => Address is not { Length: > 0 } 
        ? Task.CompletedTask 
        : Task.Run(async () =>
        {
            var stream = await Client.GetImageAsync(Address);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = stream;
                img.EndInit();
                Image = img;
            });
        });
}
