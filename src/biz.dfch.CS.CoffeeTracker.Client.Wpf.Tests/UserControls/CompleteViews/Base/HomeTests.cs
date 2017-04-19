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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.UserControls.CompleteViews.Base
{
    [TestClass]
    public class HomeTests
    {
        private Application application;

        [TestInitialize]
        public void StartApplication()
        {
            application = Application.Launch(SharedTestData.ExecutablePath);
        }

        [TestMethod]
        public void LoginGetsToHomeSucceeds()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);

            // DF-ToDo Arrange Mocking using JustMock

            //// Get Email Textbox
            var emailLabeledTextBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginEmail"));
            var emailTextBox = emailLabeledTextBox.Get(SearchCriteria.ByAutomationId("EmailTextBox"));

            //// Get Password TextBox
            var passwordLabeledPasswordBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginPassword"));
            var passwordBox = passwordLabeledPasswordBox.Get(SearchCriteria.ByClassName("PasswordBox"));

            //// Get Additional Controls
            var loginButton = baseWindow.Get(SearchCriteria.ByAutomationId("LoginButton"));

            //Act 
            emailTextBox.Enter(SharedTestData.UserWhichExists);
            passwordBox.Enter(SharedTestData.PasswordForUserWhichExists);
            loginButton.Click();
            baseWindow.WaitWhileBusy();

            // Act

            // Assert
        }

        [TestCleanup]
        public void CloseApplicationIfNotAlreadyClosed()
        {
            try
            {
                application.Close();
            }
            catch (Exception)
            {
                application.Kill();
            }
        }

        private void GetToHome()
        {
            var startWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            var emailLabeledTextBox = startWindow.Get(SearchCriteria.ByAutomationId("LoginEmail"));
            emailLabeledTextBox.Get(SearchCriteria.ByAutomationId("EmailTextBox")).Enter(SharedTestData.UserWhichExists);
            var passwordLabeledPasswordBox = startWindow.Get(SearchCriteria.ByAutomationId("LoginPassword"));
            passwordLabeledPasswordBox.Get(SearchCriteria.ByClassName("PasswordBox")).Enter(SharedTestData.PasswordForUserWhichExists);
            startWindow.Get(SearchCriteria.ByAutomationId("LoginButton")).Click();
            startWindow.WaitWhileBusy();
        }
    }
}
