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
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

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
            var registerHyperLink = baseWindow.Get<Label>("LoginHyperLink");
            registerHyperLink.Click();

            Assert.AreEqual(Resources.LanguageResources.Resources.Registration_Title, baseWindow.Title);
            var loginHyperLink = baseWindow.Get<Label>("RegistrationLoginHyperLink");

            loginHyperLink.Click();
            Assert.AreEqual(Resources.LanguageResources.Resources.Login_Title, baseWindow.Title);
        }

        [TestMethod]
        public void RegistrationTryWithTooShortPasswordTurnsMessageRed()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            baseWindow.Get<Label>("LoginHyperLink").Click();

            // Act
            var passwordRequirementsTextBlock =
                baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordRequirementsTextBlock"));

            baseWindow.Get<TextBox>("RegistrationEmailTextBox").Enter(VALID_EMAIL);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordPasswordBox")).Enter(INVALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnterPasswordPasswordBox")).Enter(INVALID_PASSWORD);
        }

        [TestMethod]
        public void RegistrationTryWithDifferentReEnteredPasswordTurnsBoxRed()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            baseWindow.Get<Label>("LoginHyperLink").Click();

            // Act
            var reEnteredPasswordTextBlock =
                baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnteredPasswordTextBlock"));

            // DF-ToDo - Find a way to access properties of a textbox

            baseWindow.Get<TextBox>("RegistrationEmailTextBox").Enter(VALID_EMAIL);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordPasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnterPasswordPasswordBox")).Enter(INVALID_PASSWORD);
        }

        [TestMethod]
        public void RegistrationTryWithInvalidMailShowsInvalidMailMessage()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            baseWindow.Get<Label>("LoginHyperLink").Click();

            // Act
            var emailTextBox = baseWindow.Get<TextBox>("RegistrationEmailTextBox");
            emailTextBox.Enter(INVALID_EMAIL);

            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordPasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnterPasswordPasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get<Button>("RegistrationRegistrateButton").Click();
            baseWindow.WaitWhileBusy();

            // Assert
            Assert.AreEqual(Color.Red, emailTextBox.BorderColor);
        }

        [TestMethod]
        public void RegistrationWithValidFormularRedirectsToLoginPageAndShowsRegisteredMessage()
        {
            // Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            baseWindow.Get<Label>("LoginHyperLink").Click();

            // Act
            baseWindow.Get<TextBox>("RegistrationEmailTextBox").Enter(VALID_EMAIL);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationPasswordPasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get(SearchCriteria.ByAutomationId("RegistrationReEnterPasswordPasswordBox")).Enter(VALID_PASSWORD);
            baseWindow.Get<Button>("RegistrationRegistrateButton").Click();
            baseWindow.WaitWhileBusy();

            // Assert
            // When this call doesn't end in an exception, the window is back in login and shows the message
            baseWindow.Get(SearchCriteria.ByAutomationId("LoginRegistrationSucceededTextBlock"));
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
    }
}