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
using System.IO;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests
{
    public static class SharedTestData
    {
        public static readonly string ProjectName = "biz.dfch.CS.CoffeeTracker.Client.Wpf";
        public static readonly string UserWhichExists = "steven.pilatschek@d-fens.net";
        public static readonly string PasswordForUserWhichExists = "123456";


        public static string GetExecutablePath()
        {
            // create path to executable file in biz.dfch.CS.CoffeeTracker.Client.Wpf/bin/Debug
            var baseDirectory = AppContext.BaseDirectory;
            var toReplace = string.Format("{0}{1}", ProjectName, ".Tests");
            var newBaseDirectory = baseDirectory.Replace(toReplace, ProjectName);
            return Path.Combine(newBaseDirectory, ProjectName + ".exe");
        }
    }
}
