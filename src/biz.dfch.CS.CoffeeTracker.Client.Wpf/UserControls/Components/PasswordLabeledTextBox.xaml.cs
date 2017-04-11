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
    /// Interaction logic for PasswordLabeledTextBox.xaml
    /// </summary>
    public partial class PasswordLabeledTextBox : UserControl, IValidatable
    {
        private Brush oldBorderBrush;

        public PasswordLabeledTextBox()
        {
            InitializeComponent();
            oldBorderBrush = UserControlPasswordBox.BorderBrush;
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
                UserControlPasswordBox.BorderBrush = oldBorderBrush;
            }
            return isValid;
        }
    }
}
