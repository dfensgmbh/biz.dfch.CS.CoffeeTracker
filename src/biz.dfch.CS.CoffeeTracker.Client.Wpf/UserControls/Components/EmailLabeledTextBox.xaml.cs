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
            var dpd = DependencyPropertyDescriptor.FromProperty(ToolTipContentProperty, typeof(EmailLabeledTextBox));
            dpd.AddValueChanged(this, (sender, args) =>
            {
                EmailTextBox.ToolTip = this.ToolTipContent;
            });
        }

        public bool Validate()
        {
            var isValid = IsValidEmail(EmailTextBox.Text);
            if (!isValid)
            {
                EmailTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                EmailTextBox.BorderBrush = Brushes.Green;
            }

            return isValid;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
