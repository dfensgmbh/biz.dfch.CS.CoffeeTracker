using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.CustomEvents;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Managers;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.Components
{
    /// <summary>
    /// Interaction logic for CoffeesSelector.xaml
    /// </summary>
    public partial class CoffeesSelector
    {
        public event EventHandler<CoffeeSelectedEventArgs> CoffeeSelected;
        private ObservableCollection<string> brands;
        private readonly CoffeeSelectorManager manager;

        public CoffeesSelector()
        {
            InitializeComponent();

            // these lines are needed for the XAML-Designer, if removed NullReferenceException in designer occurs
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            DisplayLoading();
            manager = new CoffeeSelectorManager(ClientContext.CoffeeTrackerServiceContext);
            RefreshCoffeeBrandsOnUi();
            HideLoading();
        }

        private void BrandSplitButton_OnSelection(object sender, RoutedEventArgs e)
        {
            DisplayLoading();
            RefreshCoffeesOnUi();
            HideLoading(true);
        }

        private void RefreshCoffeeBrandsOnUi()
        {
            // Load data in background and display loading screen
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                // Give the current thread the permission to manipulate data in UI
                Dispatcher.Invoke(() =>
                {
                    // Get BrandNames and set the corresponding list in the UI
                    var brandList = manager.GetCoffeeBrandNames();
                    brands = new ObservableCollection<string>(brandList);
                    CoffeeSelectorBrandSplitButton.ItemsSource = brands;
                });
            };
            worker.RunWorkerAsync();
        }

        private void RefreshCoffeesOnUi()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var brand = CoffeeSelectorBrandSplitButton.SelectedItem as string;

                    var allCoffeesOfBrand = manager.GetCoffeesOfBrand(brand);
                    // ObservableCollection updates Ui automatically
                    CoffeeSelectorCoffeeSplitButton.ItemsSource = new ObservableCollection<Coffee>(allCoffeesOfBrand);
                });
            };
            worker.RunWorkerAsync();
        }

        private void DisplayLoading()
        {
            CoffeeSelectorCoffeeSplitButton.IsEnabled = false;
            CoffeeSelectorBrandSplitButton.IsEnabled = false;
            CoffeeSelectorProgressRing.IsActive = true;
        }

        private void HideLoading(bool brandSelected = false)
        {
            if (brandSelected)
            {
                CoffeeSelectorCoffeeSplitButton.IsEnabled = true;
            }
            CoffeeSelectorBrandSplitButton.IsEnabled = true;
            CoffeeSelectorProgressRing.IsActive = false;
        }

        private void CoffeeSelectorCoffeeSplitButton_OnSelectionChanged_RaiseEvent(object sender, SelectionChangedEventArgs e)
        {
            RefreshCoffeeBrandsOnUi();
            RefreshCoffeesOnUi();

            // Raise coffee selected event only if it's assigned
            if (CoffeeSelected == null)
            {
                return;
            }
            var coffee = CoffeeSelectorCoffeeSplitButton.SelectedItem as Coffee;
            var eventArgs = new CoffeeSelectedEventArgs(coffee);
            CoffeeSelected(this, eventArgs);
        }
    }
}