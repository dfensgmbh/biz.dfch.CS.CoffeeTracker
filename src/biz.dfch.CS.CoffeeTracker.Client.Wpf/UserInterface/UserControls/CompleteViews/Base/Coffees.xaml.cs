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
        private Coffee currentlySelectedCoffee;

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
            CoffeeCoffeeForm.CoffeeFormDatePicker.Text = coffee.LastDelivery.LocalDateTime.ToLongDateString();

            currentlySelectedCoffee = coffee;
        }

        public void AddButton_OnClick(object sender, EventArgs args)
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
                LastDelivery = DateTimeOffset.Parse(CoffeeCoffeeForm.CoffeeFormDatePicker.Text)
            };

            manager.AddCoffee(coffee);
        }

        public void UpdateButton_OnClick(object sender, EventArgs args)
        {
            if (CoffeeCoffeeForm.IsValid())
            {
                var updateCoffee = currentlySelectedCoffee;

                updateCoffee.Name = CoffeeCoffeeForm.CoffeeFormNameTextBox.Text;
                updateCoffee.Brand = CoffeeCoffeeForm.CoffeeFormBrandTextBox.Text;
                updateCoffee.Price = decimal.Parse(CoffeeCoffeeForm.CoffeeFormPriceTextBox.Text);
                updateCoffee.Stock = int.Parse(CoffeeCoffeeForm.CoffeeFormStockTextBox.Text);
                updateCoffee.LastDelivery = DateTimeOffset.Parse(CoffeeCoffeeForm.CoffeeFormDatePicker.Text);

                manager.UpdateCoffee(updateCoffee);
            }
        }
    }
}