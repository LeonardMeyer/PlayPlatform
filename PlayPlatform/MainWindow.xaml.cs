using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
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
        public bool isAdmin { get; set; }
        public List<UserControl> AppButtons { get; set; }
        public List<Manifest> ManifestList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += WindowSourceInitialized;
            this.ShowCloseButton = false;
            AppManager manager = AppManager.GetInstance();
            AppButtons = new List<UserControl>();
            //Chargement des applis lourdes
            foreach (var app in manager.AppList)
            {
                UserControl appBtn = new UserControl();
                appBtn.Height = app.Value.Thumbnail.Height;
                appBtn.Width = app.Value.Thumbnail.Width;
                appBtn.Margin = new Thickness(50);
                appBtn.Cursor = Cursors.Hand;
                appBtn.Background = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(app.Value.Thumbnail.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())); ;
                appBtn.MouseUp += (source, e) =>
                {
                    //Ombre
                    var btn = (UserControl)e.Source;
                    DropShadowEffect eff = new DropShadowEffect();
                    eff.Color = Colors.White;
                    eff.BlurRadius = 20;
                    eff.ShadowDepth = 0;
                    eff.Opacity = 1;
                    btn.Effect = eff;

                    DoubleAnimation blurAnim = new DoubleAnimation(0, 20, TimeSpan.FromMilliseconds(220));
                    blurAnim.AutoReverse = true;
                    blurAnim.Completed += (sender, args) => app.Value.LaunchApp(); 

                    eff.BeginAnimation(DropShadowEffect.BlurRadiusProperty, blurAnim);
                   
                    //Zoom
                    ScaleTransform trans = new ScaleTransform();
                    btn.RenderTransformOrigin = new Point(0.5, 0.5);
                    btn.RenderTransform = trans;
                    DoubleAnimation anim = new DoubleAnimation(1, 1.2, TimeSpan.FromMilliseconds(200));
                    anim.AutoReverse = true;
                    trans.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                    trans.BeginAnimation(ScaleTransform.ScaleYProperty, anim);

                };
                AppButtons.Add(appBtn);
                AppPanel.Children.Insert(0, appBtn);

                //Manifest man = XMLParser.FromXML("Manifest.xml");
               /* var appli = new PlayPlatform.XML.Application("Test", "1.0", "Description",
                    "path//'eeze", true, TechnologyType.CSharp, CategoryType.Project, "01/03/1958", "http://www.lol.fr");
                var serv = new Server("1.0", "IIS");
                var db = new Database("Tarace", "54656","Lol", "Basededaube", "R", "JDBC", "mysql://sdsdddsdzooo", "1.2");
                var valid = new PlayPlatform.XML.Validation("Whabhi", "Gasri", "05/12/2013");
                var man = new Manifest("1.0", appli, db, serv, valid); 

                File.WriteAllText("foo.xml", XMLParser.toXML(man));*/
            }
        }


        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isAdmin)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Owner = this;
                loginWindow.Show();
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
