using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BasicTextEditorDev
{
    public class BasicTextEditor : RichTextBox
    {
        public BasicTextEditor()
        {

        }

        public RichTextBox Source { get => (GetValue(SourceProperty) as RichTextBox); set => SetValue(SourceProperty, value); }
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(RichTextBox), typeof(BasicTextEditor));

        private void ToggleBold(object sender, EventArgs e)
        {
            Selection.ApplyPropertyValue(SourceProperty, FontWeights.Bold);
        }

        private void ToggleItalic(object sender, EventArgs e)
        {

        }

        private void IncreaseFontSize(object sender, EventArgs e)
        {

        }

        private void DecreaseFontSize(object sender, EventArgs e)
        {

        }

        private void ChangeColour(object sender, EventArgs e)
        {

        }

        private void AddLink(object sender, EventArgs e)
        {

        }
    }
}
