using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.VisualBasic;

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

            //Initialize, set headers and set event handlers for font size sub menu
            MenuItem subItem;
            MenuItem newMenuItems = menuItems[2];
            for(int j = 0; j < fontSizes.Length; j++)
            {
                subItem = new MenuItem() { Header = fontSizes[j] };
                subItem.Click += ChangeFontSize;
                newMenuItems.Items.Add(subItem);
            }

            //Assign click events to their respective eventHandlers
            menuItems[0].Click += new RoutedEventHandler(ToggleBold);
            menuItems[1].Click += new RoutedEventHandler(ToggleItalic);
            menuItems[3].Click += new RoutedEventHandler(ChangeColour);
            menuItems[4].Click += new RoutedEventHandler(AddLink);

            //Add menu items to context menu
            for(int i = 0; i < menuItemNames.Length; i++)
            {
                //If i is 2, and even
                if(i == 2)
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

            //Needed for hyperlink functionality
            IsDocumentEnabled = true;
        }

        /// <summary>
        /// Toggle bold on the selected text
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
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

        /// <summary>
        /// Toggle italic on the selected text
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
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

        /// <summary>
        /// Calls event to set font size of the selected text
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
        private void ChangeFontSize(object sender, RoutedEventArgs e) => ChangeFontSize((MenuItem)sender);

        /// <summary>
        /// Sets the font size of the selected text
        /// </summary>
        /// <param name="sender">The sender as of the previous event</param>
        private void ChangeFontSize(MenuItem sender) => Selection.ApplyPropertyValue(FontSizeProperty, double.Parse(sender.Header.ToString()));

        /// <summary>
        /// Changes the colour of the selected text
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
        private void ChangeColour(object sender, RoutedEventArgs e)
        {
            string userInput;
            bool loop = true;
            do
            {
                userInput = Interaction.InputBox("Please enter a hex value:", "Hex colour", "000000", -1, -1);

                if(string.IsNullOrEmpty(userInput))
                    return;

                MatchCollection matches = Regex.Matches(userInput, @"^(([\d]|[a-f]|[A-F]){6}|([\d]|[a-f]|[A-F]){3})$");

                if(matches.Count > 0)
                    loop = false;
                else
                    MessageBox.Show("You need to enter a valid hex-code");
            }
            while(loop);
            Selection.ApplyPropertyValue(ForegroundProperty, $"#{userInput}");
        }

        /// <summary>
        /// Adds a clickable hyperlink to the selected text
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
        private void AddLink(object sender, RoutedEventArgs e)
        {
            //Makes sure text is selected
            if(Selection != null)
            {
                string input;
                while(true)
                {
                    input = Microsoft.VisualBasic.Interaction.InputBox("Input url", "Add Hyperlink", "").ToLower();
                    if(string.IsNullOrEmpty(input))
                        return;

                    if(!(input.StartsWith("http://") || input.StartsWith("https://")))
                    {
                        input = "http://" + input;
                    }

                    if(Uri.TryCreate(input, UriKind.Absolute, out Uri uriInput) && (uriInput.Scheme == Uri.UriSchemeHttp || uriInput.Scheme == Uri.UriSchemeHttps))
                    {
                        //A new hyperlink object, which is inserted at the points specified in arguements
                        Hyperlink link = new Hyperlink(Selection.Start, Selection.End)
                        {
                            //Makes the hyperlink clickable
                            IsEnabled = true,
                            //Adds the user-added hyperlink to the hyperlink object
                            NavigateUri = new Uri(input, UriKind.RelativeOrAbsolute)
                        };

                        //If the url is not absolute, make it absolute
                        if(!link.NavigateUri.IsAbsoluteUri)
                            link.NavigateUri = new Uri("http://" + link.NavigateUri);

                        //Makes the hyperlink open a new window in the default browser
                        link.RequestNavigate += (_sender, args) => Process.Start(args.Uri.ToString());
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid URL");
                    }
                }
            }
        }

        /// <summary>
        /// Checks or unchecks the checkmarks of the first two menu items
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event itself</param>
        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            //Gets the first two menu items
            MenuItem[] mi = new MenuItem[] { (MenuItem)ContextMenu.Items.GetItemAt(0), (MenuItem)ContextMenu.Items.GetItemAt(1) };

            //Marks the menu item as checked, if the selected text is bold
            if(Selection.GetPropertyValue(FontWeightProperty).ToString() == FontWeights.Bold.ToString())
                mi[0].IsChecked = true;
            else
                mi[0].IsChecked = false;

            //Marks the menu item as checked, if the selected text is italic
            if(Selection.GetPropertyValue(FontStyleProperty).ToString() == FontStyles.Italic.ToString())
                mi[1].IsChecked = true;
            else
                mi[1].IsChecked = false;
        }

        public RichTextBox Source { get => (GetValue(SourceProperty) as RichTextBox); set => SetValue(SourceProperty, value); }
    }
}
