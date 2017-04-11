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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;
using Application = TestStack.White.Application;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.UserControls.CompleteViews.Start
{
    [TestClass]
    public class LoginTests
    {
        private readonly string _userNameWhichShouldNotExist = "NotExistentName";
        private readonly string _invalidPassword = "InvPa"; //InvPa = _invalidPassword, it contains 5 characters while the password needs at least 6, so it's invalid

        private readonly string _userWhichExists = "steven.pilatschek@d-fens.net";
        private readonly string _userPassword = "123456";

        private static string _applicationPath = "";

        private Application application;

        [TestInitialize]
        public void StartApplication()
        {
            _applicationPath = SharedTestData.GetExecutablePath();
            application = Application.Launch(_applicationPath);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LoginLaunchAndCloseApplicationSucceeds()
        {
            application.Close();

            // Should Throw an InvalidOperationException, because the process doesn't exist anymore
            var arbitraryVar = application.Name;
        }

        [TestMethod]
        public void LoginSwitchToRegisterByHyperLinkOnClickSucceeds()
        {
            //Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            var hyperLink = baseWindow.Get(SearchCriteria.ByAutomationId("LoginHyperLink"));

            //Act
            hyperLink.Click();

            //Assert
            var result = baseWindow.Title;
            application.Close();

            Assert.AreNotEqual(Resources.LanguageResources.Resources.Login_Title, result);
        }

        [TestMethod]
        public void LoginTryLoginPassWrongCredentialsShowsLoadingAndAfterLoadInvalidMessage()
        {
            //Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);

            //// Get Email Textbox
            var emailLabeledTextBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginEmail"));
            var emailTextBox = emailLabeledTextBox.Get(SearchCriteria.ByAutomationId("EmailTextBox"));

            //// Get Password TextBox
            var passwordLabeledPasswordBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginPassword"));
            var passwordBox = passwordLabeledPasswordBox.Get(SearchCriteria.ByClassName("PasswordBox"));

            //// Get Additional Controls
            var loginButton = baseWindow.Get(SearchCriteria.ByAutomationId("LoginButton"));

            //Act 
            emailTextBox.Enter(_userNameWhichShouldNotExist);
            passwordBox.Enter(_invalidPassword);
            loginButton.Click();
            baseWindow.WaitWhileBusy();

            //Assert
            var invalidMessageTextBlock = baseWindow.Get(SearchCriteria.ByAutomationId("LoginMessageTextBlock"));

            Assert.IsTrue(invalidMessageTextBlock.Visible);

            application.Close();
        }

        [TestMethod]
        public void LoginTryLoginPassCorrectCredentialsShowsLoadingAndAfterLoadClosesWindow()
        {
            //Arrange
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);

            //// Get Email Textbox
            var emailLabeledTextBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginEmail"));
            var emailTextBox = emailLabeledTextBox.Get(SearchCriteria.ByAutomationId("EmailTextBox"));

            //// Get Password TextBox
            var passwordLabeledPasswordBox = baseWindow.Get(SearchCriteria.ByAutomationId("LoginPassword"));
            var passwordBox = passwordLabeledPasswordBox.Get(SearchCriteria.ByClassName("PasswordBox"));

            //// Get Additional Controls
            var loginButton = baseWindow.Get(SearchCriteria.ByAutomationId("LoginButton"));

            //Act 
            emailTextBox.Enter(_userWhichExists);
            passwordBox.Enter(_userPassword);
            loginButton.Click();
            baseWindow.WaitWhileBusy();

            // The window closes after WaitWhileBusy() hence IsClosed is still set to false.
            // Next command helps prevent this issue, so the assertion won't be executed to early
            Thread.Sleep(3000);

            // Assert
            Assert.IsTrue(baseWindow.IsClosed);
            
            application.Close();
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
