using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
// ReSharper disable All

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        public EventHandler ButtonClickedEventHandler;

        public SideBar()
        {
            // these lines are needed for the XAML-Designer, if removed NullReferenceException in designer occurs
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            InitializeComponent();
        }

        #region Events

        private void OnButtonMouseUpClick_RaiseEvent(object sender, EventArgs e)
        {
            ChangeToParentBackground(sender);
            if (null == ButtonClickedEventHandler)
            {
                return;
            }

            ButtonClickedEventHandler(sender, e);
        }

        private void ChangeToParentBackgroundEvent(object sender, MouseEventArgs e)
        {
            ChangeToParentBackground(sender);
        }


        private void ChangeBackgroundToAccentColorEvent(object sender, MouseEventArgs e)
        {
            ChangeBackGroundToAccentColor(sender);
        }

        private void SetBackgroundToDarkGray(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as Panel;
            panel.Background = Brushes.DarkGray;
        }

        #endregion

        private void ChangeToParentBackground(object sender)
        {
            var panel = sender as Panel;
            if (panel.IsMouseOver)
            {
                ChangeBackGroundToAccentColor(sender);
            }
            else
            {
                var parent = panel.Parent as Panel;
                panel.Background = parent.Background;
            }
        }

        private void ChangeBackGroundToAccentColor(object sender)
        {
            var panel = sender as Panel;
            panel.Background = Application.Current.Resources["AccentColorBrush"] as Brush;
        }
    }
}
