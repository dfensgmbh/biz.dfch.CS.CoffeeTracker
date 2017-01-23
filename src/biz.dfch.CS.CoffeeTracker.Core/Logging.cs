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
using System.Linq;
using System.Web;
using biz.dfch.CS.Commons;

namespace biz.dfch.CS.CoffeeTracker.Core
{
    public static class Logging
    {
        public enum EventId : int
        {
            Default = 0,

            // DFTODO - define further event ids heres

            Stop = int.MaxValue -2,
            Start = int.MaxValue -1,

            Exception = int.MaxValue
        }

        public static class TraceSourceName
        {
            public const string COFFEE_TRACKER_CORE = "biz.dfch.CS.CoffeTracker.Core";
            public const string WEB_API_CONFIG = "biz.dfch.CS.CoffeeTracker.Core.WebApiConfig";
            public const string COFFEES_CONTROLLER = "biz.dfch.CS.CoffeeTracker.Core.Controllers.CoffeesController";
        }
    }
}