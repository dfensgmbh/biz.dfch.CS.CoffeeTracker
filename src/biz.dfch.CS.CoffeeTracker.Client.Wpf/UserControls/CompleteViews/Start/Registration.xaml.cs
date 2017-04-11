using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            StartWindowSwitcher.Switch(new Login());
        }

        private void RegistrationButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsValidForm())
            {
                var email = RegistrationEmailTextBox.EmailTextBox.Text;
                var password = RegistrationPasswordPasswordBox.UserControlPasswordBox.Password;
                var newAppUser = new ApplicationUser()
                {
                    Name = email,
                    Password = password,
                    AspNetUserId = ""
                };

                var client = ClientContext.GetServiceContext();
                try
                {
                    client.AddToUsers(newAppUser);
                    client.SaveChanges();
                    StartWindowSwitcher.Switch(new Login(true));
                }
                catch (Exception)
                {
                    // If this error is visible, there's an error with the client side code
                    RegistrationFailedTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private bool IsValidForm()
        {
            var validatableObjects = RegistrationGrid.Children.OfType<IValidatable>();

            var isValid = true;
            foreach (var validatable in validatableObjects)
            {
                if (!validatable.Validate())
                {
                    isValid = false;
                }
            }

            if (!EqualPasswords())
            {
                isValid = false;
            }

            return isValid;
        }

        private bool EqualPasswords()
        {
            
            var password = RegistrationPasswordPasswordBox.UserControlPasswordBox.Password;
            var reEnteredPassword = RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.Password;
            var isValid = password.Equals(reEnteredPassword);
            if (!isValid)
            {
                isValid = false;
                RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.BorderBrush = Brushes.Red;
            }
            else
            {
                // Set BorderBrush to its original brush
                RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.BorderBrush = RegistrationReEnterPasswordPasswordBox.oldBorderBrush;
            }

            return isValid;
        }
    }
}