using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class ProductViewModel : ViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }

    private readonly string _ImageUrl = null!;
    public string ImageUrl
    {
        get => _ImageUrl;
        init
        {
            _ImageUrl = value;
            InitializeImageAsync(Path.Combine("Images", value))
               .ContinueWith(t => Image = t.Result, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

    private static async Task<BitmapImage?> InitializeImageAsync(string ImagePath)
    {
        await Task.Delay(Random.Shared.Next(200, 2000));
        if (!File.Exists(ImagePath))
            return null;

        var file_stream = File.OpenRead(ImagePath);
        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = file_stream;
        image.EndInit();

        return image;
    }

    #region Image : ImageSource? - Картинка

    /// <summary>Картинка</summary>
    private ImageSource? _Image;

    /// <summary>Картинка</summary>
    public ImageSource? Image { get => _Image; private set => Set(ref _Image, value); }

    #endregion
}
