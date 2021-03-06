﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CharacterRecognitionApp
{
    /// <summary>
    /// Logika interakcji dla klasy ImageTabData.xaml
    /// </summary>
    public partial class ImageTabData : UserControl
    {
        private String _fileNameImage;
        private FileStream _fileStream;

        public ImageTabData()
        {
            InitializeComponent();
        }

        private void ButtonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = @"All Image Files| *.BMP; *.bmp; *.JPG; *.JPEG *.jpg; *.jpeg; *.PNG; *.png; *.GIF; *.gif; *.tif; *.tiff; *.ico; *.ICO|
PNG|*.PNG; *.png|JPEG|*.JPG; *.JPEG *.jpg; *.jpeg|Bitmap|*.BMP; *.bmp|GIF|*.GIF; *.gif|TIF| *.tif; *.tiff|ICO|*.ico; *.ICO";
 
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                ClearButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                _fileNameImage = openFileDialog.FileName;
                
                ImageContainer.Source = this.Image();

            }
        }
        private void ButtonClearImage_Click(object sender, RoutedEventArgs e)
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
            _fileNameImage = null;
            TextBlockImage.Text = null;
            ImageContainer.Source = null;
        }

        private void ButtonRecogniteImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageContainer.Source == null)
            {
                MessageBoxResult messageEmptyImage = MessageBox.Show("Brak obrazu. Musisz wybrać obraz.",
                                          "Brak obrazu",
                                          MessageBoxButton.OK);
            }
            else
            {
                RecognitionProvider.Instance.RecogniteLetter(_fileNameImage, TextBlockImage);
            }

        }
        private ImageSource Image()
        {
            BitmapImage bitmapImage = new BitmapImage();
            _fileStream = new FileStream(_fileNameImage, FileMode.Open, FileAccess.Read);
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = _fileStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
