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
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using biz.dfch.CS.CoffeeTracker.Core.Controllers;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Commons.Diagnostics;
using biz.dfch.CS.Web.Utilities.Http;
using biz.dfch.CS.Web.Utilities.OData;
using Microsoft.AspNet.Identity.EntityFramework;
using static biz.dfch.CS.CoffeeTracker.Core.Logging.Logging;

namespace biz.dfch.CS.CoffeeTracker.Core
{
    public static class WebApiConfig
    {
        private const string CONVENTION = "OData";

        public static void Register(HttpConfiguration config)
        {
            // Log Event
            Logger.Get(TraceSourceName.API_ACTIVITIES)
                .TraceEvent(TraceEventType.Start, (int)EventId.Start, Message.WebApiConfig_Register__Start, CONVENTION);
            // Web API configuration and services

            // Web API routes
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ApplicationUser>("Users");
            builder.EntitySet<IdentityUser>("IdentityUsers");
            builder.EntitySet<Coffee>("Coffees");
            builder.EntitySet<CoffeeOrder>("CoffeeOrders");

            // Custom Actions
            var getCoffeeConsumptionByUserActionConfiguration = builder.Entity<CoffeeOrder>().Collection.Action(nameof(CoffeeOrdersController.GetCoffeeConsumptionByCurrentUser))
                .Returns<int>();
            getCoffeeConsumptionByUserActionConfiguration.Parameter<DateTimeOffset>("From");
            getCoffeeConsumptionByUserActionConfiguration.Parameter<DateTimeOffset>("Until");

            var getCoffeeConsumptionActionConfiguration = builder.Entity<CoffeeOrder>().Collection.Action(nameof(CoffeeOrdersController.GetCoffeeConsumption))
                .Returns<int>();
            getCoffeeConsumptionActionConfiguration.Parameter<DateTimeOffset>("From");
            getCoffeeConsumptionActionConfiguration.Parameter<DateTimeOffset>("Until");

            var getCoffeeConsumptionByCoffeeActionConfiguration = builder.Entity<CoffeeOrder>().Collection.Action(nameof(CoffeeOrdersController.GetCoffeeConsumptionByCoffee))
                .Returns<int>();
            getCoffeeConsumptionByCoffeeActionConfiguration.Parameter<string>("Name");
            getCoffeeConsumptionByCoffeeActionConfiguration.Parameter<string>("Brand");
            getCoffeeConsumptionByCoffeeActionConfiguration.Parameter<DateTimeOffset>("From");
            getCoffeeConsumptionByCoffeeActionConfiguration.Parameter<DateTimeOffset>("Until");

            var getMostOrderedCoffeeActionCofiguration = builder.Entity<CoffeeOrder>()
                .Collection.Action(nameof(CoffeeOrdersController.GetMostOrderedCoffee))
                .ReturnsFromEntitySet<Coffee>("Coffees");
            getMostOrderedCoffeeActionCofiguration.Parameter<DateTimeOffset>("From");
            getMostOrderedCoffeeActionCofiguration.Parameter<DateTimeOffset>("Until");

            var getMostOrderedCoffeeByUserActionCofiguration = builder.Entity<CoffeeOrder>()
                .Collection.Action(nameof(CoffeeOrdersController.GetMostOrderedCoffeeByUser))
                .ReturnsFromEntitySet<Coffee>("Coffees");
            getMostOrderedCoffeeByUserActionCofiguration.Parameter<DateTimeOffset>("From");
            getMostOrderedCoffeeByUserActionCofiguration.Parameter<DateTimeOffset>("Until");
            getMostOrderedCoffeeByUserActionCofiguration.Parameter<string>("Email");


            config.Routes.MapODataServiceRoute
            (
                routeName: "odata", 
                routePrefix: "api", 
                model: builder.GetEdmModel()
            );

            // Exception filters
            // filter are processed from bottom (first called) to top (last called)
            config.Filters.Add(new CatchallExceptionFilterAttribute(false));
            config.Filters.Add(new ODataExceptionFilterAttribute());
            config.Filters.Add(new HttpStatusExceptionFilterAttribute());
            config.Filters.Add(new ContractRequiresExceptionFilterAttribute());

            Logger.Get(TraceSourceName.API_ACTIVITIES)
                .TraceEvent(TraceEventType.Stop, (int)EventId.Stop, Message.WebApiConfig_Register__SUCCEEDED, CONVENTION);
        }
    }
}
