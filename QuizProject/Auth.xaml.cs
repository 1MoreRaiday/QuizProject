using QuizProject.Models;
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
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;

namespace QuizProject
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        internal MyDBContext context;
        public AuthWindow()
        {
            var bc = new BrushConverter();
            context = new MyDBContext();

            InitializeComponent();

            Register.MouseEnter += (s, e) => Register.Foreground = (Brush)bc.ConvertFrom("#8F96B1");
            Register.MouseLeave += (s, e) => Register.Foreground = (Brush)bc.ConvertFrom("#6B728E");
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            bool stop = false;
            if (LoginBox.Text == "") { LoginBox.BorderBrush = new SolidColorBrush(Colors.Red); stop = true; }
            else { LoginBox.BorderBrush = (Brush)bc.ConvertFrom("#50577A"); }

            if (PasswordBox.Password == "") { PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red); stop = true; }
            else { PasswordBox.BorderBrush = (Brush)bc.ConvertFrom("#50577A"); }

            if(stop) { return; }

            AccountModel account;

            try
            {
                SHA256 sha256 = SHA256.Create();
                string hashString = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(PasswordBox.Password))).Replace("-", "");
                account = context.Accounts.Single(x => x.Name == LoginBox.Text && x.Password == hashString);

                MainWindow mainWindow = new MainWindow(account, context);
                      
                
                mainWindow.Show();
                this.Close();
            }
            catch
            {
                Login.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorMessageLabel.Content = "No such user. Try to register.";
                return;
            }

        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterPopup registerPopup = new RegisterPopup();
            registerPopup.LoginBox.Text = LoginBox.Text;
            registerPopup.context = context;
            registerPopup.ShowDialog();
        }
    }
}
