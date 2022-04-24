using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CosmeticStore.Domain.DTO;
using CosmeticStore.Domain.Entities;
using CosmeticStore.WPF.Commands;
using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class CartOrderViewModel : ViewModel
{
    private MainWindowViewModel MainModel { get; } = null!;

    public ObservableCollection<CartItemViewModel> Items { get; } = new();

    #region SelectedItem : CartItemViewModel? - Текущий пункт корзины

    /// <summary>Текущий пункт корзины</summary>
    private CartItemViewModel? _SelectedItem;

    /// <summary>Текущий пункт корзины</summary>
    public CartItemViewModel? SelectedItem { get => _SelectedItem; set => Set(ref _SelectedItem, value); }

    #endregion

    #region PhoneNumber : string - Телефон

    /// <summary>Телефон</summary>
    private string? _PhoneNumber;

    /// <summary>Телефон</summary>
    public string? PhoneNumber { get => _PhoneNumber; set => Set(ref _PhoneNumber, value); }

    #endregion

    #region Description : string? - Описание

    /// <summary>Описание</summary>
    private string? _Description;

    /// <summary>Описание</summary>
    public string? Description { get => _Description; set => Set(ref _Description, value); }

    #endregion

    public int TotalItemsCount => Items.Sum(item => item.Quantity);

    public decimal TotalPrice => Items.Sum(item => item.Quantity * item.Product.Price);

    #region Command IncrementCommand - Увеличить количество

    /// <summary>Увеличить количество</summary>
    private LambdaCommand? _IncrementCommand;

    /// <summary>Увеличить количество</summary>
    public ICommand IncrementCommand => _IncrementCommand ??= new(OnIncrementCommandExecuted, p => p is CartItemViewModel);

    /// <summary>Логика выполнения - Увеличить количество</summary>
    private static void OnIncrementCommandExecuted(object? p)
    {
        if (p is CartItemViewModel item) item.Quantity++;
    }

    #endregion

    #region Command DecrementCommand - Уменьшить количество

    /// <summary>Уменьшить количество</summary>
    private LambdaCommand? _DecrementCommand;

    /// <summary>Уменьшить количество</summary>
    public ICommand DecrementCommand => _DecrementCommand ??= new(OnDecrementCommandExecuted, p => p is CartItemViewModel);

    /// <summary>Логика выполнения - Уменьшить количество</summary>
    private static void OnDecrementCommandExecuted(object? p)
    {
        if (p is CartItemViewModel item) item.Quantity--;
    }

    #endregion

    #region Command RemoveCommand - Удалить

    /// <summary>Удалить</summary>
    private LambdaCommand? _RemoveCommand;

    /// <summary>Удалить</summary>
    public ICommand RemoveCommand => _RemoveCommand ??= new(OnRemoveCommandExecuted, p => p is CartItemViewModel);

    /// <summary>Логика выполнения - Удалить</summary>
    private void OnRemoveCommandExecuted(object? p)
    {
        if (p is CartItemViewModel item) Items.Remove(item);
    }

    #endregion

    #region Command ClearCommand - Очистить корзину

    /// <summary>Очистить корзину</summary>
    private LambdaCommand? _ClearCommand;

    /// <summary>Очистить корзину</summary>
    public ICommand ClearCommand => _ClearCommand ??= new(() => Items.Clear(), () => Items.Count > 0);

    #endregion

    #region Command CheckoutCommand - СФормировать заказ

    /// <summary>СФормировать заказ</summary>
    private LambdaCommand? _CheckoutCommand;

    /// <summary>СФормировать заказ</summary>
    public ICommand CheckoutCommand => _CheckoutCommand ??= new(OnCheckoutCommandExecuted, CanCheckoutCommandExecute);

    /// <summary>Проверка возможности выполнения - СФормировать заказ</summary>
    private bool CanCheckoutCommandExecute()
    {
        if(Items.Count == 0) return false;
        if (PhoneNumber is not { Length: > 0 }) return false;
        return true;
    }

    /// <summary>Логика выполнения - СФормировать заказ</summary>
    private async void OnCheckoutCommandExecuted()
    {
        try
        {
            var order = await MainModel.OrdersRepository.CreateOrderAsync(
                MainModel.UserName ?? throw new InvalidOperationException("Отсутствует имя покупателя"),
                PhoneNumber ?? throw new InvalidOperationException("Отсутствует телефон"),
                Description,
                Items.Select(item => new OrderItemInfo(item.Product.Id, item.Quantity)));

            Items.Clear();

            MessageBox.Show(
                $"Заказ создан. Номер заказа {order.Id}", "Заказ создан",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception e)
        {
            Debug.WriteLine("Ошибка при оформлении заказа {0}", e);

            MessageBox.Show(
                $"В ходе оформления заказа возникла ошибка\r\n{e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    public CartOrderViewModel(MainWindowViewModel? MainModel)
    {
        this.MainModel = MainModel;
        Items.CollectionChanged += OnItemsChanged;
    }

    public CartOrderViewModel() : this(null)
    {
        if (App.IsDesignMode)
        {
            var rnd = new Random(10);
            var test_items = Enumerable.Range(1, 10)
               .Select(i => new CartItemViewModel
               {
                   Product = new()
                   {
                       Id = i,
                       Name = $"Product {i}",
                       Description = $"Product description {i}",
                       Price = rnd.Next(200, 5000)
                   },
                   Quantity = rnd.Next(1, 20)
               });
            Items = new(test_items);
        }
    }

    private void OnItemsChanged(object? Sender, NotifyCollectionChangedEventArgs E)
    {
        switch (E.Action)
        {
            case NotifyCollectionChangedAction.Add:
                Subscribe(E.NewItems);
                break;
            case NotifyCollectionChangedAction.Remove: break;
            case NotifyCollectionChangedAction.Replace: break;
            case NotifyCollectionChangedAction.Move: break;
            case NotifyCollectionChangedAction.Reset: break;
            default: throw new ArgumentOutOfRangeException();
        }

        OnCartStateChanged();
    }

    private void Subscribe(IList? items)
    {
        if (items is not { Count: > 0 }) return;
        foreach (INotifyPropertyChanged item in items)
            item.PropertyChanged += OnItemPropertyChanged;
    }

    private void Unubscribe(IList? items)
    {
        if (items is not { Count: > 0 }) return;
        foreach (INotifyPropertyChanged item in items)
            item.PropertyChanged -= OnItemPropertyChanged;
    }

    private void OnItemPropertyChanged(object? Sender, PropertyChangedEventArgs E)
    {
        if (E.PropertyName == nameof(CartItemViewModel.Quantity) && Sender is CartItemViewModel { Quantity: <= 0 } item) 
            Items.Remove(item);
        OnCartStateChanged();
    }

    private void OnCartStateChanged()
    {
        OnPropertyChanged(nameof(TotalItemsCount));
        OnPropertyChanged(nameof(TotalPrice));
    }

    public void Add(ProductViewModel Product)
    {
        var cart_item = Items.FirstOrDefault(p => p.Product.Id == Product.Id);
        if (cart_item is not null)
            cart_item.Quantity++;
        else
        {
            cart_item = new()
            {
                Product = Product,
            };
            Items.Add(cart_item);
        }
    }
}

public class CartItemViewModel : ViewModel
{
    public ProductViewModel Product { get; init; } = null!;

    #region Quantity : int - Количество

    /// <summary>Количество</summary>
    private int _Quantity = 1;

    /// <summary>Количество</summary>
    public int Quantity
    {
        get => _Quantity;
        set
        {
            if (value < 0) return;
            if (!Set(ref _Quantity, value)) return;
        }
    }

    #endregion
}
