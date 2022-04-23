using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.WPF.Commands;
using CosmeticStore.WPF.ViewModels.Base;
using CosmeticStore.WPF.Views.Windows;

namespace CosmeticStore.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly IRepository<Product> _ProductsRepository;
    private readonly IRepository<Category> _CategoriesRepository;

    public MainWindowViewModel(
        IRepository<Product> ProductsRepository,
        IRepository<Category> CategoriesRepository
        )
    {
        _ProductsRepository = ProductsRepository;
        _CategoriesRepository = CategoriesRepository;
    }

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

    #region Categories : IEnumerable<CategoryViewModel>? - Список категорий

    /// <summary>Список категорий</summary>
    private IEnumerable<CategoryViewModel>? _Categories;

    /// <summary>Список категорий</summary>
    public IEnumerable<CategoryViewModel>? Categories
    {
        get => _Categories;
        private set
        {
            if (!Set(ref _Categories, value)) return;
            SelectedCategory = null;
        }
    }

    #endregion

    #region SelectedCategory : CategoryViewModel? - Выбранная категория

    /// <summary>Выбранная категория</summary>
    private CategoryViewModel? _SelectedCategory;

    /// <summary>Выбранная категория</summary>
    public CategoryViewModel? SelectedCategory
    {
        get => _SelectedCategory;
        set
        {
            if (!Set(ref _SelectedCategory, value)) return;

        }
    }

    private async void LoadCategoryProducts(CategoryViewModel Category)
    {
        try
        {
            //var products = await _CategoriesRepository.
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении товаров категории:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Products : IEnumerable<ProductViewModel> - Список товаров

    /// <summary>Список товаров</summary>
    private IEnumerable<ProductViewModel> _Products = null!;

    /// <summary>Список товаров</summary>
    public IEnumerable<ProductViewModel> Products
    {
        get => _Products;
        private set
        {
            if(!Set(ref _Products, value)) return;
            SelectedProduct = null;
        }
    }

    #endregion

    #region SelectedProduct : ProductViewModel? - Выбранный товар

    /// <summary>Выбранный товар</summary>
    private ProductViewModel? _SelectedProduct;

    /// <summary>Выбранный товар</summary>
    public ProductViewModel? SelectedProduct
    {
        get => _SelectedProduct;
        set => Set(ref _SelectedProduct, value);
    }

    #endregion

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

    #region Command UpdateDataCommand - Загрузка данных

    /// <summary>Загрузка данных</summary>
    private LambdaCommand? _UpdateDataCommand;

    /// <summary>Загрузка данных</summary>
    public ICommand UpdateDataCommand => _UpdateDataCommand
        ??= new(OnUpdateDataCommandExecuted, CanUpdateDataCommandExecute);

    /// <summary>Проверка возможности выполнения - Загрузка данных</summary>
    private bool CanUpdateDataCommandExecute() => _UpdateDataCancellation is null;

    private CancellationTokenSource? _UpdateDataCancellation;
    /// <summary>Логика выполнения - Загрузка данных</summary>
    private async void OnUpdateDataCommandExecuted()
    {
        var cancellation = new CancellationTokenSource();
        _UpdateDataCancellation = cancellation;
        try
        {
            var categories = await _CategoriesRepository.GetAllAsync(cancellation.Token);
            Categories = categories
               .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                })
               .ToArray();
        }
        catch (OperationCanceledException e) when(e.CancellationToken == cancellation.Token)
        {

        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка при получении данных:\r\n{e.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        _UpdateDataCancellation = null;
    }

    #endregion
}
