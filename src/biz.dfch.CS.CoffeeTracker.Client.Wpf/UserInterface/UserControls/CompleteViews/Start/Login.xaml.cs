using System.Windows;
using System.Windows.Media;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
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
            if (!isValid)
            {
                return;
            }

            LoginMessageTextBlock.Visibility = Visibility.Collapsed;
            DisplayLoading();
            var loginManager = new LoginManager(ClientContext.CoffeeTrackerServiceContext);
            var succeeded = await loginManager.Login(LoginEmail.EmailTextBox.Text,
                LoginPassword.UserControlPasswordBox.Password);
            if (succeeded)
            {
                StartWindowSwitcher.OpenBaseWindow();
            }
            else
            {
                DisplayInvalidCredentialError();
            }

            HideLoading();
        }

        private void DisplayLoading()
        {
            // disable all controls
            SetControlsVisibility(Visibility.Collapsed);

            // display loading sequence
            ProgressRing.IsActive = true;
        }

        private void HideLoading()
        {
            // enable all controls
            SetControlsVisibility(Visibility.Visible);

            // hide loading sequence
            ProgressRing.IsActive = false;
        }

        private void SetControlsVisibility(Visibility visibility)
        {
            LoginEmail.Visibility = visibility;
            LoginButton.Visibility = visibility;
            LoginPassword.Visibility = visibility;
            LoginRegistrationStackPanel.Visibility = visibility;
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

            return LoginEmail.IsValid() && passwordHasValue;
        }

        public void DisplayInvalidCredentialError()
        {
            LoginMessageTextBlock.Text = Wpf.Resources.LanguageResources.Resources.Login_TextBox_InvalidCredentials;
            LoginMessageTextBlock.Foreground = Brushes.Red;
            LoginMessageTextBlock.Visibility = Visibility.Visible;
        }

        public void ShowCreatedAccountMessage()
        {
            LoginMessageTextBlock.Text = Wpf.Resources.LanguageResources.Resources.Login_TextBox_CreatedAccount;
            LoginMessageTextBlock.Foreground = Brushes.Green;
            LoginMessageTextBlock.Visibility = Visibility.Visible;
        }
    }
}