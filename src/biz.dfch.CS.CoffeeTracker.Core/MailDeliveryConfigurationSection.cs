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
using System.Configuration;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.CoffeeTracker.Core
{
    public class MailDeliveryConfigurationSection : ConfigurationSection
    {
        private const string USERNAME_ATTRIBUTE_NAME = "username";
        private const string PASSWORD_ATTRIBUTE_NAME = "password";

        //<configuration>

        //<!-- Configuration section-handler declaration area. -->
        //  <configSections>
        //   <section 
        //    name="mailDeliveryConfigurationSection" 
        //    type="biz.dfch.CS.CoffeeTracker.Core.MailDeliveryConfigurationSection, biz.dfch.CS.CoffeeTracker.Core" 
        //    allowLocation="true" 
        //    allowDefinition="Everywhere" 
        //   />
        //      <!-- Other <section> and <sectionGroup> elements. -->
        //  </configSections>

        //  <!-- Configuration section settings area. -->
        //  <mailDeliveryConfigurationSection username="arbitrary@examplex.com" password="********" />
        //</configuration>

        [ConfigurationProperty(USERNAME_ATTRIBUTE_NAME, IsRequired = true)]
        public string Username
        {
            get { return (string)this[USERNAME_ATTRIBUTE_NAME]; }
            set { this[USERNAME_ATTRIBUTE_NAME] = value; }
        }

        [ConfigurationProperty(PASSWORD_ATTRIBUTE_NAME, IsRequired = true)]
        public string Password
        {
            get { return (string)this[PASSWORD_ATTRIBUTE_NAME]; }
            set { this[PASSWORD_ATTRIBUTE_NAME] = value; }
        }
    }
}