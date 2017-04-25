using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Interfaces;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base
{
    /// <summary>
    /// Interaction logic for CoffeeOrders.xaml
    /// </summary>
    public partial class CoffeeOrders : ILoadable
    {
        private CoffeeOrdersManager manager = new CoffeeOrdersManager(ClientContext.CoffeeTrackerServiceContext);

        public CoffeeOrders()
        {
            InitializeComponent();
            
        }

        public void RefreshCoffeeOrdersOnUi()
        {
            // Get all needed values from filter and create instance of filter configurations
            var fromDateTime = CoffeeOrdersFilter.FilterDetermineTimePicker.DetermineTimePickerFromDateTimePicker.SelectedDate;
            Contract.Assert(fromDateTime.HasValue);
            var fromDateTimeOffset = new DateTimeOffset(fromDateTime.Value);

            var untilDateTime =
                CoffeeOrdersFilter.FilterDetermineTimePicker.DetermineTimePickerUntilDateTimePicker.SelectedDate;
            Contract.Assert(untilDateTime.HasValue);
            var untilDateTimeOffset = new DateTimeOffset(untilDateTime.Value);

            var coffee = CoffeeOrdersFilter.FilterCoffeesSelector.SelectedCoffee;

            var user = CoffeeOrdersFilter.FilterUserSplitButton.SelectedItem as ApplicationUser;

            var filterConfiguration = new FilterConfigurations(user, fromDateTimeOffset, untilDateTimeOffset, coffee);

            // Use backgroundworker
            DisplayLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += RefreshCoffeesInBackroundWorker;
            worker.RunWorkerCompleted += SetCoffeeOrdersInUiOnWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void SetCoffeeOrdersInUiOnWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            throw new NotImplementedException();
        }

        private void RefreshCoffeesInBackroundWorker(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            throw new NotImplementedException();
        }

        public void DisplayLoading()
        {
            throw new NotImplementedException();
        }

        public void HideLoading()
        {
            throw new NotImplementedException();
        }
    }
}
