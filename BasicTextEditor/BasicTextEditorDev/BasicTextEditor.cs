using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace BasicTextEditorDev
{
    public class BasicTextEditor : RichTextBox
    {

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(RichTextBox), typeof(BasicTextEditor));

        private ContextMenu cm = new ContextMenu();

        public BasicTextEditor()
        {
            DataContext = this;
            string[] menuItemNames = new string[5] { "Bold", "Italic", "Font size", "Change colour", "Create link" };
            double[] fontSizes = new double[16] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            MenuItem[] menuItems = new MenuItem[5];

            //Initialize, and set header for the menuItems
            for(int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i] = new MenuItem() { Header = menuItemNames[i] };
            }

            MenuItem subItem;
            MenuItem newMenuItems = menuItems[2];
            for(int j = 0; j < fontSizes.Length; j++)
            {
                subItem = new MenuItem() { Header = fontSizes[j] };
                subItem.Click += ChangeFontSize;
                newMenuItems.Items.Add(subItem);
            }


            //TODO: Check if selected text is bold/italicized, and check/uncheck accordingly before showing

            //Assign click events to their respective eventHandlers
            menuItems[0].Click += new RoutedEventHandler(ToggleBold);
            menuItems[1].Click += new RoutedEventHandler(ToggleItalic);
            menuItems[3].Click += new RoutedEventHandler(ChangeColour);
            menuItems[4].Click += new RoutedEventHandler(AddLink);

            for(int i = 0; i < menuItemNames.Length; i++)
            {
                //If i is not 0, and even
                if(i != 0 && i % 2 == 0)
                    //Add a separator
                    cm.Items.Add(new Separator());

                if(i < 2)
                    //make the item checkable
                    menuItems[i].IsCheckable = true;
                //Add the 'i'th item to the ContextMenu
                cm.Items.Add(menuItems[i]);
            }
            ContextMenu = cm;
            ContextMenu.Opened += new RoutedEventHandler(ContextMenuClick);
            AutoWordSelection = false;
        }

        private void ToggleBold(object sender, RoutedEventArgs e)
        {
            //If the selection isn't entirely in bold text
            if(Selection.GetPropertyValue(FontWeightProperty).ToString() != FontWeights.Bold.ToString())
            {
                //The position of the last char in the selection
                TextPointer originalSelectionEnd = Selection.End;
                //The position of the char after the last char in the selection
                TextPointer tempSelectionEnd = Selection.End.GetPositionAtOffset(1);

                //Expands the selection by 1 char to the right
                Selection.Select(Selection.Start, tempSelectionEnd);

                //Checks if the last char is whitespace
                if(Selection.Text.EndsWith(" "))
                {
                    //Make the selection italic
                    Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                    //Revert to the original selection
                    Selection.Select(Selection.Start, originalSelectionEnd);
                }
                else
                {
                    //Revert to the original selection
                    Selection.Select(Selection.Start, originalSelectionEnd);
                    //Make the selection italic
                    Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                }
            }
            else
            {
                //Make it normal
                Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ToggleItalic(object sender, RoutedEventArgs e)
        {
            //If the selection isn't entirely in italics
            if(Selection.GetPropertyValue(FontStyleProperty).ToString() != FontStyles.Italic.ToString())
            {
                //The position of the last character in the selection
                TextPointer originalSelectionEnd = Selection.End;
                //The position of the character after the last character in the selection
                TextPointer tempSelectionEnd = Selection.End.GetPositionAtOffset(1);

                Selection.Select(Selection.Start, tempSelectionEnd);

                if(Selection.Text.EndsWith(" "))
                {
                    //Make the selection italic
                    Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                    //Reverts to the original selection
                    Selection.Select(Selection.Start, originalSelectionEnd);
                }
                else
                {
                    //Reverts to the original selection
                    Selection.Select(Selection.Start, originalSelectionEnd);
                    //Make the selection italic
                    Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                }
            }
            else
            {
                //Make it normal
                Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
            }
        }

        //TODO: Add hover event
        private void ChangeFontSize(object sender, MouseEventArgs e) => ChangeFontSize((MenuItem)sender);
        private void ChangeFontSize(object sender, RoutedEventArgs e) => ChangeFontSize((MenuItem)sender);
        private void ChangeFontSize(MenuItem sender)
        {
            Selection.ApplyPropertyValue(FontSizeProperty, double.Parse(sender.Header.ToString()));
        }

        private void ChangeColour(object sender, RoutedEventArgs e)
        {
            //TODO: Find and use a ColorPicker for this (somehow)
            //Selection.ApplyPropertyValue(ForegroundProperty, $"#{(123456).ToString()}");
        }

        private void AddLink(object sender, RoutedEventArgs e)
        {
            //TODO: Add functionality
        }

        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            MenuItem[] mi = new MenuItem[] { (MenuItem)ContextMenu.Items.GetItemAt(0), (MenuItem)ContextMenu.Items.GetItemAt(1) };
            if(Selection.GetPropertyValue(FontWeightProperty).ToString() == FontWeights.Bold.ToString())
                mi[0].IsChecked = true;
            else
                mi[0].IsChecked = false;

            if(Selection.GetPropertyValue(FontStyleProperty).ToString() == FontStyles.Italic.ToString())
                mi[1].IsChecked = true;
            else
                mi[1].IsChecked = false;
        }

        public RichTextBox Source { get => (GetValue(SourceProperty) as RichTextBox); set => SetValue(SourceProperty, value); }
    }
}
