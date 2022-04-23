using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CosmeticStore.Domain.Entities;
using CosmeticStore.WPF.Commands;
using CosmeticStore.WPF.ViewModels.Base;
using CosmeticStore.WPF.Views.Windows;

namespace CosmeticStore.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    #region Title : string - Заголовок главного окна

    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Заголовок главного окна";

    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region UserName : string - Имя пользователя

    /// <summary>Имя пользователя</summary>
    private string? _UserName;

    /// <summary>Имя пользователя</summary>
    public string? UserName { get => _UserName; set => Set(ref _UserName, value); }

    #endregion

    private IEnumerable<ProductViewModel>? _Products;

    public IEnumerable<ProductViewModel>? Products
    {
        get
        {
            if (_Products is not null)
                return _Products;
            InitializeProductAsync();
            return null;
        }
    }

    private async void InitializeProductAsync()
    {
        await Task.Delay(1500);
        var products = Enumerable.Range(1, 10)
           .Select(i => new ProductViewModel
            {
               Id = i,
               Name = $"Товар {i}",
               Description = $"Описание товара {i}",
               ImageUrl = $"Img{i}.jpg",
            })
           .ToArray();
        Set(ref _Products, products, nameof(Products));
    }

    #region Command LoginCommand - Вход в систему

    /// <summary>Вход в систему</summary>
    private LambdaCommand? _LoginCommand;

    /// <summary>Вход в систему</summary>
    public ICommand LoginCommand => _LoginCommand ??= new(OnLoginCommandExecuted);

    /// <summary>Логика выполнения - Вход в систему</summary>
    private void OnLoginCommandExecuted()
    {
        var login_view_model = new LoginWindowViewModel();
        var main_window = Application.Current.MainWindow;
        var login_window = new LoginWindow
        {
            Owner = main_window,
            DataContext = login_view_model
        };
        login_view_model.Login += (_, e) =>
        {
            login_window.DialogResult = true;
            login_window.Close();
        };

        if (login_window.ShowDialog() != true) return;

        UserName = login_view_model.UserName;
    }

    #endregion
}
