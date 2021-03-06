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
namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Logging
{
    public static class Logging
    {
        public enum EventId
        {
            Default = 0,

            Stop = int.MaxValue - 2,
            Start = int.MaxValue - 1,
            Exception = int.MaxValue
        }

        public static class TraceSourceName
        {
            public const string WPF_START = "biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.CompleteViews.Start";
            public const string WPF_RUNNING = "biz.dfch.CS.CoffeeTracker.Core.Wpf.UserControls.CompleteViews.Base";
            public const string WPF_COMPONENTS = "biz.dfch.CS.CoffeeTracker.Core.Wpf.UserControls.Components";
            public const string WPF_GENERAL= "biz.dfch.CS.CoffeeTracker.Core.Wpf";
        }
    }
}
