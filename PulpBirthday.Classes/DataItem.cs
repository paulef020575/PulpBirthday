using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday.Classes
{
    public abstract class DataItem
    {
        #region Свойства

        public int Id { get; protected set; }

        protected virtual string InsertQuery { get { throw new NotImplementedException("InsertQuery"); } }

        protected virtual string UpdateQuery { get { throw new NotImplementedException("UpdateQuery"); } }

        protected virtual string DeleteQuery { get { throw new NotImplementedException("DeleteQuery"); } }

        #endregion

        #region Конструкторы

        public DataItem() { Id = 0; }

        public DataItem(DbDataReader reader) { }

        #endregion

        #region Методы

        public void Save(BdDatabase database)
        {
            if (Id == 0)
            {
                InsertDataItem(database);
            }
            else
            {
                UpdateDataItem(database);
            }
        }

        private void UpdateDataItem(BdDatabase database)
        {
            database.ExecuteQuery(UpdateQuery, CreateParameterList());
        }

        private void InsertDataItem(BdDatabase database)
        {
            Id = (int)database.ExecuteScalar(InsertQuery, CreateParameterList());
        }

        protected virtual Dictionary<string, object> CreateParameterList()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", Id);

            return parameters;
        }

        public void Delete(BdDatabase database)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", Id);

            database.ExecuteQuery(DeleteQuery, parameters);
        }

        public override bool Equals(object obj)
        {
            DataItem item = obj as DataItem;

            if (item != null)
                return Id.Equals(item.Id);

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
