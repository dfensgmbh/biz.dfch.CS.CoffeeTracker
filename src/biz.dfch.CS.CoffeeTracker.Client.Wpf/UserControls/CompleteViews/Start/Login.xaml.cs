using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher;
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void CreateAccountLabel_OnMouseUp(object sender, RoutedEventArgs e)
        {
            StartWindowSwitcher.Switch(new Registration());
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isValid = ValidateInputs();
            if (isValid)
            {
                var client = ClientContext.GetServiceContext();
                LoginInvalidCredsTextBlock.Visibility = Visibility.Collapsed;
                DisplayLoading();
                try
                {
                    await client.authenticationHelper.ReceiveAndSetToken(LoginEmail.EmailTextBox.Text, LoginPassword.Password);
                    StartWindowSwitcher.StartWindow.Close();
                }
                catch (Exception)
                {
                    if (client.authenticationHelper.bearerToken == string.Empty)
                    {
                        LoginInvalidCredsTextBlock.Visibility = Visibility.Visible;
                    }
                }
                HideLoading();
            }
        }

        private void DisplayLoading()
        {
            // disable all controls
            LoginEmail.IsEnabled = false;
            LoginButton.IsEnabled = false;
            LoginPassword.IsEnabled = false;
            LoginRegistrationStackPanel.Visibility = Visibility.Collapsed;

            // display loading sequence
            ProgressRing.IsActive = true;
        }

        private void HideLoading()
        {
            // enable all controls
            LoginEmail.IsEnabled = true;
            LoginButton.IsEnabled = true;
            LoginPassword.IsEnabled = true;
            LoginRegistrationStackPanel.Visibility = Visibility.Visible;
            
            // hide loading sequence
            ProgressRing.IsActive = false;
        }

        public bool ValidateInputs()
        {
            var passwordHasValue = string.Empty != LoginPassword.Password;

            if (!passwordHasValue)
            {
                LoginPassword.BorderBrush = Brushes.Red;
            }
            else
            {
                LoginPassword.BorderBrush = Brushes.Black;
            }

            return LoginEmail.Validate() && passwordHasValue;
        }
    }
}
