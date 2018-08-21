using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using Microsoft.Win32;

namespace CharacterRecognitionApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MLApp.MLApp _matlab;
        private String _fileNameDraw;
        private String _fileNameImage;

        public MainWindow()
        {
            InitializeComponent();
            _matlab = new MLApp.MLApp();
            _fileNameDraw = Path.GetTempFileName();

            // Change to the directory where the function is located 
            // TODO: move script to place inside project
            _matlab.Execute(@"cd d:\politechnika\semestr_8\inzynierka\testy");
        }

        private void ButtonClearDraw_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Strokes.Clear();
            TextBlockDraw.Text = null;
        }

        private void ButtonRecogniteDraw_Click(object sender, RoutedEventArgs e)
        {
            if (DrawCanvas.Strokes.Count < 1)
            {
                MessageBoxResult messageEmptyDraw = MessageBox.Show("Draw empty. You must draw something.",
                                          "Empty draw",
                                          MessageBoxButton.OK);
            }
            else
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)DrawCanvas.ActualWidth, (int)DrawCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
                rtb.Render(DrawCanvas);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                FileStream fs = File.Open(_fileNameDraw, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                encoder.Save(fs);
                fs.Close();
                Trace.WriteLine(_fileNameDraw);

                RecogniteLetter(_fileNameDraw, TextBlockDraw);
            }

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
                RecogniteLetter(_fileNameImage, TextBlockImage);
                
                
            }

        }

        private void RecogniteLetter (String fileName, TextBlock textBlock)
        {
            // Define the output 
            object result = null;

            // Call the MATLAB function myfunc
            _matlab.Feval("getLetterForApp", 1, out result, fileName);

            // Display result 
            object[] res = result as object[];
            string response = (String)res[0];
            if (response.Length > 1)
            {
                MessageBoxResult messageBlankImage = MessageBox.Show("Select image blank. Try another.",
                                     "Blank image",
                                     MessageBoxButton.OK);
            }
            else
            {
                textBlock.Text = (String)res[0];
            }


        }
    }
}
