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
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace biz.dfch.CS.CoffeeTracker.Core.Managers
{
    public class EmailManager
    {
        private readonly ExchangeService service;

        public EmailManager()
        {
            var userMail = "user";
            var password = "pw";

            service = new ExchangeService();
            service.Credentials = new WebCredentials(userMail, password);
            service.UseDefaultCredentials = true;
            service.AutodiscoverUrl(userMail);
        }
                
        public void CreateAndSendOutOfStockEmail(IEnumerable<EmailAddress> recipients)
        {
            Contract.Requires(null != recipients);

            var message = new EmailMessage(service)
            {
                Subject = "Coffeestock",
                Body = new MessageBody("A coffee is about to run out of stock m8")
            };

            foreach (var recipient in recipients)
            {
                message.ToRecipients.Add(recipient);
            }

            message.Send();
            Contract.Assert(message.IsSubmitted, "|500|");
        }
    }
}