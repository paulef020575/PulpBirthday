using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    public class MainViewModel : ViewModel
    {
        private ViewModel currentItem;

        public ViewModel CurrentItem
        {
            get { return currentItem; }
            private set
            {
                currentItem = value;
                currentItem.SetActiveViewModel = SetCurrentItem;

                NotifyPropertyChanged("CurrentItem");
            }
        }

        public MainViewModel()
        {
            RestoreDefaultViewModel();
        }

        private void SetCurrentItem(ViewModel viewModel)
        {
            viewModel.EndEditing += ChildEndEditing;
            CurrentItem = viewModel;
        }

        private void RestoreDefaultViewModel()
        {
            PersonListViewModel listVM = new PulpBirthday.PersonListViewModel();

            SetCurrentItem(listVM);
        }

        private void ChildEndEditing(object sender, EventArgs e)
        {
            RestoreDefaultViewModel();
        }
    }
}
