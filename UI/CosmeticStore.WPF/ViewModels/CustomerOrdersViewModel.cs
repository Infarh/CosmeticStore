using System;
using System.Collections.Generic;
using System.Linq;
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

    #region Command CancelOrderCommand - Отмена заказа

    /// <summary>Отмена заказа</summary>
    private LambdaCommand? _CancelOrderCommand;

    /// <summary>Отмена заказа</summary>
    public ICommand CancelOrderCommand => _CancelOrderCommand
        ??= new(OnCancelOrderCommandExecuted, p => p is Order);

    /// <summary>Логика выполнения - Отмена заказа</summary>
    private async void OnCancelOrderCommandExecuted(object? p)
    {
        if (p is not Order { Id: var order_id }) return;

        try
        {
            var removed_order = await MainModel.OrdersRepository.RemoveAsync(order_id);
            if (removed_order is null)
            {
                MessageBox.Show(
                    "Не удалось отменить заказ", "Ошибка отмены",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            SelectedOrder = null;
            Orders = Orders?.Where(order => order.Id != order_id).ToArray();
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Ошибка в процессе выполнения отмены заказа {e.Message}", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion
}
