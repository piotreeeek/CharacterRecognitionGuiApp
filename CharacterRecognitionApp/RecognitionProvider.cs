using System;
using System.Windows;
using System.Windows.Controls;

namespace CharacterRecognitionApp
{
    class RecognitionProvider
    {
        private static readonly RecognitionProvider instance = new RecognitionProvider();
        private MLApp.MLApp _matlab;

        private RecognitionProvider()
        {
            _matlab = new MLApp.MLApp();

            // Change to the directory where the function is located 
            // TODO: move script to place inside project
            _matlab.Execute(@"cd d:\politechnika\semestr_8\inzynierka\siec\final");
        }
               
        public static RecognitionProvider Instance
        {
            get
            {
                return instance;
            }
        }

        public void RecogniteLetter(String fileName, TextBlock textBlock)
        {
            // Define the output 
            object result = null;

            // Call the MATLAB function myfunc
            _matlab.Feval("getLetterForApp", 2, out result, fileName);

            // Display result 
            object[] res = result as object[];
            string response_letter = (String)res[0];
            int response_error = (int)(double)res[1];
            if (response_error == 1)
            {
                MessageBoxResult messageBlankImage = MessageBox.Show("Wybrany obraz jest pusty. Spróbuj wybrać inny.",
                                     "Pusty obraz",
                                     MessageBoxButton.OK);
            }else if(response_error != 0)
            {
                MessageBoxResult messageBlankImage = MessageBox.Show("Bład rozpoznawania litery.",
                                     "Bład",
                                     MessageBoxButton.OK);
            }
            else
            {
                textBlock.Text = (String)res[0];
            }


        }
    }
}
