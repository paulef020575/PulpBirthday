using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday.Classes
{
    public class BdDatabase
    {
        #region Свойства

        public string ConnectionString { get; private set; }

        #endregion


        #region Конструкторы

        private BdDatabase() { }

        public BdDatabase(string connectionString)
        {
            ConnectionString = connectionString;

            try
            {
                ConnectionString = (string)ExecuteScalar(PulpBirthday.Resources.Queries.Common.GetConnectionString);
            }
            catch (FbException exception)
            {
                throw new InvalidOperationException("Ошибка соединения", exception);
            }
        }

        #endregion

        #region Методы

        #region Установка соединения

        public static BdDatabase Connect(string connectionString)
        {
            return new Classes.BdDatabase(connectionString);
        }

        public static  BdDatabase Connect(string serverName, string databaseFile, string userName, string password)
        {
            FbConnectionStringBuilder csBuilder = new FbConnectionStringBuilder();
            csBuilder.DataSource = serverName;
            csBuilder.Database = databaseFile;
            csBuilder.UserID = userName;
            csBuilder.Password = password;

            return Connect(csBuilder.ToString());
        }

        #endregion

        private FbConnection CreateConnection()
        {
            return new FbConnection(ConnectionString);
        }

        private FbCommand CreateCommand(FbConnection connection, string query, Dictionary<string, object> parameters)
        {
            FbCommand command = new FbCommand(query, connection);
            foreach (KeyValuePair<string, object> parameter in parameters)
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);

            return command;
        }

        #region Исполнение запросов

        public void ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            using (FbConnection connection = CreateConnection())
            {
                FbCommand command = CreateCommand(connection, query, parameters);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ExecuteQuery(string query)
        {
            ExecuteQuery(query, new Dictionary<string, object>());
        }

        public object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            using (FbConnection connection = CreateConnection())
            {
                FbCommand command = CreateCommand(connection, query, parameters);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                return result;
            }
        }

        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, new Dictionary<string, object>());
        }

        public FbDataReader ExecuteReader(string query, Dictionary<string, object> parameters)
        {
            FbConnection connection = CreateConnection();

            FbCommand command = CreateCommand(connection, query, parameters);

            connection.Open();
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        public FbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, new Dictionary<string, object>());
        }

        #endregion

        #endregion
    }
}
