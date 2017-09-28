using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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

        #region Методы

        public override void AddToDocument(object fillObject)
        {
            PrintListViewModel viewModel = fillObject as PrintListViewModel;

            if (viewModel != null)
            {
                AddToDocument(viewModel.Document, viewModel.DateFrom);
                return;
            }

            throw new ArgumentException("Неподдерживаемый вид шаблона");
        }

        private void AddToDocument(FlowDocument document, DateTime dateFrom)
        {
            Run run = new Run(dateFrom.ToString("MMMM", new CultureInfo("ru-RU")));
            Paragraph paragrath = new Paragraph(run);
            document.Blocks.Add(paragrath);
        }


        #endregion
    }
}
