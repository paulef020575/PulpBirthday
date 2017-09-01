using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    [Serializable]
    public class TextBlockTemplateItem : TemplateItem
    {
        #region Свойства

        public string Text { get; set; }

        #endregion

        #region Конструкторы

        public TextBlockTemplateItem() : this(0) { }

        public TextBlockTemplateItem(int id)
            : base(id, TemplateItemType.TextBlock)
        { }

        #endregion
    }
}
