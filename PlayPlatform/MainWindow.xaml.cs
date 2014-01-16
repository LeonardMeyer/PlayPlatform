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
using MahApps.Metro.Controls;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Interop;
using PlayLibrary;
using PlayPlatform.XML;
using System.IO;

namespace PlayPlatform
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += WindowSourceInitialized;

            AppManager manager = AppManager.GetInstance();
            //Chargement des applis lourdes
            foreach (var app in manager.AppList)
            {
                UserControl appBtn = new UserControl();
                appBtn.Height = app.Value.Thumbnail.Height;
                appBtn.Width = app.Value.Thumbnail.Width;
                appBtn.Background = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(app.Value.Thumbnail.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())); ;
                appBtn.MouseDown += (source, e) =>
                {
                    app.Value.LaunchApp();
                };
                AppPanel.Children.Insert(0, appBtn);

                Manifest man = XMLParser.FromXML("Manifest.xml");
                /*var appli = new PlayPlatform.XML.Application("Test", "1.0", "Description",
                    "path//'eeze", true, TechnologyType.CSharp, CategoryType.Project, "01/03/1958");
                var serv = new Server("1.0", "IIS");
                var db = new Database("Tarace", "54656","Lol", "Basededaube", "R", "JDBC", "mysql://sdsdddsdzooo", "1.2");
                var valid = new PlayPlatform.XML.Validation("Whabhi", "Gasri", "05/12/2013");
                var man = new Manifest("1.0", appli, db, serv, valid); 

                File.WriteAllText("foo.xml", XMLParser.toXML(man));*/
            }
        }

            

        //Disable window drag & drop
        #region 
        private void WindowSourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion

    }
}
