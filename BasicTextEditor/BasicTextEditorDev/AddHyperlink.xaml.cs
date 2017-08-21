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

        public string Hyperlink { get => "http://www." + txtBoxHyperlink.Text; }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
