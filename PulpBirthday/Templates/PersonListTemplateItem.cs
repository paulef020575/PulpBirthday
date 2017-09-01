using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    [Serializable]
    public class PersonListTemplateItem : TemplateItem
    {
        #region Конструкторы

        public PersonListTemplateItem() : this(0) { }

        public PersonListTemplateItem(int id)
            : base(id, TemplateItemType.PersonList)
        { }

        #endregion
    }
}
