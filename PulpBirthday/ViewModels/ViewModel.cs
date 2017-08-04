using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    public class ViewModel : INotifyPropertyChanged
    {
        public bool IsModified { get; private set; }

        protected ViewModel() { IsModified = false; }

        private PropertyChangedEventHandler onPropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { onPropertyChanged += value; }
            remove { onPropertyChanged -= value; }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            SetModified();
        }

        protected void ClearModified()
        {
            IsModified = false;
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs("IsModified"));
        }

        protected void SetModified()
        {
            IsModified = true;
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs("IsModified"));
        }
    }
}
