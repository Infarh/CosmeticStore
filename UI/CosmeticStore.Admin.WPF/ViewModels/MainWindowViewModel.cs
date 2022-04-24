using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using CosmeticStore.Admin.WPF.Commands;
using CosmeticStore.Admin.WPF.ViewModels.Base;
using CosmeticStore.Admin.WPF.Views.Windows;
using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;
using CosmeticStore.Interfaces.Repositories;
using CosmeticStore.WebAPI.Clients.Repositories;

namespace CosmeticStore.Admin.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public IProductsRepository ProductsRepository { get; }
    public IRepository<Category> CategoryRepository { get; }
    public ImagesClient ImagesClient { get; }

    public MainWindowViewModel(
        IProductsRepository ProductsRepository,
        IRepository<Category> CategoryRepository,
        ImagesClient ImagesClient)
    {
        this.ProductsRepository = ProductsRepository;
        this.CategoryRepository = CategoryRepository;
        this.ImagesClient = ImagesClient;
    }

    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    private string _Title = "Админка";

    /// <summary>Заголовок окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region Categories : IEnumerable<Category>? - Категории

    /// <summary>Категории</summary>
    private IEnumerable<Category>? _Categories;

    /// <summary>Категории</summary>
    public IEnumerable<Category>? Categories
    {
        get => _Categories;
        private set
        {
            if (!Set(ref _Categories, value)) return;
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
            if (!Set(ref _SelectedCategory, value)) return;
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
            Products = await ProductsRepository.GetCategoryProductsAsync(category_id);
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
            Categories = await CategoryRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка загрузки данных {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Command CreateCategoryCommand - Создать категорию

    /// <summary>Создать категорию</summary>
    private LambdaCommand? _CreateCategoryCommand;

    /// <summary>Создать категорию</summary>
    public ICommand CreateCategoryCommand => _CreateCategoryCommand ??= new(OnCreateCategoryCommandExecuted);

    /// <summary>Логика выполнения - Создать категорию</summary>
    private async void OnCreateCategoryCommandExecuted()
    {
        var max_id = Categories?.Max(c => c.Id) + 1 ?? Random.Shared.Next(1000, 10000);

        var message_model = new TextDialogViewModel
        {
            Title = "Новая категория",
            Message = "Название категории",
            Value = $"Новая категория {max_id}"
        };

        var message_dlg = new TextDialogWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = message_model,
        };

        message_model.Completed += (_, e) =>
        {
            message_dlg.DialogResult = e.Arg;
            message_dlg.Close();
        };

        if (message_dlg.ShowDialog() != true) return;

        var new_category = new Category
        {
            Name = message_model.Value
        };

        try
        {
            var result = await CategoryRepository.AddAsync(new_category);
            Categories = new List<Category>(Categories!) { new_category };
            SelectedCategory = new_category;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка при создании новой категории {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Command RemoveCategoryCommand - Удаление категории

    /// <summary>Удаление категории</summary>
    private LambdaCommand? _RemoveCategoryCommand;

    /// <summary>Удаление категории</summary>
    public ICommand RemoveCategoryCommand => _RemoveCategoryCommand ??= new(OnRemoveCategoryCommandExecuted, p => p is Category);

    /// <summary>Логика выполнения - Удаление категории</summary>
    private async void OnRemoveCategoryCommandExecuted(object? p)
    {
        if (p is not Category { Id: var category_id } category) return;
        if (MessageBox.Show(
                $"Подтверждаете удаление категории {category.Name}",
                "Удаление категории",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No) != MessageBoxResult.Yes)
            return;

        try
        {
            var result = await CategoryRepository.RemoveAsync(category_id);
            if (result is null)
            {
                MessageBox.Show(
                    $"Не удалось удалить категорию {category.Name}", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Categories = Categories?.Where(c => c.Id != category_id).ToArray();
            SelectedCategory = null;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка в процессе удаления категории {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Command CreateProductCommand - Создать товар

    /// <summary>Создать товар</summary>
    private LambdaCommand? _CreateProductCommand;

    /// <summary>Создать товар</summary>
    public ICommand CreateProductCommand => _CreateProductCommand ??= new(OnCreateProductCommandExecuted, () => SelectedCategory is { });

    /// <summary>Логика выполнения - Создать товар</summary>
    private async void OnCreateProductCommandExecuted()
    {
        if (SelectedCategory is not { } category) return;

        var edit_product_model = new CreateProductDialogViewModel
        {
            Title = "Создание нового товара",
        };

        var window = new CreateProductDialogWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = edit_product_model,
        };

        edit_product_model.Completed += (_, e) =>
        {
            window.DialogResult = e;
            window.Close();
        };

        if (window.ShowDialog() != true) return;

        var new_product = new Product
        {
            Name = edit_product_model.Name,
            Price = edit_product_model.Price,
            Description = edit_product_model.Description,
            Category = category,
            CategoryId = category.Id,
        };

        try
        {
            if (edit_product_model.ImageUrl is { Length: > 0 } image_file_path && File.Exists(image_file_path))
            {
                await using var image_file = File.OpenRead(image_file_path);
                new_product.ImageUrl = Path.GetFileName(image_file_path);
                await ImagesClient.SendImageAsync(new_product.ImageUrl, image_file);
            }

            var result = await ProductsRepository.AddAsync(new_product);
            if (result <= 0)
            {
                MessageBox.Show("Не удалось создать новый товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Products = new List<Product>(Products!) { new_product };
            SelectedProduct = new_product;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка при создании нового товара {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Command RemoveCommand - Удаление товара

    /// <summary>Удаление товара</summary>
    private LambdaCommand? _RemoveCommand;

    /// <summary>Удаление товара</summary>
    public ICommand RemoveProductCommand => _RemoveCommand ??= new(OnRemoveCommandExecuted, p => p is Product);

    /// <summary>Логика выполнения - Удаление товара</summary>
    private async void OnRemoveCommandExecuted(object? p)
    {
        if (p is not Product { Id: var product_id } product)
            return;

        if (MessageBox.Show(
                $"Подтверждаете удаление товара {product.Name}",
                "Удаление товара",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No) != MessageBoxResult.Yes)
            return;

        try
        {
            var result = await ProductsRepository.RemoveAsync(product_id);
            if (result is null)
            {
                MessageBox.Show(
                    $"Не удалось удалить товар {product.Name}", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Products = Products?.Where(c => c.Id != product_id).ToArray();
            SelectedProduct = null;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Ошибка при удалении товара {e.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}
