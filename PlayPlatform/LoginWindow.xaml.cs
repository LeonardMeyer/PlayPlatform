using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using PlayLibrary;

namespace PlayPlatform
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Keyboard.Focus(pwdBox);
        }

        private void passwdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Password == pwdBox.Password)
            {
                var owner = (this.Owner as MainWindow);
                owner.profileTxtBlock.Text = "Administrateur";
                owner.disconnectBtn.Visibility = Visibility.Visible;
                owner.ShowCloseButton = true;
                owner.isAdmin = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Mot de passe erroné", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                pwdBox.Clear();
                pwdBox.Focus();
            }
           
        }

        private void SettingsBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Application.Current.Windows.OfType<PasswordWindow>().Any())
            {
                this.Hide();
                VirtualKeyBoardHelper.RemoveTabTip();
                PasswordWindow passWindow = new PasswordWindow();
                passWindow.Owner = this;
                passWindow.Tag = pwdBox;
                passWindow.Show();
            }
        }



    }
}
