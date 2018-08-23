using Microsoft.Win32;
using System;
using System.Diagnostics;
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

                ImageSource imageSource = new BitmapImage(new Uri(_fileNameImage));
                ImageContener.Source = imageSource;

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
                MessageBoxResult messageEmptyImage = MessageBox.Show("No image selected. You must select image.",
                                          "No image",
                                          MessageBoxButton.OK);
            }
            else
            {
                Trace.WriteLine(_fileNameImage);
                RecognitionProvider.Instance.RecogniteLetter(_fileNameImage, TextBlockImage);


            }

        }
    }
}
