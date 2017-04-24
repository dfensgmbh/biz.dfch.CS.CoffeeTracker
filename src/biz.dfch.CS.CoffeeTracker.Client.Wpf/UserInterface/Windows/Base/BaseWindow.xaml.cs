using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Interfaces;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.CompleteViews.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.Windows.Base
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : ILoadable
    {
        private BaseWindowManager baseWindowManager = new BaseWindowManager(ClientContext.CoffeeTrackerServiceContext);

        public BaseWindow()
        {
            InitializeComponent();
            BaseWindowSwitcher.BaseWindow = this;
            BaseWindowSwitcher.Switch(new Home());
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DisplayLoading);
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                var panel = sender as Panel;
                Contract.Assert(null != panel);
                Dispatcher.Invoke(() => { SwitchPages(panel); });
            };
            worker.RunWorkerCompleted += (o, args) => { Dispatcher.Invoke(HideLoading); };
            worker.RunWorkerAsync();
        }

        public void SwitchPages(Panel panel)
        {
            Contract.Requires(null != panel);

            if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Home))
            {
                BaseWindowSwitcher.Switch(new Home());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Statistics))
            {
                BaseWindowSwitcher.Switch(new Statistics());
            }
            else if (panel.ToolTip.ToString()
                .Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_CoffeeOrder))
            {
                BaseWindowSwitcher.Switch(new CoffeeOrders());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Coffees))
            {
                BaseWindowSwitcher.Switch(new Coffees());
            }
            else if (panel.ToolTip.ToString().Equals(Wpf.Resources.LanguageResources.Resources.BaseWindow_SideBar_Logout))
            {
                baseWindowManager.Logout();
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

        public void SwitchToStartWindow()
        {
            if (null == StartWindowSwitcher.StartWindow)
            {
                StartWindowSwitcher.StartWindow = new StartWindow();
            }
            StartWindowSwitcher.StartWindow.Show();
            BaseWindowSwitcher.BaseWindow.Close();
        }
    }
}