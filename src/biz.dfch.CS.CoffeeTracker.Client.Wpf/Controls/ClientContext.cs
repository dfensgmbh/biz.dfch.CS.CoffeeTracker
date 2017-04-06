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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Controls
{
    public static class ClientContext
    {
        private static readonly ApiClientConfigurationSection _apiClientConfigurationSection
            = (ApiClientConfigurationSection)ConfigurationManager.GetSection("apiClientConfiguration");


        private static CoffeeTrackerServiceContext _coffeeTrackerClient;

        public static CoffeeTrackerServiceContext GetServiceContext()
        {
            if (null == _coffeeTrackerClient)
            {
                _coffeeTrackerClient = new CoffeeTrackerServiceContext(_apiClientConfigurationSection.ApiBaseUri.AbsoluteUri);
            }

            return _coffeeTrackerClient;
        }
    }
}
