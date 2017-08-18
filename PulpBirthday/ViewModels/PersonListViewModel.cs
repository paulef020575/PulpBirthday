using PulpBirthday.Classes;
using PulpBirthday.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PulpBirthday
{
    public class PersonListViewModel : ViewModel
    {
        #region Свойства

        private BdDatabase database;

        public ObservableCollection<PersonViewModel> PersonList { get; private set; }

        private int sortingOrder;

        public int SortingOrder
        {
            get { return sortingOrder; }
            set
            {
                sortingOrder = value;
                RefreshSortingList();
            }
        }

        private string searchedText = "";

        public string SearchedText
        {
            get { return searchedText; }
            set
            {
                if (searchedText != value)
                {
                    searchedText = value;
                    NotifyPropertyChanged("SearchedText");

                    RefreshSortingList();
                }
            }
        }

        public bool NotEmptyMask { get { return (SearchedText.Length > 0); } }


        public PersonViewModel selectedPerson;

        public PersonViewModel SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                if (selectedPerson != value)
                {
                    selectedPerson = value;
                    NotifyPropertyChanged("SelectedPerson");
                }
            }
        }

        public bool HasSelectedPerson { get { return (SelectedPerson != null); } }

        #endregion

        #region Конструкторы

        public PersonListViewModel()
        {
            database = BdDatabase.Connect(Settings.Default.Server, Settings.Default.Database,
                                            Settings.Default.User, Settings.Default.Password);

            SortingOrder = 0;
        }

        #endregion

        #region Методы

        private void RefreshSortingList()
        {
            PersonList = PersonViewModel.LoadList(database, SortingOrder, SearchedText);
            NotifyPropertyChanged("PersonList");
        }

        private void ClearSearchedText()
        {
            SearchedText = "";
        }

        private void AddNewPerson()
        {
            PersonViewModel vm = new PersonViewModel();

            if (SetActiveViewModel != null)
                SetActiveViewModel(vm);
        }

        private void EditSelectedPerson()
        {
            PersonViewModel vm = SelectedPerson.Copy();

            if (SetActiveViewModel != null)
                SetActiveViewModel(vm);
        }

        private void DeleteSelectedPerson()
        {
            if (MessageBox.Show(PulpBirthday.Resources.Message.ConfirmDelete, SelectedPerson.ToString(), MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                SelectedPerson.Delete();

                PersonList.Remove(SelectedPerson);
                SelectedPerson = null;
            }
        }

        private void PrintPersonList()
        {
            PrintListViewModel vm = new PulpBirthday.PrintListViewModel();
            if (SetActiveViewModel != null)
                SetActiveViewModel(vm);
        }

        #endregion


        #region Команды

        #region ClearMask

        private RelayCommand clearMaskCommand;

        public RelayCommand ClearMask
        {
            get
            {
                if (clearMaskCommand == null)
                    clearMaskCommand = new RelayCommand(param => ClearSearchedText(), param => NotEmptyMask);

                return clearMaskCommand;
            }
        }

        #endregion

        #region AddPerson

        private RelayCommand addPersonCommand;

        public RelayCommand AddPerson
        {
            get
            {
                if (addPersonCommand == null)
                    addPersonCommand = new RelayCommand(param => AddNewPerson());

                return addPersonCommand;
            }
        }

        #endregion

        #region EditPerson

        private RelayCommand editPersonCommand;

        public RelayCommand EditPerson
        {
            get
            {
                if (editPersonCommand == null)
                    editPersonCommand = new RelayCommand(param => EditSelectedPerson(), param => HasSelectedPerson);

                return editPersonCommand;
            }
        }

        #endregion

        #region DeletePerson

        private RelayCommand deletePersonCommand;

        public RelayCommand DeletePerson
        {
            get
            {
                if (deletePersonCommand == null)
                    deletePersonCommand = new RelayCommand(param => DeleteSelectedPerson(), param => HasSelectedPerson);

                return deletePersonCommand;
            }
        }

        #endregion

        #region RefreshList

        private RelayCommand refreshListCommand;

        public RelayCommand RefreshList
        {
            get
            {
                if (refreshListCommand == null)
                    refreshListCommand = new RelayCommand(param => RefreshSortingList());

                return refreshListCommand;
            }
        }

        #endregion

        #region PrintList

        private RelayCommand printListCommand;

        public RelayCommand PrintList
        {
            get
            {
                if (printListCommand == null)
                    printListCommand = new RelayCommand(param => PrintPersonList(), param => (PersonList.Count
                     > 0));

                return printListCommand;
            }
        }

        #endregion

        #endregion

        public string Name { get { return "Список сотрудников"; } }

    }
}
