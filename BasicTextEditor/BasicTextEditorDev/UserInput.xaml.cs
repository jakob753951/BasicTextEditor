using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace BasicTextEditorDev
{
    /// <summary>
    /// Interaction logic for AddHyperlink.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        public enum WindowUse { Hex, Link }
        WindowUse use;
        public UserInput(WindowUse wu)
        {
            InitializeComponent();
            if(wu == WindowUse.Hex)
            {
                use = WindowUse.Hex;
                Title = "Hex colour";
                LabelUI.Content = "Please enter a hex value:";
            }
            else
                use = WindowUse.Link;
        }
        //A string with the url
        public string TextResult => TextBoxUserInput.Text;

        //Closes the window with a 'true' flag
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if(use == WindowUse.Hex)
            {
                MatchCollection matches = Regex.Matches(TextResult, @"^(([\d]|[a-f]|[A-F]){6}|([\d]|[a-f]|[A-F]){3})$");
                if(matches.Count > 0)
                    DialogResult = true;
                else
                    MessageBox.Show("You need to enter a valid hex-code");
            }
            else
                DialogResult = true;
        }

        // Closes the window
        private void ButtonCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
