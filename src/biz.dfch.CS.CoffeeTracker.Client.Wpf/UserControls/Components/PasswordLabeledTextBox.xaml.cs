using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for PasswordLabeledTextBox.xaml
    /// </summary>
    public partial class PasswordLabeledTextBox : UserControl, IValidatable
    {
        public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.Register("ToolTipContent", typeof(string), typeof(PasswordLabeledTextBox));

        public string ToolTipContent
        {
            get
            {
                return GetValue(ToolTipContentProperty) as string;
            }
            set
            {
                SetValue(ToolTipContentProperty, value);
            }
        }

        public PasswordLabeledTextBox()
        {
            InitializeComponent();
            var dpd = DependencyPropertyDescriptor.FromProperty(ToolTipContentProperty, typeof(PasswordLabeledTextBox));
            dpd.AddValueChanged(this, (sender, args) =>
            {
                UserControlPasswordBox.ToolTip = this.ToolTipContent;
            });
        }

        public bool Validate()
        {
            var isValid = UserControlPasswordBox.Password.Length >= 6;
            if (!isValid)
            {
                UserControlPasswordBox.BorderBrush = Brushes.Red;
            }
            else
            {
                UserControlPasswordBox.BorderBrush = Brushes.Green;
            }
            return isValid;
        }
    }
}
