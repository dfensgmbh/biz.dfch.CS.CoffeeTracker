/**
 * Copyright 2017 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Windows.Controls;
using System.Windows.Media;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.UserControls.CompleteViews.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base;
using MahApps.Metro.Controls;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher
{
    public static class BaseWindowSwitcher
    {
        public static BaseWindow BaseWindow;

        public static void Switch(UserControl newPage)
        {
            BaseWindow.BaseWindowContent.Content = newPage;
            var newPageClassName = newPage.GetType().Name;

            // validate which page is loaded, and set the corresponding title
            if (newPageClassName.Equals(nameof(Home)))
            {
                SwitchTitle(Resources.LanguageResources.Resources.Home_Title);
            }
            else if (newPageClassName.Equals(nameof(Statistics)))
            {
                SwitchTitle(Resources.LanguageResources.Resources.Statistics_Title);
            }
            else if (newPageClassName.Equals(nameof(CoffeeOrders)))
            {
                SwitchTitle(Resources.LanguageResources.Resources.CoffeeOrders_Title);
            }
            else if (newPageClassName.Equals(nameof(Coffees)))
            {
                SwitchTitle(Resources.LanguageResources.Resources.Coffees_Title);
            }
        }

        private static void SwitchTitle(string title)
        {
            BaseWindow.Title = title;
            BaseWindow.BaseWindowTitleLabel.Content = title;
        }

        public static void DisplayError(string message)
        {
            BaseWindow.BaseWindowUserMessageLabel.Invoke(() =>
            {
                BaseWindow.BaseWindowUserMessageLabel.Foreground = Brushes.Red;
                BaseWindow.BaseWindowUserMessageLabel.Content = message;
            });
        }

        public static void DisplaySuccess(string message)
        {
            BaseWindow.BaseWindowUserMessageLabel.Invoke(() =>
            {
                BaseWindow.BaseWindowUserMessageLabel.Foreground = Brushes.Green;
                BaseWindow.BaseWindowUserMessageLabel.Content = message;
            });
        }
    }
}