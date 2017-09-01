using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    [Serializable]
    public class PersonNameTemplateItem : TemplateItem
    {
        #region Конструкторы

        public PersonNameTemplateItem() : this(0) { }

        public PersonNameTemplateItem(int id)
            : base(id, TemplateItemType.PersonName)
        { }

        #endregion
    }
}
