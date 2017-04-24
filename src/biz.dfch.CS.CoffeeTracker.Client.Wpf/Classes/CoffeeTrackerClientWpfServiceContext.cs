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
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes
{
    public class CoffeeTrackerClientWpfServiceContext : CoffeeTrackerServiceContext
    {
        public event EventHandler<ReceivingResponseEventArgs> UnAuthorizedThrown;

        public CoffeeTrackerClientWpfServiceContext(string hostUri) 
            : base(hostUri)
        {
            this.ReceivingResponse += CheckAndRaiseUnAuthorized;
        }

        public CoffeeTrackerClientWpfServiceContext(string hostUri, string userName, string password) 
            : base(hostUri, userName, password)
        {
        }

        private void CheckAndRaiseUnAuthorized(object sender, ReceivingResponseEventArgs args)
        {
            if (null == UnAuthorizedThrown)
            {
                return;
            }
            UnAuthorizedThrown.Invoke(this, args);
        }
    }
}
