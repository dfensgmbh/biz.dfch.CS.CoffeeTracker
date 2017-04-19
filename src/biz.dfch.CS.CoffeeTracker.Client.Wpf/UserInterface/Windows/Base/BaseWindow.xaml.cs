using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Interfaces;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.CompleteViews.Base;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.Windows.Base
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : ILoadable
    {
        public BaseWindow()
        {
            InitializeComponent();
            BaseWindowSwitcher.BaseWindow = this;
            BaseWindowSwitcher.Switch(new Home());
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            Contract.Assert(null != panel);
            
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

        public void DisplayLoading()
        {
            BaseWindowContent.Visibility = Visibility.Collapsed;
            BaseWindowProgressRing.IsActive = true;
        }

        public void HideLoading()
        {
            BaseWindowContent.Visibility = Visibility.Visible;
            BaseWindowProgressRing.IsActive = false;
        }
    }
}
