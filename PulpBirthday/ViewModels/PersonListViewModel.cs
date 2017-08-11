using PulpBirthday.Classes;
using PulpBirthday.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    public class PersonListViewModel
    {
        private BdDatabase database;

        public ObservableCollection<PersonViewModel> PersonList { get; private set; }

        public PersonListViewModel()
        {
            database = BdDatabase.Connect(Settings.Default.Server, Settings.Default.Database,
                                            Settings.Default.User, Settings.Default.Password);

            PersonList = PersonViewModel.LoadList(database);

            CurrentItem = PersonList;
        }


        public string Name { get { return "Список сотрудников"; } }

        public object CurrentItem { get; private set; }
    }
}
