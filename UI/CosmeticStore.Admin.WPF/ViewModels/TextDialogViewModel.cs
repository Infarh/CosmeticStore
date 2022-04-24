using System;
using System.Windows.Input;
using CosmeticStore.Admin.WPF.Commands;
using CosmeticStore.Admin.WPF.Infrastructure;
using CosmeticStore.Admin.WPF.ViewModels.Base;

namespace CosmeticStore.Admin.WPF.ViewModels;

public class TextDialogViewModel : ViewModel
{
    public event EventHandler<EventArgs<bool>>? Completed;

    protected virtual void OnCompleted(bool IsSuccess) => Completed?.Invoke(this, IsSuccess);

    #region Title : string - Заголовок

    /// <summary>Заголовок</summary>
    private string _Title = "Вопрос";

    /// <summary>Заголовок</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region Message : string - Заголовок

    /// <summary>Заголовок</summary>
    private string _Message = "Введите текст";

    /// <summary>Заголовок</summary>
    public string Message { get => _Message; set => Set(ref _Message, value); }

    #endregion

    #region Value : string? - Заголовок

    /// <summary>Заголовок</summary>
    private string? _Value;

    /// <summary>Заголовок</summary>
    public string? Value { get => _Value; set => Set(ref _Value, value); }

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
