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
using MahApps.Metro.Controls;

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
