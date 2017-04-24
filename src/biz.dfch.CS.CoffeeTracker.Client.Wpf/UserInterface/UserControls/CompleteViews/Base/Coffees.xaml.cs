using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Interfaces;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for Coffees.xaml
    /// </summary>
    public partial class Coffees : ILoadable
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
            if (!CoffeeCoffeeForm.IsValid())
            {
                return;
            }

            DisplayLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += (o, eventArgs) => { SendUpdate(); };
            worker.RunWorkerCompleted += (o, eventArgs) => { HideLoading(); };
            worker.RunWorkerAsync();
        }

        public void SendUpdate()
        {
            var updateCoffee = currentlySelectedCoffee;
            Dispatcher.Invoke(() =>
            {
                updateCoffee.Name = CoffeeCoffeeForm.CoffeeFormNameTextBox.Text;
                updateCoffee.Brand = CoffeeCoffeeForm.CoffeeFormBrandTextBox.Text;
                updateCoffee.Price = decimal.Parse(CoffeeCoffeeForm.CoffeeFormPriceTextBox.Text);
                updateCoffee.Stock = int.Parse(CoffeeCoffeeForm.CoffeeFormStockTextBox.Text);
                updateCoffee.LastDelivery = DateTimeOffset.Parse(CoffeeCoffeeForm.CoffeeFormDatePicker.Text);
            });

            Task.Delay(5000).Wait();
            manager.UpdateCoffee(updateCoffee);
        }

        public void DisplayLoading()
        {
            SetEnabled(false);
            CoffeesProgressRing.IsActive = true;
        }

        public void HideLoading()
        {
            SetEnabled(true);
            CoffeesProgressRing.IsActive = false;
        }

        private void SetEnabled(bool enabled)
        {
            CoffeeCoffeeForm.SetEnabled(enabled);
            CoffeeCoffeeSelector.SetEnabled(enabled);
        }
    }
}