using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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

        public override void AddToDocument(object fillObject)
        {
            PrintListViewModel viewModel = fillObject as PrintListViewModel;

            if (viewModel != null)
            {
                AddToDocument(viewModel.Document, Text);
                return;
            }

            throw new ArgumentException("Неподдерживаемый вид шаблона");
        }

        private void AddToDocument(FlowDocument document, string text)
        {
            Paragraph p = new Paragraph(new Run(text));
            document.Blocks.Add(p);
        }
    }
}
