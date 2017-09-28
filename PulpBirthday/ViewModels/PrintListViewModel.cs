using PulpBirthday.Classes;
using PulpBirthday.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PulpBirthday
{
    public class PrintListViewModel : ViewModel
    {
        #region Свойства

        private BdDatabase database;

        public List<Person> PersonList { get; private set; }

        private DateTime dateFrom;
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set
            {
                if (dateFrom != value)
                {
                    dateFrom = value;
                    NotifyPropertyChanged("DateFrom");
                    RefreshDocument();
                }
            }
        }

        public FlowDocument Document { get; private set; }

        public double FontSize
        {
            get { return Document.FontSize; }
            set
            {
                Document.FontSize = value;
                NotifyPropertyChanged("FontSize");
            }
        }

        public SolidColorBrush FontColor
        {
            get { return (SolidColorBrush)Document.Foreground; }
            set
            {
                Document.Foreground = value;
                NotifyPropertyChanged("FontColor");
            }
        }

        public FontFamily FontFamily
        {
            get { return Document.FontFamily; }
            set
            {
                Document.FontFamily = value;
                NotifyPropertyChanged("FontFamily");
            }
        }

        #endregion

        #region Конструкторы

        public PrintListViewModel()
        {
            database = BdDatabase.Connect(Settings.Default.Server, Settings.Default.Database,
                                            Settings.Default.User, Settings.Default.Password);

            Document = new FlowDocument();

            DateTime today = DateTime.Today;

            if (today.Day > 15)
            {
                today = today.AddMonths(1);
            }

            DateFrom = new DateTime(today.Year, today.Month, 1);
        }

        #endregion

        #region Методы

        private void RefreshDocument()
        {
            PersonList = Person.LoadList(database, DateFrom, DateFrom.AddMonths(1).AddDays(-1), true, 1, "");

            Document.Blocks.Clear();

            Template template = Template.Open(Settings.Default.ListTemplate);
            template.FillDocument(this);
        }

        private void ReturnToMainList()
        {
            OnEndEditing();
        }

        private void ChangeMonth(short value)
        {
            DateFrom = DateFrom.AddMonths(value);
        }

        private void ShowTemplateView()
        {
            if (SetActiveViewModel != null)
                SetActiveViewModel(new TemplateViewModel(true));
        }

        private void PrintDocument()
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                Document.PageHeight = dialog.PrintableAreaHeight;
                Document.PageWidth = dialog.PrintableAreaWidth;
                Document.PagePadding = new Thickness(40);

                dialog.PrintDocument(((IDocumentPaginatorSource)Document).DocumentPaginator, "Печать");
            }
        }

        #endregion

        #region Команды

        #region Close

        private RelayCommand closeCommand;

        public RelayCommand Close
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new RelayCommand(param => ReturnToMainList());

                return closeCommand;
            }
        }

        #endregion

        #region NextMonth

        private RelayCommand nextMonthCommand;

        public RelayCommand NextMonth
        {
            get
            {
                if (nextMonthCommand == null)
                    nextMonthCommand = new RelayCommand(param => ChangeMonth(1));
                return nextMonthCommand;
            }
        }

        #endregion

        #region PreviousMonth

        private RelayCommand previousMonthCommand;

        public RelayCommand PreviousMonth
        {
            get
            {
                if (previousMonthCommand == null)
                    previousMonthCommand = new RelayCommand(param => ChangeMonth(-1));
                return previousMonthCommand;
            }
        }

        #endregion

        #region EditTemplate

        private RelayCommand editTemplateCommand;

        public RelayCommand EditTemplate
        {
            get
            {
                if (editTemplateCommand == null)
                    editTemplateCommand = new RelayCommand(param => ShowTemplateView());

                return editTemplateCommand;
            }
        }

        #endregion

        #region Print

        private RelayCommand printCommand;

        public RelayCommand Print
        {
            get
            {
                if (printCommand == null)
                    printCommand = new RelayCommand(param => PrintDocument());

                return printCommand;
            }
        }

        #endregion

        #endregion

        public void OnImageChanged(object sender, EventArgs e)
        {
            ImageBlockTemplateItem item = sender as ImageBlockTemplateItem;

            if (item != null)
            {
                List<Block> blockList = Document.Blocks.ToList<Block>();
                Document.Blocks.Clear();

                foreach (Block block in blockList)
                {
                    if (block.Tag == item)
                        Document.Blocks.Add(item.CreateParagrath());
                    else
                        Document.Blocks.Add(block);
                }

                NotifyPropertyChanged("Document");
            }
        }
    }
}
