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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Resources.LanguageResources;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : MetroWindow
    {
        public BaseWindow()
        {
            InitializeComponent();
            BaseWindowSwitcher.BaseWindow = this;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeToParentBackground(sender, e);
            var panel = sender as Panel;
            if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Home))
            {
                BaseWindowSwitcher.Switch(new Home());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Statistics))
            {
                BaseWindowSwitcher.Switch(new Statistics());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_CoffeeOrder))
            {
                BaseWindowSwitcher.Switch(new CoffeeOrders());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Coffees))
            {
                BaseWindowSwitcher.Switch(new Coffees());
            }
        }

        private void SetBackgroundToAccentColor(object sender, MouseEventArgs e)
        {
            var panel = sender as Panel;
            panel.Background = Application.Current.Resources["AccentColorBrush"] as Brush;
        }

        private void SetBackgroundToDarkGray(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as Panel;
            panel.Background = Brushes.DarkGray;
        }

        private void ChangeToParentBackground(object sender, MouseEventArgs e)
        {
            var panel = sender as Panel;
            if (panel.IsMouseOver)
            {
                SetBackgroundToAccentColor(sender, e);
            }
            else
            {
                var parent = panel.Parent as Panel;
                panel.Background = parent.Background;
            }
        }
    }
}
