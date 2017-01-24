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
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Commons.Diagnostics;

namespace biz.dfch.CS.CoffeeTracker.Core
{
    public static class WebApiConfig
    {
        private const string CONVENTION = "OData";

        public static void Register(HttpConfiguration config)
        {
            // Log Event
            Logger.Get(Logging.TraceSourceName.API_ACTIVITIES)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, Message.WebApiConfig_Register__Start, CONVENTION);
            // Web API configuration and services

            // Web API routes
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            builder.EntitySet<Coffee>("Coffees");
            builder.EntitySet<CoffeeOrder>("CoffeeOrders");
            config.Routes.MapODataServiceRoute("odata", "api", builder.GetEdmModel());

            Logger.Get(Logging.TraceSourceName.API_ACTIVITIES)
                .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, Message.WebApiConfig_Register__SUCCEEDED, CONVENTION);
        }
    }
}
