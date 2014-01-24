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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awesomium.Core;
using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Interop;

namespace PlayPlatform
{
    /// <summary>
    /// Logique d'interaction pour BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : MetroWindow
    {
        public BrowserWindow(string url)
        {
            InitializeComponent();
            webView.Source = new Uri(url);
        }

        //Handler qui permet de retirer les effets de bounce WPF de la fenêtre
        private void ManipulationBoundaryFeedbackHandler(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
