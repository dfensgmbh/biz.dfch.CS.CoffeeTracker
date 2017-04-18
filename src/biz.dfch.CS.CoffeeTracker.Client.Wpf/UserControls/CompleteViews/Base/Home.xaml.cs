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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;
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
            worker.DoWork += BackgroundWorkerDoWork;
            worker.RunWorkerCompleted += (o, args) => { HideLoading(); };
            worker.RunWorkerAsync();
        }

        private void RefreshPriceOnUi(decimal price)
        {
            HomePriceLabel.Content = price;
        }

        private void RefreshStockOnUi(decimal stock)
        {
            HomeOnStockLabel.Content = stock;
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

        private void BackgroundWorkerDoWork(object o, EventArgs args)
        {
            try
            {
                var homeManager = new HomeManager(ClientContext.CreateServiceContext());
                homeManager.AddCoffeeOrder(selectedCoffee.Id);
                selectedCoffee = homeManager.RefreshCoffee(selectedCoffee);
                RefreshPriceOnUi(selectedCoffee.Price);
                RefreshStockOnUi(selectedCoffee.Stock);
            }
            catch (Exception x)
            {
                if (x is DataServiceRequestException)
                {
                    x = x as DataServiceRequestException;
                    if (x.InnerException is DataServiceClientException)
                    {
                        HttpStatusCode statusCode;
                        var dataServiceClientException = x.InnerException as DataServiceClientException;
                        Enum.TryParse(dataServiceClientException.StatusCode.ToString(), out statusCode);
                        if (statusCode == HttpStatusCode.BadRequest)
                            BaseWindowSwitcher.DisplayError(
                                Wpf.Resources.LanguageResources.Resources.Home_Label_CoffeeOutOfStock);
                        else if (statusCode == HttpStatusCode.InternalServerError)
                        {
                            BaseWindowSwitcher.DisplayError(
                                Wpf.Resources.LanguageResources.Resources.Home_Label_CouldntPlaceOrder);
                        }
                        else if (statusCode == HttpStatusCode.BadGateway)
                        {
                            BaseWindowSwitcher.DisplayError(
                                Wpf.Resources.LanguageResources.Resources.Shared_ServiceNotAvailable);
                        }
                    }
                }
            }
        }
    }
}