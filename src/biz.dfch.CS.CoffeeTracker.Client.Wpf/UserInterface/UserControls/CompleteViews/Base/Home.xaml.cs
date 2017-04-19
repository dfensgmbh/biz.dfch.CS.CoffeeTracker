using System;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Globalization;
using System.Net;
using System.Windows;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Interfaces;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : ILoadable
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
            Dispatcher.Invoke(() => { HomePriceLabel.Content = price.ToString(CultureInfo.InvariantCulture); });
        }

        private void RefreshStockOnUi(long stock)
        {
            Dispatcher.Invoke(() => { HomeOnStockLabel.Content = stock.ToString(); });
        }

        public void DisplayLoading()
        {
            HomeAddOrderViewBox.Visibility = Visibility.Collapsed;
            HomeAddOrderGridLoading.Visibility = Visibility.Visible;
        }

        public void HideLoading()
        {
            HomeAddOrderViewBox.Visibility = Visibility.Visible;
            HomeAddOrderGridLoading.Visibility = Visibility.Collapsed;
        }

        private void BackgroundWorkerDoWork(object o, EventArgs args)
        {
            try
            {
                var homeManager = new HomeManager(ClientContext.CoffeeTrackerServiceContext);
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