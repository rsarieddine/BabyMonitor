using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BabyMonitorApp.Helpers
{
    public class BaseCommand : ICommand
    {
        private Action _task;
        public BaseCommand(Action task)
        {
            _task = task;
            active = true;

        }
        public bool CanExecute(object parameter) => active;

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if(null != _task)_task();
        }

        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; if(null!= CanExecuteChanged) CanExecuteChanged(this, new EventArgs()); }
        }
    }
}
