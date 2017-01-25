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

using System.Collections.Generic;
using biz.dfch.CS.CoffeeTracker.Core.Model;

namespace biz.dfch.CS.CoffeeTracker.Core.Logging
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
            public const string API_ACTIVITIES = "biz.dfch.CS.CoffeeTracker.Core";
            public const string COFFEETRACKER_CONTROLLERS = "biz.dfch.CS.CoffeeTracker.Core.Controllers";
        }

        public static string FormatEntity(BaseEntity entity)
        {
            var message = "";

            foreach (var property in GetPropertyNames(entity))
            {
                var value = entity.GetType().GetProperty(property).GetValue(entity, null);
                message = string.Format("{0} \t {1} : {2} \n", message, property, value);
            }

            return string.Format("Properties: \n {0}", message);
        }

        private static IEnumerable<string> GetPropertyNames(object theObject)
        {
            var propertyNames = new List<string>();

            foreach (var property in theObject.GetType().GetProperties())
            {
                propertyNames.Add(property.Name);
            }

            return propertyNames;
        }
    }
}