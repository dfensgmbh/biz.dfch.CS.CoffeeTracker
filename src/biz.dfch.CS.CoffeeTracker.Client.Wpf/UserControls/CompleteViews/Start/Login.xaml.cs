using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("Implement Login");
        }
    }
}
