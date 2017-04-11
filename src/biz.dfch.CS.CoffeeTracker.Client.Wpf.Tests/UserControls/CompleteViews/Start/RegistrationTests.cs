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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.UserControls.CompleteViews.Start
{
    [TestClass]
    public class RegistrationTests
    {
        private static string _applicationPath = "";
        private const string VALID_EMAIL = "email@example.com";
        private const string VALID_PASSWORD = "123456";

        private const string INVALID_EMAIL = "ThisIsNotAMail";
        private const string INVALID_PASSWORD = "InvPa"; // Invalid because it has less than 6 characters

        private Application application;

        [TestInitialize]
        public void StartApplication()
        {
            _applicationPath = SharedTestData.GetExecutablePath();
            application = Application.Launch(_applicationPath);
        }


        [TestMethod]
        public void RegistrationSwitchToRegisterPageAndBackChangesTitleSucceeds()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);

            //Act / Assert
            var registerHyperLink = baseWindow.Get(SearchCriteria.ByAutomationId("LoginHyperLink"));
            registerHyperLink.Click();

            Assert.AreEqual(Resources.LanguageResources.Resources.Registration_Title, baseWindow.Title);
            var loginHyperLink = baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationLoginHyperLink"));

            loginHyperLink.Click();
            Assert.AreEqual(Resources.LanguageResources.Resources.Login_Title, baseWindow.Title);
            application.Close();
        }

        // If the test fails, make sure the user doesn't already exist in the database
        [TestMethod]
        public void RegistrationWithValidFormularRedirectsToLoginPage()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            baseWindow.Get(SearchCriteria.ByAutomationId("LoginHyperLink")).Click();
            

            // Act
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationEmailTextBox")).Get(SearchCriteria.ByClassName("TextBox")).Enter(VALID_EMAIL);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordPasswordBox")).Get(SearchCriteria.ByClassName("PasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnterPasswordPasswordBox")).Get(SearchCriteria.ByClassName("PasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationButton")).Click();
            baseWindow.WaitWhileBusy();

            // Assert
            Assert.IsTrue(baseWindow.Get(SearchCriteria.ByAutomationId("LoginMessageTextBlock")).Visible);
            application.Close();
        }
    }
}