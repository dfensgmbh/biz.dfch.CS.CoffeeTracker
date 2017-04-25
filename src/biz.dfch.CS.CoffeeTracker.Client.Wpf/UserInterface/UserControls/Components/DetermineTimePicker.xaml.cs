﻿using System;
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
    /// Interaction logic for DetermineTimePicker.xaml
    /// </summary>
    public partial class DetermineTimePicker : IValidatable
    {
        public DetermineTimePicker()
        {
            InitializeComponent();
            var nowBeforeThreeMonths = DateTimeOffset.Now.AddMonths(-3);
            var now = DateTimeOffset.Now;
            DetermineTimePickerFromDateTimePicker.DisplayDate = nowBeforeThreeMonths.DateTime;
            DetermineTimePickerUntilDateTimePicker.DisplayDate = now.DateTime;
        }

        public bool IsValid()
        {
            if (!DetermineTimePickerFromDateTimePicker.DisplayDate.HasValue ||
                !DetermineTimePickerUntilDateTimePicker.DisplayDate.HasValue)
            {
                return false;
            }

            var from = DetermineTimePickerFromDateTimePicker.DisplayDate.Value;
            var until = DetermineTimePickerUntilDateTimePicker.DisplayDate.Value;
            return from <= until;
        }
    }
}