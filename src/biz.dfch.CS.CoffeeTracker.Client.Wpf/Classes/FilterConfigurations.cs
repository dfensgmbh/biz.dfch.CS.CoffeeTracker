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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes
{
    public class FilterConfigurations
    {
        public ApplicationUser User;
        public DateTimeOffset From;
        public DateTimeOffset Until;
        public Coffee Coffee;

        public FilterConfigurations(ApplicationUser user, DateTimeOffset from, DateTimeOffset until, Coffee coffee)
        {
            Contract.Requires(null != User);
            Contract.Requires(null != from);
            Contract.Requires(null != until);
            Contract.Requires(null != coffee);

            User = user;
            From = from;
            Until = until;
            Coffee = coffee;
        }
    }
}
