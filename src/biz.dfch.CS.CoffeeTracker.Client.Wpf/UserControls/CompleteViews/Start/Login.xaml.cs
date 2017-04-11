using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Resources.LanguageResources;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login(bool justCreatedAccount = false)
        {
            InitializeComponent();
            if (justCreatedAccount)
            {
                ShowCreatedAccountMessage();
            }
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
                LoginMessageTextBlock.Visibility = Visibility.Collapsed;
                DisplayLoading();
                try
                {
                    await client.authenticationHelper.ReceiveAndSetToken(LoginEmail.EmailTextBox.Text, LoginPassword.UserControlPasswordBox.Password);
                    StartWindowSwitcher.StartWindow.Close();
                }
                catch (Exception)
                {
                    if (client.authenticationHelper.bearerToken == string.Empty)
                    {
                        ShowInvalidCredentialError();
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
            var passwordHasValue = string.Empty != LoginPassword.UserControlPasswordBox.Password;

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

        public void ShowInvalidCredentialError()
        {
            LoginMessageTextBlock.Text = Wpf.Resources.LanguageResources.Resources.Login_TextBox_InvalidCredentials;
            LoginMessageTextBlock.Foreground = Brushes.Red;
        }

        public void ShowCreatedAccountMessage()
        {
            LoginMessageTextBlock.Visibility = Visibility.Visible;LoginMessageTextBlock.Text = Wpf.Resources.LanguageResources.Resources.Login_TextBox_CreatedAccount;
            LoginMessageTextBlock.Foreground = Brushes.Green;
            LoginMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}
