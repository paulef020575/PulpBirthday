using Microsoft.Win32;
using PulpBirthday.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulpBirthday
{
    public class TemplateViewModel : ViewModel
    {
        #region Свойства

        private Template template;

        private int lastId;

        public bool IsPersonList
        {
            get { return template.IsPersonList; }
        }

        private string fileName;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public ObservableCollection<TemplateItem> Items { get; private set; }

        private TemplateItem selectedItem;

        public TemplateItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    NotifyPropertyChanged("SelectedItem");
                }
            }
        }

        #endregion

        #region Конструкторы 

        public TemplateViewModel(bool isPersonList)
        {
            string defaultFileName = Settings.Default.ListTemplate;
            if (defaultFileName.Length == 0)
            {
                defaultFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
                defaultFileName = Path.Combine(defaultFileName, "Default.template");
            }

            Items = new ObservableCollection<PulpBirthday.TemplateItem>();
            OpenTemplate(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultFileName));
            Items.CollectionChanged += Items_CollectionChanged;
        }


        #endregion

        #region Методы 

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Items");
        }
        private void Add(TemplateItemType type)
        {
            switch (type)
            {
                case TemplateItemType.MonthName:
                    Items.Add(new MonthNameTemplateItem(++lastId));
                    break;

                case TemplateItemType.PersonList:
                    Items.Add(new PersonListTemplateItem(++lastId));
                    break;

                case TemplateItemType.PersonName:
                    Items.Add(new PersonNameTemplateItem(++lastId));
                    break;

                case TemplateItemType.TextBlock:
                    Items.Add(new TextBlockTemplateItem(++lastId));
                    break;

                case TemplateItemType.ImageBlock:
                    Items.Add(new ImageBlockTemplateItem(++lastId));
                    break;

                default:
                    throw new NotImplementedException(type.ToString());
            }
        }

        private bool CanAddItem(TemplateItemType type)
        {
            switch (type)
            {
                case TemplateItemType.PersonList:
                    return IsPersonList;

                case TemplateItemType.PersonName:
                    return !IsPersonList;

                default:
                    return true;
            }
        }

        private void RemoveSelectedItem(TemplateItem item)
        {
            Items.Remove(item);
        }

        private void MoveUp(TemplateItem item)
        {
            int position = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(position - 1, item);
        }

        private  bool CanMoveUp(TemplateItem item)
        {
            return (Items.IndexOf(item) > 0);
        }

        private void MoveDown(TemplateItem item)
        {
            int position = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(position + 1, item);
        }

        private bool CanMoveDown(TemplateItem item)
        {
            return (Items.IndexOf(item) < Items.Count - 1);
        }

        private void SaveTemplate()
        {
            if (fileName.Length == 0)
                SaveTemplateToFile();

            template.Save(fileName, Items);
            Name = Path.GetFileName(fileName);
            Settings.Default.ListTemplate = fileName;
            Settings.Default.Save();
        }

        private void SaveTemplateToFile()
        {
            fileName = SelectNameForSavingFile();

            if (fileName.Length > 0)
                SaveTemplate();
        }


        private string SelectNameForSavingFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Файлы шаблонов|*.template";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Title = "Сохранение шаблона";

            if (fileName.Length > 0)
                saveFileDialog.FileName = fileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return "";
        }

        private void OpenTemplate()
        {
            string openFileName = SelectFileNameForOpening();

            if (openFileName.Length > 0)
            {
                OpenTemplate(openFileName);
            }                
        }

        private void OpenTemplate(string openFileName)
        {
            try
            {
                template = Template.Open(openFileName);
                Items.Clear();
                lastId = 0;
                foreach (TemplateItem item in template.Items)
                {
                    Items.Add(item);
                    lastId = (lastId < item.Id ? item.Id : lastId);
                }

                fileName = openFileName;
                Name = Path.GetFileName(fileName);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Ошибка открытия файла: " + e.Message);
            }
        }

        private string SelectFileNameForOpening()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Файлы шаблонов|*.template";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Открытие шаблона";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return "";
        }

        #endregion

        #region Команды

        #region Добавление элементов

        #region AddItem

        private RelayCommand addItem;

        public RelayCommand AddItem
        {
            get
            {
                if (addItem == null)
                    addItem = new RelayCommand(param => Add((TemplateItemType)param), param => CanAddItem((TemplateItemType)param));
                return addItem;
            }
        }

        #endregion


        #endregion

        #region RemoveItem

        private RelayCommand removeItemCommand;

        public RelayCommand RemoveItem
        {
            get
            {
                if (removeItemCommand == null)
                    removeItemCommand = new RelayCommand(param => RemoveSelectedItem((TemplateItem)param));

                return removeItemCommand;
            }
        }

        #endregion

        #region MoveUpItem

        private RelayCommand moveUpItemCommand;

        public RelayCommand MoveUpItem
        {
            get
            {
                if (moveUpItemCommand == null)
                    moveUpItemCommand = new RelayCommand(param => MoveUp((TemplateItem)param), param => CanMoveUp((TemplateItem)param));
                return moveUpItemCommand;
            }
        }

        #endregion

        #region MoveDownItem

        private RelayCommand moveDownItemCommand;

        public RelayCommand MoveDownItem
        {
            get
            {
                if (moveDownItemCommand == null)
                    moveDownItemCommand = new RelayCommand(param => MoveDown((TemplateItem)param), param => CanMoveDown((TemplateItem)param));
                return moveDownItemCommand;
            }
        }

        #endregion

        #region Save

        private RelayCommand saveCommand;

        public RelayCommand Save
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(param => SaveTemplate(), param => IsModified);

                return saveCommand;
            }
        }

        #endregion

        #region Open

        private RelayCommand openCommand;

        public RelayCommand Open
        {
            get
            {
                if (openCommand == null)
                    openCommand = new RelayCommand(param => OpenTemplate());

                return openCommand;
            }
        }

        #endregion

        #endregion
    }
}
