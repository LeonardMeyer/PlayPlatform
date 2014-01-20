using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

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

        //Enable drag with no windows style
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void passwdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Password == pwdBox.Password)
            {
                var owner = (this.Owner as MainWindow);
                owner.profileTxtBlock.Text = "Administrateur";
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
            PasswordWindow passWindow = new PasswordWindow();
            passWindow.Owner = this;
            this.Hide();
            passWindow.Show();
        }



    }
}
