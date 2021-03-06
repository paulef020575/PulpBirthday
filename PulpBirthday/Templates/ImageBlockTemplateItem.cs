﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;

namespace PulpBirthday
{
    [Serializable]
    public class ImageBlockTemplateItem : TemplateItem, INotifyPropertyChanged
    {
        #region Свойства

        [XmlIgnore]
        public BitmapImage Image { get; set; }

        public byte[] ImageBytes
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    JpegBitmapEncoder coder = new JpegBitmapEncoder();
                    coder.Frames.Add(BitmapFrame.Create(Image));
                    coder.Save(ms);
                    return ms.ToArray();
                }   
            }

            set
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(value))
                {
                    //JpegBitmapDecoder decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    image.Freeze();

                    Image = image;
                }
            }
        }

        #endregion

        #region Конструкторы

        public ImageBlockTemplateItem() : this(0) { }

        public ImageBlockTemplateItem(int id)
            : base(id, TemplateItemType.ImageBlock)
        {
            Image = new BitmapImage(new Uri("d://KarjalaPulp.png"));
        }

        #endregion

        #region Методы

        private void OpenImageFromFile(string fileName)
        {
            Image = new BitmapImage(new Uri(fileName));
            NotifyPropertyChanged("Image");

            if (onDocumentChanged != null)
                onDocumentChanged(this, EventArgs.Empty);
        }

        private void OpenImage()
        {
            OpenFileDialog openFileDialog = CreateDialog();
            if (openFileDialog.ShowDialog() == true)
                OpenImageFromFile(openFileDialog.FileName);
        }

        private OpenFileDialog CreateDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Точечные рисунки|*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Добавить изображение из файла";

            return openFileDialog;
        }

        public override void AddToDocument(object fillObject)
        {
            PrintListViewModel printList = fillObject as PrintListViewModel;

            if (printList != null)
            {
                if (onDocumentChanged == null)
                    onDocumentChanged += printList.OnImageChanged;
                AddImageToDocument(printList.Document);
                return;
            }

            throw new ArgumentException("Неподдерживаемый вид шаблона");
        }

        private void AddImageToDocument(FlowDocument document)
        {
            document.Blocks.Add(CreateParagrath());            
        }

        public Paragraph CreateParagrath()
        {
            Paragraph paragrath = new Paragraph();
            System.Windows.Controls.Image imageCopy = new System.Windows.Controls.Image();
            imageCopy.Source = Image;
            paragrath.Inlines.Add(imageCopy);

            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "Изменить изображение";
            menuItem.Command = LoadFile;

            menu.Items.Add(menuItem);
            imageCopy.ContextMenu = menu;

            paragrath.Tag = this;

            return paragrath;
        }

        private EventHandler onDocumentChanged;

        #endregion

        #region Команды

        #region MyRegion

        private RelayCommand loadFileCommand;

        public RelayCommand LoadFile
        {
            get
            {
                if (loadFileCommand == null)
                    loadFileCommand = new RelayCommand(param => OpenImage());

                return loadFileCommand;
            }
        }

        #endregion

        #endregion

        #region реализация INotifyPropertyChanged

        private PropertyChangedEventHandler onPropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { onPropertyChanged += value; }
            remove { onPropertyChanged -= value; }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
