using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Diagnostics;

namespace BasicTextEditorDev
{
    public class BasicTextEditor : RichTextBox
    {

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(RichTextBox), typeof (BasicTextEditor));

        private ContextMenu cm = new ContextMenu();

        public BasicTextEditor()
        {
            DataContext = this;
            string[] menuItemNames = new string[6] { "Bold", "Italic", "Increase font size", "Decrease font size", "Change colour", "Create link" };
            MenuItem[] menuItems = new MenuItem[6];

            //Initialize, and set header for the menuItems
            for(int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i] = new MenuItem() { Header = menuItemNames[i] };
            }

            //TODO: Check if selected text is bold/italicized, and check/uncheck accordingly before showing

            //Assign click events to their respective eventHandlers
            menuItems[0].Click += new RoutedEventHandler(ToggleBold);
            menuItems[1].Click += new RoutedEventHandler(ToggleItalic);
            menuItems[2].Click += new RoutedEventHandler(IncreaseFontSize);
            menuItems[3].Click += new RoutedEventHandler(DecreaseFontSize);
            menuItems[4].Click += new RoutedEventHandler(ChangeColour);
            menuItems[5].Click += new RoutedEventHandler(AddLink);

            for(int i = 0; i < menuItemNames.Length; i++)
            {
                //If i is not 0, and even
                if(i!= 0 && i % 2 == 0)
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

            IsDocumentEnabled = true;
            IsReadOnly = false;
            Document.Blocks.FirstBlock.Margin = new Thickness(0);
        }

        private void ToggleBold(object sender, RoutedEventArgs e)
        {
            //If the selection isn't entirely in bold text
            if(Selection.GetPropertyValue(FontWeightProperty).ToString() != FontWeights.Bold.ToString())
            {
                //The position of the last char in the selection
                TextPointer originalSelectionEnd = Selection.End;
                //The position of the char after the last char in the selection
                TextPointer SelectionEndNext = Selection.End.GetPositionAtOffset(1);

                //Expands the selection by 1 char to the right
                Selection.Select(Selection.Start, SelectionEndNext);

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
                TextPointer SelectionEndNext = Selection.End.GetPositionAtOffset(1);

                Selection.Select(Selection.Start, SelectionEndNext);

                if(Selection.Text.EndsWith(" "))
                {
                    //Make the selection italic
                    Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                    //Reverts to the original selection
                    Selection.Select(Selection.Start, originalSelectionEnd);
                }
                else
                {
                    //Revert to the original selection
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

        //TODO: FIX THESE TWO. I DON'T UNDERSTAND WHAT IS HAPPENING!!!
        //TODO: Add hover event
        private void IncreaseFontSize(object sender, RoutedEventArgs e)
        {
            Selection.ApplyPropertyValue(FontSizeProperty, FontSize++);
        }

        private void DecreaseFontSize(object sender, RoutedEventArgs e)
        {
            Selection.ApplyPropertyValue(FontSizeProperty, FontSize--);
        }

        private void ChangeColour(object sender, RoutedEventArgs e)
        {
           //TODO: Find and use a ColorPicker for this (somehow)
           Selection.ApplyPropertyValue(ForegroundProperty, $"#{(123456).ToString()}");
        }

        private void AddLink(object sender, RoutedEventArgs e)
        {
            //Makes sure text is selected
            if(Selection != null)
            {
                //Initilizes a window for the user to write a hyperlink
                AddHyperlink ahl = new AddHyperlink();
                //If the window is closed by pressing 'OK'
                if(ahl.ShowDialog() == true)
                {
                    //A new hyperlink object, which is inserted at the points specified in arguements
                    Hyperlink link = new Hyperlink(Selection.Start, Selection.End);
                    //Makes the hyperlink clickable
                    link.IsEnabled = true;
                    //Adds the user-added hyperlink to the hyperlink object
                    link.NavigateUri = new Uri(ahl.Hyperlink);
                    //Makes the hyperlink open a new window in the default browser
                    link.RequestNavigate += (_sender, args) => Process.Start(args.Uri.ToString());
                }
            }
        }

        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            MenuItem[] mi = new MenuItem[] { (MenuItem)ContextMenu.Items.GetItemAt(0), (MenuItem)ContextMenu.Items.GetItemAt(1)};
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
