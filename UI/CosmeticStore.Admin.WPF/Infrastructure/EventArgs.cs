namespace CosmeticStore.Admin.WPF.Infrastructure;

public class EventArgs<T>
{
    public T? Arg { get; init; }

    public EventArgs() { }
    public EventArgs(T? arg) => Arg = arg;

    public static implicit operator T?(EventArgs<T> args) => args.Arg;

    public static implicit operator EventArgs<T>(T? arg) => new(arg);
}
