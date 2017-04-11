using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for EmailLabeledTextBox.xaml
    /// </summary>
    public partial class EmailLabeledTextBox : IValidatable
    {
        private Brush oldBorderBrush;
        public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.Register("ToolTipContent", typeof(string), typeof(EmailLabeledTextBox));

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

        public EmailLabeledTextBox()
        {
            InitializeComponent();
            oldBorderBrush = EmailTextBox.BorderBrush;
            var dpd = DependencyPropertyDescriptor.FromProperty(ToolTipContentProperty, typeof(EmailLabeledTextBox));
            dpd.AddValueChanged(this, (sender, args) =>
            {
                EmailTextBox.ToolTip = this.ToolTipContent;
            });
        }

        public bool Validate()
        {
            var hasValue = string.Empty != EmailTextBox.Text;
            if (!hasValue)
            {
                EmailTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                EmailTextBox.BorderBrush = oldBorderBrush;
            }

            return hasValue;
        }
    }
}
