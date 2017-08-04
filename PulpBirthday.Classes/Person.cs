using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday.Classes
{
    public class Person
    {
        #region Свойства

        public int Id { get; private set; }

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public DateTime Birthday { get; set; }

        public bool Female { get; set; }

        public string Email { get; set; }

        public bool IsInList { get; set; }

        public bool IsSending { get; set; }

        #endregion

        #region Конструкторы

        public Person()
        {
        }

        public Person(FbDataReader reader)
        {
            Id = (int)reader["id"];
            Lastname = (string)reader["lastname"];
            Firstname = (string)reader["firstname"];
            Secondname = (string)reader["secondname"];
            Birthday = (DateTime)reader["birthday"];
            Female = (bool)reader["sex"];
            Email = (string)reader["email"];
            IsInList = (bool)reader["isInList"];
            IsSending = (bool)reader["isSending"];
        }

        #endregion

        #region Методы

        public static List<Person> LoadList(BdDatabase database, DateTime dateFrom, DateTime dateTo, bool forList)
        {
            string query = PulpBirthday.Resources.Queries.Person.LoadList;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("dateFrom", dateFrom);
            parameters.Add("dateTo", dateTo);
            parameters.Add("isList", forList);

            List<Person> personList = new List<Person>();

            using (FbDataReader reader = database.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                    personList.Add(new Person(reader));

                reader.Close();
            }

            return personList;
        }

        public static List<Person> LoadList(BdDatabase database)
        {
            DateTime Jan1 = new DateTime(2017, 1, 1);
            DateTime Dec31 = new DateTime(2017, 12, 31);

            return LoadList(database, Jan1, Dec31, false);
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
