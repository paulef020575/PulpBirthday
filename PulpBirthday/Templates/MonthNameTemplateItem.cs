using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    [Serializable]
    public class MonthNameTemplateItem : TemplateItem
    {
        #region Конструкторы

        public MonthNameTemplateItem()
            : this(0)
        { }

        public MonthNameTemplateItem(int id)
            : base(id, TemplateItemType.MonthName)
        { }

        #endregion
    }
}
