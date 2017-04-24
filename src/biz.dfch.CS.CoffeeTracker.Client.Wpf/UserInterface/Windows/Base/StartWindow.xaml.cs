using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start;
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : MetroWindow
    {
        public StartWindow()
        {
            InitializeComponent();
            StartWindowSwitcher.StartWindow = this;
            StartWindowSwitcher.Switch(new Login());
        }
    }
}