using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

                DisplayLoading();

                var worker = new BackgroundWorker();
                worker.DoWork += (o, ea) =>
                {
                    var client = ClientContext.GetServiceContext();
                    client.AddToUsers(newAppUser);
                    client.SaveChanges();
                };

                worker.RunWorkerCompleted += async (o, args) =>
                {
                    try
                    {
                        var client = ClientContext.GetServiceContext();
                        await client.authenticationHelper.ReceiveAndSetToken(email, password);
                        if (!string.IsNullOrEmpty(client.authenticationHelper.bearerToken))
                        {
                            client.authenticationHelper.bearerToken = "";
                            StartWindowSwitcher.Switch(new Login(true));
                        }
                        else
                        {
                            // If this error is visible, there's an error with the client side code
                            RegistrationFailedTextBlock.Text =
                                Wpf.Resources.LanguageResources.Resources.Registration_RegistrationFailed;
                            RegistrationFailedTextBlock.Visibility = Visibility.Visible;
                            HideLoading();
                        }
                    }
                    catch (Exception)
                    {
                        // If this error is visible, there's an error with the client side code
                        RegistrationFailedTextBlock.Text =
                            Wpf.Resources.LanguageResources.Resources.Registration_ServiceNotAvailable;
                        RegistrationFailedTextBlock.Visibility = Visibility.Visible;
                        HideLoading();
                    }
                };

                worker.RunWorkerAsync();
            }
        }

        private bool IsValidForm()
        {
            var stackPanelChildren = RegistrationFormStackPanel.Children;

            var isValid = true;
            foreach (var child in stackPanelChildren)
            {
                var validatable = child as IValidatable;
                if (null != validatable)
                {
                    if (!validatable.Validate())
                    {
                        isValid = false;
                    }
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
                RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.BorderBrush = Brushes.Red;
            }
            else
            {
                RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.BorderBrush = Brushes.Green;
            }

            return isValid;
        }

        private void DisableControls()
        {
            RegistrationEmailTextBox.EmailTextBox.IsEnabled = false;
            RegistrationPasswordPasswordBox.UserControlPasswordBox.IsEnabled = false;
            RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.IsEnabled = false;
            RegistrationButton.IsEnabled = false;
            RegistrationLinkStackPanel.Visibility = Visibility.Collapsed;
        }

        private void EnableControls()
        {
            RegistrationEmailTextBox.EmailTextBox.IsEnabled = true;
            RegistrationPasswordPasswordBox.UserControlPasswordBox.IsEnabled = true;
            RegistrationReEnterPasswordPasswordBox.UserControlPasswordBox.IsEnabled = true;
            RegistrationButton.IsEnabled = true;
            RegistrationLinkStackPanel.Visibility = Visibility.Visible;
        }

        private void DisplayLoading()
        {
            DisableControls();
            RegistrationProgressRing.IsActive = true;
        }

        private void HideLoading()
        {
            RegistrationProgressRing.IsActive = false;
            EnableControls();
        }
    }
}