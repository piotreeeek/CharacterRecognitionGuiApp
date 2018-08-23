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
    /// Logika interakcji dla klasy DrawTabData.xaml
    /// </summary>
    public partial class DrawTabData : UserControl
    {
        private String _fileNameDraw;

        public DrawTabData()
        {
            InitializeComponent();
            _fileNameDraw = System.IO.Path.GetTempFileName();
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

                RecognitionProvider.Instance.RecogniteLetter(_fileNameDraw, TextBlockDraw);
            }

        }
    }

}
