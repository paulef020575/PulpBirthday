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
        #region Свойства

        public bool IsModified { get; private set; }

        private Action<ViewModel> setActiveViewModel;

        public Action<ViewModel> SetActiveViewModel
        {
            get { return setActiveViewModel; }
            set { setActiveViewModel = value; }
        }
        
        #endregion

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


        private EventHandler onEndEditing;

        public event EventHandler EndEditing
        {
            add { onEndEditing += value; }
            remove { onEndEditing -= value; }
        }

        protected void OnEndEditing()
        {
            if (onEndEditing != null)
                onEndEditing(this, EventArgs.Empty);
        }
    }
}
