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
            StartWindowSwitcher.Switch(new Register());
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var client = ClientContext.GetServiceContext();
            try
            {
                await client.authenticationHelper.ReceiveAndSetToken(LoginEmail.Text, LoginPassword.Password);
            }
            catch (Exception)
            {
                if (client.authenticationHelper.bearerToken == string.Empty)
                {
                    Debug.WriteLine("Goddamn");
                }
            }
        }
    }
}
