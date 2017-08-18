using PulpBirthday.Classes;
using PulpBirthday.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PulpBirthday
{
    public class PrintListViewModel : ViewModel
    {
        #region Свойства

        private BdDatabase database;


        private DateTime dateFrom;
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (dateFrom != value)
                {
                    dateFrom = value;
                    NotifyPropertyChanged("DateFrom");
                    RefreshDocument();
                }
            }
        }

        public FlowDocument Document { get; private set; }

        #endregion

        #region Конструкторы

        public PrintListViewModel()
        {
            database = BdDatabase.Connect(Settings.Default.Server, Settings.Default.Database,
                                            Settings.Default.User, Settings.Default.Password);

            Document = new FlowDocument();

            DateTime today = DateTime.Today;

            if (today.Day > 15)
            {
                today = today.AddMonths(1);
            }

            DateFrom = new DateTime(today.Year, today.Month, 1);
        }

        #endregion

        #region Методы

        private void RefreshDocument()
        {
            List<Person> personList = Person.LoadList(database, DateFrom, DateFrom.AddMonths(1).AddDays(-1), true, 1, "");

            Document.Blocks.Clear();

            Paragraph paragrath = new Paragraph();
            paragrath.Inlines.Add(DateFrom.ToString("MMMM"));

            Document.Blocks.Add(paragrath);

            foreach (Person person in personList)
            {
                paragrath = new Paragraph();
                paragrath.Inlines.Add(person.Birthday.ToString("dd"));
                paragrath.Inlines.Add(person.Lastname);
                paragrath.Inlines.Add(person.Firstname);
                Document.Blocks.Add(paragrath);
            }

            NotifyPropertyChanged("Document");
        }

        private void ReturnToMainList()
        {
            OnEndEditing();
        }

        private void ChangeMonth(short value)
        {
            DateFrom = DateFrom.AddMonths(value);
        }

        #endregion

        #region Команды

        #region Close

        private RelayCommand closeCommand;

        public RelayCommand Close
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new RelayCommand(param => ReturnToMainList());

                return closeCommand;
            }
        }

        #endregion

        #region NextMonth

        private RelayCommand nextMonthCommand;

        public RelayCommand NextMonth
        {
            get
            {
                if (nextMonthCommand == null)
                    nextMonthCommand = new RelayCommand(param => ChangeMonth(1));
                return nextMonthCommand;
            }
        }

        #endregion

        #region PreviousMonth

        private RelayCommand previousMonthCommand;

        public RelayCommand PreviousMonth
        {
            get
            {
                if (previousMonthCommand == null)
                    previousMonthCommand = new RelayCommand(param => ChangeMonth(-1));
                return previousMonthCommand;
            }
        }

        #endregion

        #endregion
    }
}
