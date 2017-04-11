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
