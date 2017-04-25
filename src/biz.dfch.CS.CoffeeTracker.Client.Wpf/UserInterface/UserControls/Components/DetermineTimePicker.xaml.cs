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
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.Components
{
    /// <summary>
    /// Interaction logic for DetermineTimePicker.xaml
    /// </summary>
    public partial class DetermineTimePicker : IValidatable
    {
        private Brush oldBorderBrush;

        public DetermineTimePicker()
        {
            InitializeComponent();
            oldBorderBrush = DetermineTimePickerFromDateTimePicker.BorderBrush;

            var nowBeforeThreeMonths = DateTimeOffset.Now.AddMonths(-3);
            var now = DateTimeOffset.Now;
            DetermineTimePickerFromDateTimePicker.DisplayDate = nowBeforeThreeMonths.DateTime;
            DetermineTimePickerUntilDateTimePicker.DisplayDate = now.DateTime;
        }

        public bool IsValid()
        {
            if (!DetermineTimePickerFromDateTimePicker.DisplayDate.HasValue ||
                !DetermineTimePickerUntilDateTimePicker.DisplayDate.HasValue)
            {
                return false;
            }

            var from = DetermineTimePickerFromDateTimePicker.DisplayDate.Value;
            var until = DetermineTimePickerUntilDateTimePicker.DisplayDate.Value;
            var isValid = from <= until;
            
            if (!isValid)
            {
                TurnInvalid(DetermineTimePickerUntilDateTimePicker);
            }
            else
            {
                TurnValid(DetermineTimePickerUntilDateTimePicker);
            }

            return isValid;
        }

        private void TurnInvalid(Control picker)
        {
            picker.BorderBrush = Brushes.Red;
        }

        private void TurnValid(Control picker)
        {
            picker.BorderBrush = oldBorderBrush;
        }
    }
}
