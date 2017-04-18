using System;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Resources.LanguageResources;

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
                try
                {
                    AddCoffeeOrder();
                    RefreshStock(selectedCoffee);
                }
                catch (Exception x)
                {
                    if (x.GetType() == typeof(DataServiceRequestException))
                    {
                        x = x as DataServiceRequestException;
                        if (x.InnerException.GetType() == typeof(DataServiceClientException))
                        {
                            HttpStatusCode statusCode;
                            var dataServiceClientException = x.InnerException as DataServiceClientException;
                            Enum.TryParse(dataServiceClientException.StatusCode.ToString(), out statusCode);
                            if (HttpStatusCode.BadRequest == statusCode)
                            {
                                BaseWindowSwitcher.DisplayError(Wpf.Resources.LanguageResources.Resources.Home_Label_CoffeeOutOfStock);
                            }
                            else if (HttpStatusCode.InternalServerError == statusCode)
                            {
                                BaseWindowSwitcher.DisplayError(Wpf.Resources.LanguageResources.Resources.Home_Label_CouldntPlaceOrder);
                            }
                            else if (HttpStatusCode.BadGateway == statusCode)
                            {
                                BaseWindowSwitcher.DisplayError(
                                    Wpf.Resources.LanguageResources.Resources.Shared_ServiceNotAvailable);
                            }
                        }
                    }
                }
            };
            worker.RunWorkerCompleted += (o, args) => { HideLoading(); };

            worker.RunWorkerAsync();
        }

        private void AddCoffeeOrder()
        {
            var client = ClientContext.CreateServiceContext();
            var coffeeOrder = new CoffeeOrder()
            {
                Name = ClientContext.CurrentUserName + DateTimeOffset.Now,
                UserId = ClientContext.CurrentUserId,
                CoffeeId = selectedCoffee.Id
            };
            client.AddToCoffeeOrders(coffeeOrder);
            client.SaveChanges();
        }

        private void RefreshPrice(Coffee coffee)
        {
            var refreshedCoffee = RefreshCoffee(coffee);
            HomePriceLabel.Content = refreshedCoffee.Price;
        }

        private void RefreshStock(Coffee coffee)
        {
            var refreshedCoffee = RefreshCoffee(coffee);
            HomeOnStockLabel.Content = refreshedCoffee.Stock;
        }

        private Coffee RefreshCoffee(Coffee coffee)
        {
            var client = ClientContext.CreateServiceContext();
            return client.Coffees.Where(c => c.Brand == coffee.Brand).Where(x => x.Name == coffee.Name).FirstOrDefault();
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