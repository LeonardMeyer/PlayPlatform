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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace PlayPlatform
{
    /// <summary>
    /// Logique d'interaction pour PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : MetroWindow
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        //Enable drag with no windows style
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (oldPassTxtBox.Password == Properties.Settings.Default.Password)
            {
                if (newPassTxtBox.Password == confirmPassTxtBox.Password && newPassTxtBox.Password.Length > 0 && confirmPassTxtBox.Password.Length > 0)
                {
                    Properties.Settings.Default.Password = newPassTxtBox.Password;
                    MessageBox.Show("Le mot de passe a bien été changé", "Succès !", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("La confirmation du mot de passe a échoué", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    newPassTxtBox.Clear();
                    confirmPassTxtBox.Clear();                                                             
                    newPassTxtBox.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ancien mot de passe erroné", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                oldPassTxtBox.Clear();
                oldPassTxtBox.Focus();
            }
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            this.Owner.Show();
        }

    }
}
