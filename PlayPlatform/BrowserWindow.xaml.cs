using MahApps.Metro.Controls;
using PlayLibrary;
using System;
using System.Windows;
using System.Windows.Forms;


namespace PlayPlatform
{
    /// <summary>
    /// Logique d'interaction pour BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window
    {
        public BrowserWindow(string url)
        {
            InitializeComponent();
            WebView.ScriptErrorsSuppressed = true;
            WebView.AllowNavigation = true;
            WebView.Navigate(new Uri(url));
            WebView.DocumentCompleted += LoadCompleteEventHandler;
        }

        private void LoadCompleteEventHandler(object sender, WebBrowserDocumentCompletedEventArgs navigationEventArgs)
        {
            //Repositionnement de la popup par rapport à la grid
            AirspacePopup.Width = BrowserGrid.ColumnDefinitions[1].ActualWidth;
            AirspacePopup.Height = BrowserGrid.RowDefinitions[1].ActualHeight;
            AirspacePopup.HorizontalOffset = BrowserGrid.ColumnDefinitions[0].ActualWidth;
            AirspacePopup.VerticalOffset = BrowserGrid.RowDefinitions[0].ActualHeight;

            HtmlElementCollection elements = this.WebView.Document.GetElementsByTagName("input");
            //Pour chaque élement input dans le document...
            foreach (HtmlElement input in elements)
            {
                //On lui attache le clavier si c'est un type text, textarea ou password.
                if (input.GetAttribute("type").ToLower() == "text" || input.GetAttribute("type").ToLower() == "password" || input.GetAttribute("type").ToLower() == "textarea" )
                {
                    input.GotFocus += (o, args) => VirtualKeyBoardHelper.AttachTabTip();
                    input.LostFocus += (o, args) => VirtualKeyBoardHelper.RemoveTabTip();
                }
            }
        }

        private void canvasBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
