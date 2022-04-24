using System;
using System.Windows.Input;

namespace CosmeticStore.Admin.WPF.Commands.Base;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);
}
