using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.NetworkInformation;
using MahApps.Metro.Controls;
using Microsoft.Win32;
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
using Point = System.Windows.Point;

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
                appBtn.Background = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(app.Value.Thumbnail.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())); 
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
            }

            //Chargement des applications connectés
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

            //On test l'accès au proxy pour savoir s'il est nécessaire d'afficher les applis sur le reseau de l'exia.
            try
            {
                PingReply pingReply = new Ping().Send("10.154.0.1", 5000);
                if (pingReply.Status == IPStatus.Success)
                {
                    foreach (var manifest in ManifestList)
                    {
                        UserControl webAppBtn = new UserControl();
                        webAppBtn.Height = 200;
                        webAppBtn.Width = 200;
                        webAppBtn.Margin = new Thickness(50);

                        if (manifest.Application.Icon.Length > 0)
                        {
                            //Création de l'icone à partir de l'URL
                            Uri pathToIcon = new Uri(manifest.Application.Icon);
                            WebRequest request = WebRequest.Create(pathToIcon);
                            using (var response = (WebResponse)request.GetResponse())
                            {
                                Bitmap webAppIcon = null;
                                using (Stream stream = response.GetResponseStream())
                                {
                                    webAppIcon = new Bitmap(stream);
                                }
                                webAppBtn.Background =
                                    new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(webAppIcon.GetHbitmap(), IntPtr.Zero,
                                        Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
                                response.Close();
                            }
                        }
                        else //S'il n'y a pas d'image
                        {
                            webAppBtn.Background = new SolidColorBrush(Colors.Black);
                        }
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
            }
            catch (Exception)
            {
                //Si le ping échoue, on n'affiche pas les applis connectés
            }

            //Création de la clé pour que le WebControl soit un IE11
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                if (key != null)
                {
                    Object o = key.GetValue("PlayPlatform.exe");
                    if (o == null)
                    {
                        key.SetValue("PlayPlatform.exe", "11000", RegistryValueKind.DWord);
                    }
                    key.Close();
                } 
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
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
