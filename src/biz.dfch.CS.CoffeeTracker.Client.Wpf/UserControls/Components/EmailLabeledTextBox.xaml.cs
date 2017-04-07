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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for EmailLabeledTextBox.xaml
    /// </summary>
    public partial class EmailLabeledTextBox : IValidationable
    {
        private Brush oldBorderBrush;

        public EmailLabeledTextBox()
        {
            InitializeComponent();
            oldBorderBrush = EmailTextBox.BorderBrush;
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
