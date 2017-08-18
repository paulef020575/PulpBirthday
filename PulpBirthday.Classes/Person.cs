using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday.Classes
{
    public class Person : DataItem
    {
        #region Свойства

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public DateTime Birthday { get; set; }

        public bool Female { get; set; }

        public string Email { get; set; }

        public bool IsInList { get; set; }

        public bool IsSending { get; set; }

        #region Запросы

        protected override string InsertQuery
        {
            get
            {
                return PulpBirthday.Resources.Queries.Person.Insert;
            }
        }

        protected override string UpdateQuery
        {
            get
            {
                return PulpBirthday.Resources.Queries.Person.Update;
            }
        }

        protected override string DeleteQuery
        {
            get
            {
                return PulpBirthday.Resources.Queries.Person.Delete;
            }
        }

        #endregion

        #endregion

        #region Конструкторы

        public Person()
            : base()
        {
            Lastname = "";
            Firstname = "";
            Secondname = "";

            Birthday = DateTime.Today;
            Female = false;
            Email = "";
            IsInList = true;
            IsSending = true;
        }

        public Person(FbDataReader reader)
            : base(reader)
        {
            Id = (int)reader["id"];
            Lastname = (string)reader["lastname"];
            Firstname = (string)reader["firstname"];
            Secondname = (string)reader["secondname"];
            Birthday = (DateTime)reader["birthday"];
            Female = ((short)reader["female"] > 0);
            Email = (string)reader["email"];
            IsInList = ((short)reader["isInList"] > 0);
            IsSending = ((short)reader["isSending"] > 0);
        }

        #endregion

        #region Методы

        protected override Dictionary<string, object> CreateParameterList()
        {
            Dictionary<string, object> parameters = base.CreateParameterList();

            parameters.Add("lastname", Lastname);
            parameters.Add("firstname", Firstname);
            parameters.Add("secondname", Secondname);
            parameters.Add("female", (Female ? 1 : 0));
            parameters.Add("birthday", Birthday);
            parameters.Add("email", Email);
            parameters.Add("isInList", IsInList);
            parameters.Add("isSending", IsSending);

            return parameters;
        }


        public static List<Person> LoadList(BdDatabase database, DateTime dateFrom, DateTime dateTo, 
                                            bool forList, int sortingOrder, string mask)
        {
            string query = PulpBirthday.Resources.Queries.Person.LoadListSortedByName;
            if (sortingOrder == 1)
                query = PulpBirthday.Resources.Queries.Person.LoadListSortedByBirthday;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("dateFrom", dateFrom);
            parameters.Add("dateTo", dateTo);
            parameters.Add("isList", forList);
            parameters.Add("mask", mask);

            List<Person> personList = new List<Person>();

            using (FbDataReader reader = database.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                    personList.Add(new Person(reader));

                reader.Close();
            }

            return personList;
        }

        public static List<Person> LoadList(BdDatabase database, string mask)
        {
            DateTime Jan1 = new DateTime(2017, 1, 1);
            DateTime Dec31 = new DateTime(2017, 12, 31);

            return LoadList(database, Jan1, Dec31, false, 0, mask);
        }

        public static Person Load(BdDatabase database, int id)
        {
            string query = PulpBirthday.Resources.Queries.Person.Load;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", id);

            using (FbDataReader reader = database.ExecuteReader(query, parameters))
            {
                Person person = null;
                if (reader.Read())
                {
                    person = new Classes.Person(reader);
                }

                reader.Close();

                if (person == null)
                    throw new ArgumentOutOfRangeException(PulpBirthday.Resources.Message.IdentifierNotFoundError);

                return person;
            }

        }

        #endregion

    }
}
