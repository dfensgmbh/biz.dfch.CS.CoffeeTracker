using System;
using System.Collections.Generic;
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

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.Components
{
    /// <summary>
    /// Interaction logic for CoffeeForm.xaml
    /// </summary>
    public partial class CoffeeForm : IValidatable
    {
        public CoffeeForm()
        {
            InitializeComponent();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
