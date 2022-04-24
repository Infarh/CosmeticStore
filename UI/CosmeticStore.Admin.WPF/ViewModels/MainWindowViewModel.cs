using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using CosmeticStore.Admin.WPF.Commands;
using CosmeticStore.Admin.WPF.ViewModels.Base;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.Interfaces.Repositories;

namespace CosmeticStore.Admin.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public IProductsRepository ProductsRepository { get; }
    public IRepository<Category> CategoryRepository { get; }

    public MainWindowViewModel(
        IProductsRepository ProductsRepository,
        IRepository<Category> CategoryRepository)
    {
        this.ProductsRepository = ProductsRepository;
        this.CategoryRepository = CategoryRepository;
    }

    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    private string _Title = "Админка";

    /// <summary>Заголовок окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region Catigoties : IEnumerable<Category>? - Категории

    /// <summary>Категории</summary>
    private IEnumerable<Category>? _Catigoties;

    /// <summary>Категории</summary>
    public IEnumerable<Category>? Catigoties
    {
        get => _Catigoties;
        private set
        {
            if(!Set(ref _Catigoties, value)) return;
            SelectedCategory = null;
        }
    }

    #endregion

    #region SelectedCategory : Category? - Выбранная категория

    /// <summary>Выбранная категория</summary>
    private Category? _SelectedCategory;

    /// <summary>Выбранная категория</summary>
    public Category? SelectedCategory
    {
        get => _SelectedCategory;
        set
        {
            if(!Set(ref _SelectedCategory, value)) return;
            UpdateProductsAsync(value);
        }
    }

    private async void UpdateProductsAsync(Category? category)
    {
        if (category is not { Id: var category_id })
        {
            Products = null;
            return;
        }

        try
        {
            Products = await ProductsRepository.GetCategoryProducts(category_id);
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка загрузки данных списка товаров категории {e.Message}",
                "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Products : IEnumerable<Product>? - Список товаров

    /// <summary>Список товаров</summary>
    private IEnumerable<Product>? _Products;

    /// <summary>Список товаров</summary>
    public IEnumerable<Product>? Products
    {
        get => _Products; 
        private set => Set(ref _Products, value);
    }

    #endregion

    #region SelectedProduct : Product? - Выбранный товар

    /// <summary>Выбранный товар</summary>
    private Product? _SelectedProduct;

    /// <summary>Выбранный товар</summary>
    public Product? SelectedProduct
    {
        get => _SelectedProduct; 
        set => Set(ref _SelectedProduct, value);
    }

    #endregion

    #region Command ExitCommand - Выход

    /// <summary>Выход</summary>
    public ICommand ExitCommand { get; } = new LambdaCommand(Application.Current.Shutdown);

    #endregion

    #region Command UpdateDataCommand - Загрузка данных

    /// <summary>Загрузка данных</summary>
    private LambdaCommand? _UpdateDataCommand;

    /// <summary>Загрузка данных</summary>
    public ICommand UpdateDataCommand => _UpdateDataCommand ??= new(OnUpdateDataCommandExecuted);

    /// <summary>Логика выполнения - Загрузка данных</summary>
    private async void OnUpdateDataCommandExecuted()
    {
        try
        {
            Catigoties = await CategoryRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка загрузки данных {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}
