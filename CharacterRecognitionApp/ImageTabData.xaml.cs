using Microsoft.Win32;
using System;
using System.Diagnostics;
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

        public ImageTabData()
        {
            InitializeComponent();
        }

        private void ButtonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = @"All Image Files| *.BMP; *.bmp; *.JPG; *.JPEG *.jpg; *.jpeg; *.PNG; *.png; *.GIF; *.gif; *.tif; *.tiff; *.ico; *.ICO|
PNG|*.PNG; *.png|JPEG|*.JPG; *.JPEG *.jpg; *.jpeg|Bitmap|*.BMP; *.bmp|GIF|*.GIF; *.gif|TIF| *.tif; *.tiff|ICO|*.ico; *.ICO";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Open document 
                _fileNameImage = openFileDialog.FileName;
                Trace.WriteLine(_fileNameImage);
                
                ImageContener.Source = this.Image();

            }
        }
        private void ButtonClearImage_Click(object sender, RoutedEventArgs e)
        {
            _fileNameImage = null;
            TextBlockImage.Text = null;
            ImageContener.Source = null;
        }

        private void ButtonRecogniteImage_Click(object sender, RoutedEventArgs e)
        {
            if (ImageContener.Source == null)
            {
                MessageBoxResult messageEmptyImage = MessageBox.Show("Brak obrazu. Musisz wybrać obraz.",
                                          "Brak obrazu",
                                          MessageBoxButton.OK);
            }
            else
            {
                Trace.WriteLine(_fileNameImage);
                RecognitionProvider.Instance.RecogniteLetter(_fileNameImage, TextBlockImage);


            }

        }
        private ImageSource Image()
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileStream fileStream = new FileStream(_fileNameImage, FileMode.Open, FileAccess.Read);
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = fileStream;
            bitmapImage.EndInit();
            fileStream.Dispose();

            return bitmapImage;
        }
    }
}
