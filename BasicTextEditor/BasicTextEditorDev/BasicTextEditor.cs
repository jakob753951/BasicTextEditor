using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BasicTextEditorDev
{
    public class BasicTextEditor : RichTextBox
    {
        public BasicTextEditor()
        {
            string[] menuItems = new string[8] { "Bold", "Italic", null, "Increase font size", "Decrease font size", null, "Change colour", "Create link" };
            DataContext = this;
            MenuItem[] items = new MenuItem[8];
            for(int i = 0; i < items.Length; i++)
            {
                items[i] = new MenuItem();
            }
            ContextMenu cm = new ContextMenu();
            MenuItem miBold = new MenuItem(), miItalics = new MenuItem(), miFontIncr = new MenuItem(), miFontDecr = new MenuItem(), miColour = new MenuItem(), miLink = new MenuItem();

            for(int i = 0; i < menuItems.Length; i++)
            {
                if(menuItems[i] != null)
                {
                    items[i].Header = menuItems[i];
                    cm.Items.Add(items[i]);
                }
                else
                {
                    cm.Items.Add(new Separator());
                }
            }
            this.ContextMenu = cm;
        }

        public RichTextBox Source
        {
            get => (GetValue(SourceProperty) as RichTextBox); set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(RichTextBox), typeof(BasicTextEditor));

        private void MakeBold(object sender, EventArgs e)
        {
            base.Selection.ApplyPropertyValue(SourceProperty, FontWeights.Bold);
        }

        private void MakeItallic(object sender, EventArgs e)
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
