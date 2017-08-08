using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BasicTextEditorDev
{
    public class BasicTextEditor : TextBox
    {
        public TextBox Source
        {
            get => (GetValue(SourceProperty) as TextBox); set => SetValue(SourceProperty, value);
        }
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(TextBox), typeof(BasicTextEditor));

        //TODO: eventhandlers til flg.
        /*
         * Fed skrift
         * Kursiv skrift
         * Skrift størrelse
         * Skrift farve
         * Tilføje et link til markeret tekst
         */

        private void MakeBold(object sender, EventArgs e)
        {

        }
    }
}
