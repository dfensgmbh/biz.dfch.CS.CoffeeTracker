using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for Coffees.xaml
    /// </summary>
    public partial class Coffees : UserControl
    {
        private readonly CoffeesManager manager = new CoffeesManager(ClientContext.CoffeeTrackerServiceContext);

        public Coffees()
        {
            InitializeComponent();
        }

        public void CoffeeSelector_OnCoffeeSelected(object sender, CoffeeSelectedEventArgs args)
        {
            Contract.Requires(null != args);
            Contract.Requires(null != args.SelectedCoffee);

            var coffee = args.SelectedCoffee;
            CoffeeCoffeeForm.CoffeeFormNameTextBox.Text = coffee.Name;
            CoffeeCoffeeForm.CoffeeFormBrandTextBox.Text = coffee.Brand;
            CoffeeCoffeeForm.CoffeeFormPriceTextBox.Text = coffee.Price.ToString(CultureInfo.CurrentCulture);
            CoffeeCoffeeForm.CoffeeFormStockTextBox.Text = coffee.Stock.ToString();
            CoffeeCoffeeForm.CoffeeFormLastDeliveryTextBox.Text = coffee.LastDelivery.ToString(CultureInfo.CurrentCulture);
        }

        public void Button_OnClick(object sender, EventArgs args)
        {
            var isValid = CoffeeCoffeeForm.IsValid();
            if (!isValid)
            {
                return;
            }

            var coffee = new Coffee
            {
                Name = CoffeeCoffeeForm.CoffeeFormNameTextBox.Text,
                Brand = CoffeeCoffeeForm.CoffeeFormBrandTextBox.Text,
                Price = decimal.Parse(CoffeeCoffeeForm.CoffeeFormPriceTextBox.Text),
                Stock = int.Parse(CoffeeCoffeeForm.CoffeeFormStockTextBox.Text),
                LastDelivery = DateTimeOffset.Parse(CoffeeCoffeeForm.CoffeeFormLastDeliveryTextBox.Text)
            };

            var senderButton = sender as Button;
            Contract.Assert(null != senderButton);

            if (senderButton.Equals(CoffeesAddButton))
            {
                manager.AddCoffee(coffee);
            }
            else if (senderButton.Equals(CoffeesUpdateButton))
            {
                manager.UpdateCoffee(coffee);
            }
        }
    }
}
