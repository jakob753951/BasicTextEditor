using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Documents;

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
        }

        private void ToggleBold(object sender, RoutedEventArgs e)
        {
            //If the selection isn't entirely in bold text
            if(Selection.GetPropertyValue(FontWeightProperty).ToString() != FontWeights.Bold.ToString())
                //Make it bold
                Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
            else
                //Make it normal
                Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
        }

        private void ToggleItalic(object sender, RoutedEventArgs e)
        {
            //If the selection isn't entirely in italics
            if(Selection.GetPropertyValue(FontStyleProperty).ToString() != FontStyles.Italic.ToString())
            {
                TextPointer originalSelectionStart = Selection.Start;
                //The position of the last character in the selection
                TextPointer originalSelectionEnd = Selection.End;
                //The position of the character after the last character in the selection
                //TextPointer nextChar = Selection.End.GetNextContextPosition(LogicalDirection.Forward);
                //Expands the selection
                //Selection.Select(Selection.Start, nextChar);
                //Checks if the last character is " "

                TextPointer tempSelectionStart = Selection.Start;
                TextPointer tempSelectionEnd = tempSelectionStart.GetPositionAtOffset(1);
                List<char> italicChar = new List<char>();
                List<char> normalChar = new List<char>();

                foreach(char c in Selection.Text)
                {
                    Selection.Select(tempSelectionStart, tempSelectionEnd);
                    if(Selection.GetPropertyValue(FontStyleProperty).ToString() == FontStyles.Italic.ToString())
                    {
                        italicChar.Add(c);
                    }
                    else
                    {
                        normalChar.Add(c);
                    }
                    tempSelectionStart = tempSelectionStart.GetPositionAtOffset(1);
                    tempSelectionEnd = tempSelectionStart.GetPositionAtOffset(1);
                }

                Selection.Select(originalSelectionStart, originalSelectionEnd);

                for(int i = 1; i <= normalChar.Count; i++)
                {
                    if(normalChar[i] != ' ')
                    {
                        Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                        break;
                    }
                    if(i == normalChar.Count && italicChar.Count == 0)
                    {
                        Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
                    }
                }

                //if(Selection.Text.EndsWith(" "))
                //{
                //    //Make the selection italic
                //    Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                //    //Reverts to the original selection
                //    Selection.Select(Selection.Start, originalSelectionEnd);
                //}
                //else
                //{
                //    //Reverts to the original selection
                //    Selection.Select(Selection.Start, originalSelectionEnd);
                //    //Make the selection italic
                //    Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
                //}
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
            //TODO: Add functionality
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
