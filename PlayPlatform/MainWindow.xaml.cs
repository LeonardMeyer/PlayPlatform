using MahApps.Metro.Controls;
using PlayLibrary;
using PlayPlatform.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

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
            //Chargement des applis DLL
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

            //Chargement des applications web
            //string manifestsPath = "@\\10.0.0.1\folderName";
            string manifestsPath = @"../../../manifestFolder/";

            if (Directory.Exists(manifestsPath))
            {
                ManifestList = new List<Manifest>();
                string[] pathList = Directory.GetFiles(manifestsPath);
                foreach (string path in pathList)
                {
                    ManifestList.Add(XMLParser.FromXML(path));
                }
            }

             foreach(var manifest in ManifestList)
             {
                 UserControl webAppBtn = new UserControl();
                 webAppBtn.Height = 200;
                 webAppBtn.Width = 200;
                 webAppBtn.Margin = new Thickness(50);
                 webAppBtn.Background = new SolidColorBrush(Colors.YellowGreen);
                 webAppBtn.Cursor = Cursors.Hand;
                 webAppBtn.MouseUp += (source, e) =>
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
                     blurAnim.Completed += (sender, args) => new BrowserWindow(manifest.Application.Url).Show();

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
                 AppPanel.Children.Insert(AppPanel.Children.Count, webAppBtn);  
             }

            
        }

        private void GridConnect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isAdmin)
            {
                //Si il n'y a aucune LoginWindow
                if (!System.Windows.Application.Current.Windows.OfType<LoginWindow>().Any())
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Owner = this;
                    loginWindow.Show();
                }
            }
            else
            {
                PasswordWindow passWindow = new PasswordWindow();
                passWindow.Show();
            }
        }

        private void disconnectBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isAdmin)
            {
                this.profileTxtBlock.Text = "Invité";
                this.disconnectBtn.Visibility = Visibility.Hidden;
                this.exitBtn.Visibility = Visibility.Hidden;
                this.isAdmin = false;
            }
        }

        //Handler qui permet de retirer les effets de bounce WPF de la fenêtre
        private void ManipulationBoundaryFeedbackHandler(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
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

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VirtualKeyBoardHelper.AttachTabTip();
        }

        private void exitBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        


 

    }
}
