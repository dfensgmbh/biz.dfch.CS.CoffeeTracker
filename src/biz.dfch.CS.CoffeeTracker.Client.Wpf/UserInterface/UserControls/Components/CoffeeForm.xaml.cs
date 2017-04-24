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

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.Components
{
    /// <summary>
    /// Interaction logic for CoffeeForm.xaml
    /// </summary>
    public partial class CoffeeForm : IValidatable
    {
        public Brush oldBorderBrush;

        public CoffeeForm()
        {
            InitializeComponent();
            oldBorderBrush = CoffeeFormNameTextBox.BorderBrush;
        }

        public bool IsValid()
        {
            var priceText = CoffeeFormPriceTextBox.Text;
            decimal priceDecimal;
            var priceValid = decimal.TryParse(priceText, out priceDecimal);
            CoffeeFormPriceTextBox.BorderBrush = !priceValid ? Brushes.Red : oldBorderBrush;

            var stockText = CoffeeFormStockTextBox.Text;
            int stockInt;
            var stockValid = int.TryParse(stockText, out stockInt);
            CoffeeFormStockTextBox.BorderBrush = !stockValid ? Brushes.Red : oldBorderBrush;

            var lastDeliveryText = CoffeeFormDatePicker.Text;
            DateTimeOffset lastDeliveryDateTimeOffset;
            var dateTimeOffsetValid = DateTimeOffset.TryParse(lastDeliveryText, out lastDeliveryDateTimeOffset);
            CoffeeFormDatePicker.BorderBrush = !dateTimeOffsetValid ? Brushes.Red : oldBorderBrush;

            return priceValid && stockValid && dateTimeOffsetValid;
        }

        public void DisableControls()
        {
            SetControlsIsEnabled(false);
        }

        public void EnableControls()
        {
            SetControlsIsEnabled(true);
        }

        private void SetControlsIsEnabled(bool enabled)
        {
            CoffeeFormNameTextBox.IsEnabled = enabled;
            CoffeeFormBrandTextBox.IsEnabled = enabled;
            CoffeeFormPriceTextBox.IsEnabled = enabled;
            CoffeeFormStockTextBox.IsEnabled = enabled;
            CoffeeFormDatePicker.IsEnabled = enabled;
        }
    }
}
