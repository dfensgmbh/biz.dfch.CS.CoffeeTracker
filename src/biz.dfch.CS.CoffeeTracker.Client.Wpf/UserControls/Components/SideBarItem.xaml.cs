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

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for SideBarItem.xaml
    /// </summary>
    public partial class SideBarItem : UserControl
    {
        public static readonly DependencyProperty LabelContentProperty = DependencyProperty.Register("LabelContent",
            typeof(string), typeof(SideBarItem));

        public string LabelContent
        {
            get { return GetValue(LabelContentProperty) as string; }
            set { SetValue(LabelContentProperty, value); }
        }

        public SideBarItem()
        {
            InitializeComponent();
            var dpd = DependencyPropertyDescriptor.FromProperty(LabelContentProperty, typeof(SideBarItem));
            dpd.AddValueChanged(this, (sender, args) => { SideBarButtonLabel.Content = this.LabelContent; });
        }

        private void SetBackgroundToLightGray(object sender, MouseEventArgs e)
        {
            var label = sender as Label;
            label.Background = Brushes.LightGray;
        }

        private void SetBackgroundToDarkGray(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            label.Background = Brushes.DarkGray;
        }

        private void ChangeToParentBackground(object sender, MouseEventArgs e)
        {
            var label = sender as Label;
            if (label.IsMouseOver)
            {
                SetBackgroundToLightGray(sender, e);
            }
            else
            {
                var parent = label.Parent as Grid;
                label.Background = parent.Background;
            }
        }
    }
}