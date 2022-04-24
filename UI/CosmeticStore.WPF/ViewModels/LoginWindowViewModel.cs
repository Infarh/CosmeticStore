using System;
using System.Windows.Input;
using CosmeticStore.WPF.Commands;
using CosmeticStore.WPF.Infrastructure;
using CosmeticStore.WPF.ViewModels.Base;

namespace CosmeticStore.WPF.ViewModels;

public class LoginWindowViewModel : ViewModel
{
    public event EventHandler<EventArgs<string>>? Login;
    protected virtual void OnLoing(string Name) => Login?.Invoke(this, Name);

    #region Title : string - Заголовок главного окна

    /// <summary>Заголовок главного окна</summary>
    private string _Title = "Вход в систему";

    /// <summary>Заголовок главного окна</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion

    #region UserName : string? - Имя пользователя

    /// <summary>Имя пользователя</summary>
    private string? _UserName;

    /// <summary>Имя пользователя</summary>
    public string? UserName { get => _UserName; set => Set(ref _UserName, value); }

    #endregion

    #region Command LoginCommand - Вход в систему

    /// <summary>Вход в систему</summary>
    private LambdaCommand? _LoginCommand;

    /// <summary>Вход в систему</summary>
    public ICommand LoginCommand => _LoginCommand
        ??= new(OnLoginCommandExecuted, p => p is string { Length: > 0 });

    /// <summary>Логика выполнения - Вход в систему</summary>
    private void OnLoginCommandExecuted(object? p)
    {
        if (p is not string { Length: > 0 } user_name) return;
        OnLoing(user_name);
    }

    #endregion
}
