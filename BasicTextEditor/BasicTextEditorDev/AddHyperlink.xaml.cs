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

namespace BasicTextEditorDev
{
    /// <summary>
    /// Interaction logic for AddHyperlink.xaml
    /// </summary>
    public partial class AddHyperlink : Window
    {
        public AddHyperlink()
        {
            InitializeComponent();
        }

        //A string with the url
        public string Hyperlink { get => txtBoxHyperlink.Text; }

        //Closes the window with a 'true' flag
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        // Closes the window
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
