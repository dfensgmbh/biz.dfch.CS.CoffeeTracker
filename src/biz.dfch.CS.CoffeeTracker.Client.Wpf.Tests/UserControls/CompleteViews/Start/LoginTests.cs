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
using System.Net.Mime;
using System.Windows;
using biz.dfch.CS.CoffeeTracker.Client.Wpf.UserControls.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;
using Application = TestStack.White.Application;
using Button = TestStack.White.UIItems.Button;
using Label = TestStack.White.UIItems.Label;
using TextBox = TestStack.White.UIItems.TextBox;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Tests.UserControls.CompleteViews.Start
{
    [TestClass]
    public class LoginTests
    {
        private readonly string applicationPath = Path.Combine(AppContext.BaseDirectory, "biz.dfch.CS.CoffeeTracker.Client.Wpf.exe");
        private readonly string UserNameWhichShouldNotExist = "NotExistentName";
        private readonly string InvalidPassword = "InvPa"; //InvPa = InvalidPassword, it contains 5 characters while the password needs at least 6, so it's invalid

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LaunchAndCloseApplicationSucceeds()
        {
            var sut = Application.Launch(applicationPath);
            sut.Close();

            // Should Throw an InvalidOperationException, because the process doesn't exist anymore
            var arbitraryVar = sut.Name;
        }

        [TestMethod]
        public void SwitchToRegisterByHyperLinkOnClickSucceeds()
        {
            //Arrange
            var application = Application.Launch(applicationPath);
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);
            var hyperLink = baseWindow.Get<Label>("LoginHyperLink");

            //Act
            hyperLink.Click();

            //Assert
            var result = baseWindow.Title;
            application.Close();

            Assert.AreEqual(Resources.LanguageResources.Resources.Registration_Title, result);
        }

        [TestMethod]
        public void TryLoginPassWrongCredentialsShowsLoadingAndAfterLoadInvalidMessage()
        {
            //Arrange
            var application = Application.Launch(applicationPath);
            var baseWindow = application.GetWindow(Resources.LanguageResources.Resources.Login_Title);

            //// Get Email Textbox
            var emailTextBoxSearchCriteria = SearchCriteria.ByAutomationId("LoginEmail");
            var emailLabeledTextBox = baseWindow.Get(emailTextBoxSearchCriteria);
            var emailTextBox = emailLabeledTextBox.Get<TextBox>("EmailTextBox");

            //// Get Password TextBox
            var passwordTextBoxSearchCriteria = SearchCriteria.ByAutomationId("LoginPassword");
            var passwordBox = baseWindow.Get(passwordTextBoxSearchCriteria);

            //// Get Additional Controls
            var loginButton = baseWindow.Get<Button>("LoginButton");


            var hyperLink = baseWindow.Get<Label>("LoginHyperLink");

            //Act / Assert
            emailTextBox.Enter(UserNameWhichShouldNotExist);
            passwordBox.Enter(InvalidPassword);
            loginButton.Click();

            //// Get Loading 
            var progressRingCriteria = SearchCriteria.ByAutomationId("ProgressRing");
            var progressRing = baseWindow.Get(progressRingCriteria);


            Assert.IsTrue(progressRing.Visible);

            Assert.IsFalse(hyperLink.Visible);
            //Assert.IsFalse(invalidCredentialsTextBlock.Visible);
            Assert.IsFalse(emailTextBox.Enabled);
            Assert.IsFalse(passwordBox.Enabled);
            Assert.IsFalse(loginButton.Enabled);
            baseWindow.WaitWhileBusy();


            application.Close();
        }
    }
}
