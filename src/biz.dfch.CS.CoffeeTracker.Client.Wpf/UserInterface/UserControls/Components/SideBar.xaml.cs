using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar
    {
        public event EventHandler ButtonClicked;

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
            ChangeToParentBackground(sender as Panel);
            // ReSharper disable once UseNullPropagation
            if (null == ButtonClicked)
            {
                return;
            }

            ButtonClicked(sender, e);
        }

        private void ChangeToParentBackgroundEvent(object sender, MouseEventArgs e)
        {
            ChangeToParentBackground(sender as Panel);
        }

        private void ChangeBackgroundToAccentColorEvent(object sender, MouseEventArgs e)
        {
            ChangeBackGroundToAccentColor(sender as Panel);
        }

        private void SetBackgroundToDarkGray(object sender, MouseButtonEventArgs e)
        {
            var panel = sender as Panel;
            Contract.Assert(null != panel);
            panel.Background = Brushes.DarkGray;
        }

        #endregion

        private void ChangeToParentBackground(Panel sender)
        {
            Contract.Requires(null != sender);

            if (sender.IsMouseOver)
            {
                ChangeBackGroundToAccentColor(sender);
            }
            else
            {
                var parent = sender.Parent as Panel;
                Contract.Assert(null != parent);

                sender.Background = parent.Background;
            }
        }

        private void ChangeBackGroundToAccentColor(Panel sender)
        {
            Contract.Requires(null != sender);
            sender.Background = Application.Current.Resources["AccentColorBrush"] as Brush;
        }
    }
}
