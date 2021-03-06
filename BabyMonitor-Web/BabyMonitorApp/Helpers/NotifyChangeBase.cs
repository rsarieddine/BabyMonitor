﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApp.Helpers
{
    public class NotifyChangeBase : INotifyPropertyChanged
    {
        public void NotifyProperty()
        {

        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String PropertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChanged(PropertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) if(null != PropertyChanged)PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
