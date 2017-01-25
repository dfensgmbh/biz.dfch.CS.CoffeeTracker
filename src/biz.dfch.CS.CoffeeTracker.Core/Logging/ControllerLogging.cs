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

using System.Diagnostics;
using biz.dfch.CS.CoffeeTracker.Core.Model;
using biz.dfch.CS.Commons.Diagnostics;

namespace biz.dfch.CS.CoffeeTracker.Core.Logging
{
    public static class ControllerLogging
    {
        public static class ModelNames
        {
            public const string COFFEE = "Coffee";
            public const string COFFEEORDER = "CoffeeOrder";
            public const string USER = "User";
        }

        public static void LogGetEntities(string modelname)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
               .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, Message.Get_Entities, modelname);
        }

        public static void LogGetEntity(string modelname, string id)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, Message.Get_Entity, modelname, id);
        }

        public static void LogUpdateEntityStartPut(string modelname, string id)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, Message.Update_Entity_Put_Start, modelname, id);
        }

        public static void LogUpdateEntityStopPut(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
               .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, Message.Update_Entity_Put_Stop, modelname, entity.Id, Logging.FormatEntity(entity));
        }

        public static void LogInsertEntityStart(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, Message.Insert_Entity_Start, modelname, Logging.FormatEntity(entity));
        }

        public static void LogInsertEntityStop(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                            .TraceEvent(TraceEventType.Stop, (int)Logging.EventId.Stop, Message.Insert_Entity_Stop, modelname, entity.Id);
        }

        public static void LogUpdateEntityStartPatch(string modelname, string id)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                          .TraceEvent(TraceEventType.Start, (int)Logging.EventId.Start, Message.Update_Entity_Patch_Start, modelname, id);
        }

        public static void LogUpdateEntityStopPatch(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Information, (int)Logging.EventId.Default, Message.Update_Entity_Patch_Stop, modelname, entity.Id, Logging.FormatEntity(entity));
        }

        public static void LogDeleteEntityStart(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
              .TraceEvent(TraceEventType.Warning, (int)Logging.EventId.Default, Message.Delete_Entity_Start, modelname, entity.Id, Logging.FormatEntity(entity));
        }

        public static void LogDeleteEntityStop(string modelname, BaseEntity entity)
        {
            Logger.Get(Logging.TraceSourceName.COFFEETRACKER_CONTROLLERS)
                .TraceEvent(TraceEventType.Warning, (int)Logging.EventId.Default, Message.Delete_Entity_Stop, modelname, entity.Id);
        }

        
    }
}