using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using PulpBirthday.Classes;

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

        public override void AddToDocument(object fillObject)
        {
            PrintListViewModel viewModel = fillObject as PrintListViewModel;

            if (viewModel != null)
            {
                AddToDocument(viewModel.Document, viewModel.PersonList);
                return;
            }

            throw new ArgumentException("Неподдерживаемый вид шаблона");
        }

        private void AddToDocument(FlowDocument document, List<Person> personList)
        {
            List documentList = new List();
            documentList.MarkerStyle = System.Windows.TextMarkerStyle.Box;

            foreach (Person person in personList)
            {
                PersonViewModel vm = new PulpBirthday.PersonViewModel(person);
                Paragraph p = new Paragraph(new Run(vm.Birthday.Day.ToString() + "\t" + vm.Fullname));
                documentList.ListItems.Add(new ListItem(p));
            }

            document.Blocks.Add(documentList);
        }

        #endregion
    }
}
