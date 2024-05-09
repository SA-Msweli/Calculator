using System.Windows.Input;

namespace Calculator.Commands
{
  public abstract class PressButtonCommand : ICommand
  {
    private readonly Action execute;
    private readonly Func<bool> canExecute;
    private readonly List<WeakReference> canExecuteChangedHandlers;

    protected PressButtonCommand()
    {
      canExecuteChangedHandlers = new List<WeakReference>();
    }
    
    public PressButtonCommand(Action execute, Func<bool> canExecute) : this()
    {
      if (execute == null)
        throw new ArgumentNullException("execute");
      this.execute = execute;
      this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
        canExecuteChangedHandlers.Add(new WeakReference(value));
      }
      remove
      {
        var reference = canExecuteChangedHandlers.First(r => r.Target == value);
        canExecuteChangedHandlers.Remove(reference);
        CommandManager.RequerySuggested -= value;
      }
    }

    public virtual void Execute(object? parameter)
    {
      execute();
    }

    public virtual bool CanExecute(object? parameter)
    {
      if (canExecute == null)
        return true;
      return canExecute();
    }
  }

  public class PressButtonCommand<T> : PressButtonCommand
  {
    private readonly Action<T> execute;

    public PressButtonCommand(Action<T> execute)
    {
      this.execute = execute;
    }
    public override void Execute(object parameter)
    {
      execute((T)parameter);
    }
  }
}
