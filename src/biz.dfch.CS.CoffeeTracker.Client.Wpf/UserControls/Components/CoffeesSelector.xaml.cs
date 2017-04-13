using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components
{
    /// <summary>
    /// Interaction logic for CoffeesSelector.xaml
    /// </summary>
    public partial class CoffeesSelector : UserControl
    {
        public event EventHandler CoffeeSelected;
        private readonly ObservableCollection<string> brands = new ObservableCollection<string>();
        private List<Coffee> coffees = new List<Coffee>();

        public CoffeesSelector()
        {
            InitializeComponent();

            // these lines are needed for the XAML-Designer, if removed NullReferenceException in designer occurs
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            RefreshCoffeeBrands();
        }

        private void BrandSplitButton_OnSelection(object sender, RoutedEventArgs e)
        {
            RefreshCoffees();
        }

        private void RefreshCoffeeBrands()
        {
            var client = ClientContext.GetServiceContext();

            // Load data in background and display loading screen
            var worker = new BackgroundWorker();
            DisplayLoading();
            worker.DoWork += (o, args) =>
            {
                // Give the current thread the permission to manipulate data
                this.Dispatcher.Invoke(() =>
                {
                    coffees = client.Coffees.ToList();
                    var allBrands = coffees.Select(coffee => coffee.Brand).ToList().Distinct().ToList();
                    foreach (var brand in allBrands)
                    {
                        brands.Add(brand);
                    }

                    CoffeeSelectorBrandSplitButton.ItemsSource = brands;
                });
            };
            worker.RunWorkerCompleted += (o, args) => { HideLoading(); };
            worker.RunWorkerAsync();
        }

        private void RefreshCoffees()
        {
            DisplayLoading();
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var brand = CoffeeSelectorBrandSplitButton.SelectedItem as string;

                    var allCoffeesOfBrand = coffees.Where(c => c.Brand.Equals(brand)).ToList<Coffee>();
                    var allCoffeesOfBrandObservableCollection = new ObservableCollection<Coffee>();
                    foreach (var coffee in allCoffeesOfBrand)
                    {
                        allCoffeesOfBrandObservableCollection.Add(coffee);
                    }

                    CoffeeSelectorCoffeeSplitButton.ItemsSource = allCoffeesOfBrandObservableCollection;
                });
            };

            worker.RunWorkerCompleted += (o, args) => { HideLoading(); };
            worker.RunWorkerAsync();
        }

        private void DisplayLoading()
        {
            CoffeeSelectorCoffeeSplitButton.IsEnabled = false;
            CoffeeSelectorBrandSplitButton.IsEnabled = false;
            CoffeeSelectorProgressRing.IsActive = true;
        }

        private void HideLoading()
        {
            CoffeeSelectorCoffeeSplitButton.IsEnabled = true;
            CoffeeSelectorBrandSplitButton.IsEnabled = true;
            CoffeeSelectorProgressRing.IsActive = false;
        }

        private void CoffeeSelectorCoffeeSplitButton_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Raise coffee selected event
            if (CoffeeSelected != null)
            {
                var coffee = CoffeeSelectorCoffeeSplitButton.SelectedItem as Coffee;
                var eventArgs = new CoffeeSelectedEventArgs(coffee);
                CoffeeSelected(this, eventArgs);
            }
        }
    }
}