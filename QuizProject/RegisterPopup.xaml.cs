using QuizProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace QuizProject
{
    /// <summary>
    /// Interaction logic for RegisterPopup.xaml
    /// </summary>
    public partial class RegisterPopup : Window
    {
        internal MyDBContext context;

        public RegisterPopup()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            
            var bc = new BrushConverter();

            bool stop = false;

            if (LoginBox.Text == "") { LoginBox.BorderBrush = new SolidColorBrush(Colors.Red); stop = true; }
            else { LoginBox.BorderBrush = (Brush)bc.ConvertFrom("#50577A"); }

            if (PasswordBox.Password == "") { PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red); stop = true; }
            else { PasswordBox.BorderBrush = (Brush)bc.ConvertFrom("#50577A"); }

            if (stop) { return; }

            try
            {
                context.Accounts.Single(x => x.Name == LoginBox.Text);
            }
            catch
            {
                SHA256 sha256 = SHA256.Create();
                string hashString = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(PasswordBox.Password))).Replace("-", "");


                context.Accounts.Add(new AccountModel { Name = LoginBox.Text, Password = hashString});
                context.SaveChangesAsync();
                ErrorMessageLabel.Content = "Succesfully registered. You can login now.";
                ErrorMessageLabel.Foreground = new SolidColorBrush(Colors.DarkGreen);
                return;
            }
            Login.BorderBrush = new SolidColorBrush(Colors.Red);
            ErrorMessageLabel.Content = "Account with this login is already registered";
        }
    }
}
