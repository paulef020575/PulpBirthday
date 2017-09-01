using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PulpBirthday
{
    [Serializable]
    [XmlInclude(typeof(ImageBlockTemplateItem))]
    [XmlInclude(typeof(MonthNameTemplateItem))]
    [XmlInclude(typeof(PersonListTemplateItem))]
    [XmlInclude(typeof(PersonNameTemplateItem))]
    [XmlInclude(typeof(TextBlockTemplateItem))]
    public class TemplateItem
    {
        #region Свойства

        public int Id { get; set; }

        public TemplateItemType ItemType { get; set; }

        public bool IsTextBlock { get { return (ItemType == TemplateItemType.TextBlock); } }

        public bool IsImageBlock { get { return (ItemType == TemplateItemType.ImageBlock); } }

        #endregion

        #region Конструкторы

        private TemplateItem() { }

        public TemplateItem(int id, TemplateItemType itemType)
        {
            Id = id;
            ItemType = itemType;
        }

        #endregion

        #region Методы

        public override string ToString()
        {
            switch (ItemType)
            {
                case TemplateItemType.MonthName:
                    return "<НазваниеМесяца>";

                case TemplateItemType.PersonList:
                    return "<Список>";

                case TemplateItemType.PersonName:
                    return "<ИмяАдресата>";

                case TemplateItemType.TextBlock:
                    return "<ПроизвольныйТекст>";

                case TemplateItemType.ImageBlock:
                    return "<Изображение>";

                default:
                    return "<НеизвестныйВидБлока>";
            }
        }

        public override bool Equals(object obj)
        {
            TemplateItem item = obj as TemplateItem;

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
