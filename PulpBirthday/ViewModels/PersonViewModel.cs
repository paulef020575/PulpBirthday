using PulpBirthday.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PulpBirthday
{
    public class PersonViewModel : ViewModel
    {
        private Person person;

        #region Свойства

        public int Id
        {
            get { return person.Id; }
        }

        public string Lastname
        {
            get { return person.Lastname; }
            set
            {
                if (!string.Equals(person.Lastname, value, StringComparison.CurrentCulture))
                {
                    person.Lastname = value;
                    NotifyPropertyChanged("Lastname");
                    NotifyPropertyChanged("Fullname");
                }
            }
        }

        public string Firstname
        {
            get { return person.Firstname; }
            set
            {
                if (!string.Equals(person.Firstname, value, StringComparison.CurrentCulture))
                {
                    person.Lastname = value;
                    NotifyPropertyChanged("Firstname");
                    NotifyPropertyChanged("Fullname");
                }
            }
        }

        public string Secondname
        {
            get { return person.Secondname; }
            set
            {
                if (!string.Equals(person.Secondname, value, StringComparison.CurrentCulture))
                {
                    person.Secondname = value;
                    NotifyPropertyChanged("Secondname");
                    NotifyPropertyChanged("Fullname");
                }
            }
        }

        public DateTime Birthday
        {
            get { return person.Birthday; }
            set
            {
                if (!DateTime.Equals(person.Birthday, value))
                {
                    person.Birthday = value;
                    NotifyPropertyChanged("Birthday");
                }
            }
        }

        public bool Male
        {
            get { return !person.Female; }
            set
            {
                if (person.Female = value)
                {
                    person.Female = !value;
                    NotifyPropertyChanged("Male");
                    NotifyPropertyChanged("Female");
                }
            }
        }

        public bool Female
        {
            get { return person.Female; }
            set
            {
                if (person.Female != value)
                {
                    person.Female = value;
                    NotifyPropertyChanged("Female");
                    NotifyPropertyChanged("Male");
                }
            }
        }

        public string Email
        {
            get { return person.Email; }
            set
            {
                if (person.Email != value)
                {
                    person.Email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        public bool IsInList
        {
            get { return person.IsInList; }
            set
            {
                if (person.IsInList != value)
                {
                    person.IsInList = value;
                    NotifyPropertyChanged("IsInList");
                }
            }
        }

        public bool IsSending
        {
            get { return person.IsSending; }
            set
            {
                if (person.IsSending != value)
                {
                    person.IsSending = value;
                    NotifyPropertyChanged("IsSending");
                }
            }
        }


        public string Fullname
        {
            get
            {
                StringBuilder result = new StringBuilder(Lastname);

                if (Firstname.Length > 0)
                {
                    result.Append(" " + Firstname);

                    if (Secondname.Length > 0)
                    {
                        result.Append(" " + Secondname);
                    }
                }

                return result.ToString();
            }
        }

        public string Shortname
        {
            get
            {
                StringBuilder result = new StringBuilder(Lastname);

                if (Firstname.Length > 0)
                {
                    result.Append(" " + Firstname.Substring(0, 1) + ".");

                    if (Secondname.Length > 0)
                    {
                        result.Append(" " + Secondname.Substring(0, 1) + ".");
                    }
                }

                return result.ToString();
            }
        }

        #endregion

        #region Конструкторы

        public PersonViewModel(Person _person)
        {
            person = _person;

            ClearModified();
        }

        public PersonViewModel() : this(new Person())
        {
            SetModified();
        }

        #endregion

        #region Методы

        public static ObservableCollection<PersonViewModel> LoadList(BdDatabase database,
                    DateTime dateFrom, DateTime dateTo, bool isForList)
        {
            ObservableCollection<PersonViewModel> list = new ObservableCollection<PersonViewModel>();

            List<Person> personList = Person.LoadList(database, dateFrom, dateTo, isForList);

            foreach (Person person in personList)
            {
                list.Add(new PersonViewModel(person));
            }

            return list;
        }

        public static ObservableCollection<PersonViewModel> LoadList(BdDatabase database)
        {
            DateTime Jan1 = new DateTime(2017, 1, 1), Dec31 = new DateTime(2017, 12, 31);

            return LoadList(database, Jan1, Dec31, false);
        }


        public override string ToString()
        {
            return Shortname;
        }

        #endregion

        #region Команды

        #endregion
    }
}
