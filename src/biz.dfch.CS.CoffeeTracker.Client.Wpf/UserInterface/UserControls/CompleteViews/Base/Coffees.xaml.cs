using System;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for Coffees.xaml
    /// </summary>
    public partial class Coffees : UserControl
    {
        private CoffeesManager manager = new CoffeesManager(ClientContext.CoffeeTrackerServiceContext);

        public Coffees()
        {
            InitializeComponent();
        }

        public void AddButton_OnClick(object sender, EventArgs args)
        {
            var isValid = CoffeeCoffeeForm.IsValid();
            if (isValid)
            {
                var newCoffee = new Coffee
                {
                    Name = CoffeeCoffeeForm.CoffeeFormNameTextBox.Text,
                    Brand = CoffeeCoffeeForm.CoffeeFormBrandTextBox.Text,
                    Price = decimal.Parse(CoffeeCoffeeForm.CoffeeFormPriceTextBox.Text),
                    Stock = Int32.Parse(CoffeeCoffeeForm.CoffeeFormStockTextBox.Text),
                    LastDelivery = DateTimeOffset.Parse(CoffeeCoffeeForm.CoffeeFormLastDeliveryTextBox.Text)
                };
                manager.AddCoffee(newCoffee);
            }
        }
    }
}
