using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PulpBirthday
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> execute;

        readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> _execute, Predicate<object> _canExecute)
        {
            execute = _execute;
            canExecute = _canExecute;
        }

        public RelayCommand(Action<object> _execute)
            : this(_execute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            return (canExecute == null ? true : canExecute(parameter));
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
