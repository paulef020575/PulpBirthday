using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PulpBirthday
{
    [Serializable]
    public class Template
    {
        #region Свойства

        public bool IsPersonList { get; set; }

        public TemplateItem[] Items { get; set; }

        #endregion

        #region Конструкторы

        public Template() : this(true)
        {
        }

        public Template(bool isPersonList)
        {
            IsPersonList = isPersonList;
            Items = new TemplateItem[0];
        }

        public static Template Open(string fileName)
        {
            XmlSerializer serializer = GetSerializer();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                return (Template)serializer.Deserialize(fs);
            }
        }

        #endregion

        #region Методы

        public void Save(string fileName, IList<TemplateItem> items)
        {
            Items = items.ToArray();

            XmlSerializer serializer = GetSerializer();

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }

        private static XmlSerializer GetSerializer()
        {
            return new XmlSerializer(typeof(Template));
        }

        #endregion
    }
}
