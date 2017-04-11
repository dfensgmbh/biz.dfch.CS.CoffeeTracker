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
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Switcher
{
    public static class StartWindowSwitcher
    {
        public static StartWindow StartWindow;

        public static void Switch(UserControl newPage)
        {
            StartWindow.Navigate(newPage);
            var newPageClassName = newPage.GetType().Name;

            // validate which page is loaded, and set the corresponding title
            if (newPageClassName.Equals(nameof(Login)))
            {
                StartWindow.Title = Resources.LanguageResources.Resources.Login_Title;
            }
            else if (newPageClassName.Equals(nameof(Registration)))
            {
                StartWindow.Title = Resources.LanguageResources.Resources.Registration_Title;
            }
        }
    }
}
