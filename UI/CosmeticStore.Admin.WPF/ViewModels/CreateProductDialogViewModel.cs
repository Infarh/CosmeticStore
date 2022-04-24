using System;
using System.Windows.Input;
using CosmeticStore.Admin.WPF.Commands;
using CosmeticStore.Admin.WPF.Infrastructure;
using CosmeticStore.Admin.WPF.ViewModels.Base;
using Microsoft.Win32;

namespace CosmeticStore.Admin.WPF.ViewModels;

public class CreateProductDialogViewModel : ViewModel
{
    public event EventHandler<EventArgs<bool>>? Completed;

    protected virtual void OnCompleted(bool IsSuccess) => Completed?.Invoke(this, IsSuccess);

    #region Title : string - Заголовок

    /// <summary>Заголовок</summary>
    private string _Title = "Новый товар";

    /// <summary>Заголовок</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region Name : string - Название товара

    /// <summary>Название товара</summary>
    private string _Name = "Товар";

    /// <summary>Название товара</summary>
    public string Name { get => _Name; set => Set(ref _Name, value); }

    #endregion

    #region Description : string? - Описание товара

    /// <summary>Описание товара</summary>
    private string? _Description;

    /// <summary>Описание товара</summary>
    public string? Description { get => _Description; set => Set(ref _Description, value); }

    #endregion

    #region Price : decimal - Стоимость

    /// <summary>Стоимость</summary>
    private decimal _Price;

    /// <summary>Стоимость</summary>
    public decimal Price { get => _Price; set => Set(ref _Price, value); }

    #endregion

    #region ImageUrl : string? - Картинка

    /// <summary>Картинка</summary>
    private string? _ImageUrl;

    /// <summary>Картинка</summary>
    public string? ImageUrl { get => _ImageUrl; set => Set(ref _ImageUrl, value); }

    #endregion

    #region Command SelectImageCommand - Выбрать картинку

    /// <summary>Выбрать картинку</summary>
    private LambdaCommand? _SelectImageCommand;

    /// <summary>Выбрать картинку</summary>
    public ICommand SelectImageCommand => _SelectImageCommand ??= new(OnSelectImageCommandExecuted);

    /// <summary>Логика выполнения - Выбрать картинку</summary>
    private void OnSelectImageCommandExecuted()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Выбор изображения",
            Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|Все файлы (*.*)|*.*"
        };

        if (dialog.ShowDialog() != true) return;

        ImageUrl = dialog.FileName;
    }

    #endregion

    #region Command OkCommand - Подтвердить выбор

    /// <summary>Подтвердить выбор</summary>
    private LambdaCommand? _OkCommand;

    /// <summary>Подтвердить выбор</summary>
    public ICommand OkCommand => _OkCommand ??= new(OnOkCommandExecuted);

    /// <summary>Логика выполнения - Подтвердить выбор</summary>
    private void OnOkCommandExecuted() => OnCompleted(true);

    #endregion

    #region Command CancelCommand - Подтвердить выбор

    /// <summary>Подтвердить выбор</summary>
    private LambdaCommand? _CancelCommand;

    /// <summary>Подтвердить выбор</summary>
    public ICommand CancelCommand => _CancelCommand ??= new(OnCancelCommandExecuted);

    /// <summary>Логика выполнения - Подтвердить выбор</summary>
    private void OnCancelCommandExecuted() => OnCompleted(false);

    #endregion
}
