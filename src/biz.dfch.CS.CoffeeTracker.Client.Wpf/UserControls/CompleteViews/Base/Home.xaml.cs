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
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private Coffee selectedCoffee;

        public Home()
        {
            InitializeComponent();
        }

        private void CoffeesSelector_OnCoffeeSelected(object sender, CoffeeSelectedEventArgs e)
        {
            HomePriceLabel.Content = e.SelectedCoffee.Price;
            HomeOnStockLabel.Content = e.SelectedCoffee.Stock;
            HomeOrderButton.IsEnabled = true;
            selectedCoffee = e.SelectedCoffee;
        }

        private void HomeOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var worker = new BackgroundWorker();
            DisplayLoading();
            worker.DoWork += (o, args) =>
            {
                AddCoffeeOrder();
            };
            worker.RunWorkerCompleted += (o, args) =>
            {
                HideLoading();
            };

            worker.RunWorkerAsync();
        }

        private void AddCoffeeOrder()
        {
            var client = ClientContext.GetServiceContext();
            var coffeeOrder = new CoffeeOrder()
            {
                Name = ClientContext.CurrentUserName+DateTimeOffset.Now,
                UserId = ClientContext.CurrentUserId,
                CoffeeId = selectedCoffee.Id
            };
            client.AddToCoffeeOrders(coffeeOrder);
            client.SaveChanges();
        }

        private void DisplayLoading()
        {
            HomeAddOrderViewBox.Visibility = Visibility.Collapsed;
            HomeAddOrderGridLoading.Visibility = Visibility.Visible;
        }

        private void HideLoading()
        {
            HomeAddOrderViewBox.Visibility = Visibility.Visible;
            HomeAddOrderGridLoading.Visibility = Visibility.Collapsed;
        }
    }
}
