using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using CosmeticStore.Domain.Entities;
using CosmeticStore.WPF.Commands;
using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class CustomerOrdersViewModel : ViewModel
{
    public MainWindowViewModel MainModel { get; }

    public CustomerOrdersViewModel() : this(null!) { }

    public CustomerOrdersViewModel(MainWindowViewModel MainModel) => this.MainModel = MainModel;

    #region Orders : IEnumerable<Order>? - Заказы

    /// <summary>Заказы</summary>
    private IEnumerable<Order>? _Orders;

    /// <summary>Заказы</summary>
    public IEnumerable<Order>? Orders
    {
        get => _Orders;
        private set => Set(ref _Orders, value);
    }

    #endregion

    #region SelectedOrder : Order? - Текущий заказ

    /// <summary>Текущий заказ</summary>
    private Order? _SelectedOrder;

    /// <summary>Текущий заказ</summary>
    public Order? SelectedOrder
    {
        get => _SelectedOrder;
        set => Set(ref _SelectedOrder, value);
    }

    #endregion

    #region Command LoadOrdersCommand - Загрузить данные

    /// <summary>Загрузить данные</summary>
    private LambdaCommand? _LoadOrdersCommand;

    /// <summary>Загрузить данные</summary>
    public ICommand LoadOrdersCommand => _LoadOrdersCommand ??= new(OnLoadOrdersCommandExecuted);

    /// <summary>Логика выполнения - Загрузить данные</summary>
    private async void OnLoadOrdersCommandExecuted()
    {
        try
        {
            Orders = await MainModel.OrdersRepository.GetCustomerOrdersAsync(MainModel.UserName!);
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка загрузки данных заказов\r\n{e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}
