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
using System.Data.Services.Client;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.CustomEvents;
using biz.dfch.CS.Commons.Diagnostics;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes
{
    public class CoffeeTrackerClientWpfServiceContext : CoffeeTrackerServiceContext
    {
        public event EventHandler<StatusCodeEventArgs> OnExceptionalStatusCode;

        public CoffeeTrackerClientWpfServiceContext(string hostUri)
            : base(hostUri)
        {
            ReceivingResponse += CheckAndRaiseExceptionFromServer;
        }

        public CoffeeTrackerClientWpfServiceContext(string hostUri, string userName, string password)
            : base(hostUri, userName, password)
        {
        }

        private void CheckAndRaiseExceptionFromServer(object sender, ReceivingResponseEventArgs args)
        {
            Contract.Requires(null != args);
            Contract.Requires(null != args.ResponseMessage);
            Logger.Get(Logging.Logging.TraceSourceName.WPF_GENERAL)
                .TraceEvent(TraceEventType.Error, (int) Logging.Logging.EventId.Exception,
                    args.ResponseMessage.ToString());

            if (null == OnExceptionalStatusCode)
            {
                return;
            }

            if (400 > args.ResponseMessage.StatusCode)
            {
                return;
            }

            var receivedStatusCode = (HttpStatusCode) args.ResponseMessage.StatusCode;
            var statusCodeEventArgs = new StatusCodeEventArgs(receivedStatusCode);
            OnExceptionalStatusCode.Invoke(this, statusCodeEventArgs);
        }
    }
}