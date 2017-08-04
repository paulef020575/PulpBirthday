using PulpBirthday.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

        #region Командыё

        #endregion
    }
}
