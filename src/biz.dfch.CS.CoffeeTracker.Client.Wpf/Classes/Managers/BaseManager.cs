﻿/**
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

using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.CustomEvents;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Switcher;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserInterface.Windows.Base;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Windows.Base;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers
{
    public abstract class BaseManager
    {
        protected CoffeeTrackerClientWpfServiceContext Context;

        protected BaseManager(CoffeeTrackerClientWpfServiceContext ctx)
        {
            Context = ctx;
        }

        public void Logout()
        {
            Contract.Requires(null != BaseWindowSwitcher.BaseWindow);

            ClientContext.DestroySession();
            BaseWindowSwitcher.BaseWindow.SwitchToStartWindow();
        }
    }
}