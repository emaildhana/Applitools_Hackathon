using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium; 
using System.Collections.Generic;
using System.Linq; 
using System.Threading;

namespace Applitools_Hackathon
{
    [TestClass]
    public class TraditionalTests
    {
        static RemoteWebDriver driver;
        static LoginPageUIElements loginPageUIElements;
        static LandingPage landingPage; 

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            //driver.Navigate().GoToUrl("https://demo.applitools.com/hackathon.html"); //Version 1
            driver.Navigate().GoToUrl("https://demo.applitools.com/hackathonV2.html"); //Version 1

            loginPageUIElements = new LoginPageUIElements(driver);
            landingPage = new LandingPage(driver); 
        }

        [TestMethod]
        [Description("Login Page UI Elements Test")]
        public void Test1_LoginPageUIElementsTest()
        {
            List<LoginElements> expectedLoginPageUIElements = new List<LoginElements>()
            {
			 LoginElements._logoImage
			,LoginElements._loginFormHeader
			,LoginElements._userNameLabel
			,LoginElements._userNameInput
			//,LoginElements._userNameIcon
			,LoginElements._passwordLabel
			,LoginElements._passwordInput
			//,LoginElements._passwordIcon
			,LoginElements._loginBtn
			,LoginElements._rememberMeLabel
			,LoginElements._rememberMeChkBox
			,LoginElements._twitterImage
			,LoginElements._facebookImage
			//,LoginElements._linkedInImage
			};

            loginPageUIElements.InitLoginPageWebElementMap();

            Assert.IsTrue(loginPageUIElements.CheckForElementsExist(expectedLoginPageUIElements));
        }

        [DataTestMethod]
        [DataRow("","", "Please enter both username and password", "")] //Updated the error message due to V2 changes
        [DataRow("abcd", "", "Password must be present", "")]
        [DataRow("", "abcd", "Username must be present", "")]
        [DataRow("abcd", "abcd", "", "https://demo.applitools.com/hackathonAppV2.html")]//Updated the error message due to V2 changes

        [Description("Data Driven Test")]
        
        public void Test2_DataDrivenTest(string userNameValue, string passwordValue, string alertMsgValue, string urlValue)
        {
            InduceDelay(2);
            Console.WriteLine($"user name {userNameValue} - password {passwordValue} - alert msg {alertMsgValue}");

            if (!String.IsNullOrEmpty(userNameValue))
            {
                loginPageUIElements.userNameInput.Clear();
                loginPageUIElements.userNameInput.SendKeys(userNameValue);
            }

            if (!String.IsNullOrEmpty(passwordValue))
            {
                loginPageUIElements.passwordInput.Clear();
                loginPageUIElements.passwordInput.SendKeys(passwordValue);
            }

            loginPageUIElements.loginBtn.Click();

            InduceDelay(2);

            string actualAlertMsg = null; 

            try
            {
                actualAlertMsg = loginPageUIElements.alertMsg.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Alert message is not present");
            }


            if (!String.IsNullOrEmpty(alertMsgValue))
            {
                Assert.AreEqual(alertMsgValue, actualAlertMsg);
            }
            else if (!String.IsNullOrEmpty(urlValue))
            {
                string actualURL = driver.Url;
                Assert.AreEqual(urlValue, actualURL);
            }

            try
            {
                loginPageUIElements.userNameInput.Clear();
                loginPageUIElements.passwordInput.Clear();
            }
            catch (NoSuchElementException)
            {

                Console.WriteLine("User name and password elements are not present");
            }
           
        }

        [TestMethod]
        [Description("Table Sort Test")]
        //Note: Have not automated the validation of in tact of each data row after sorting, since some of the cells contains image, text highlighted
        public void Test3_TableSortTest()
        {
            InduceDelay(1);

            var defaultAmountList = landingPage.amountColumn.Select(x => x.Text.Trim().Remove(x.Text.Length - 4).Replace(" ","")).ToList();
            var intList = defaultAmountList.Select(float.Parse).ToList();
            var orderedDefaultAmountList = intList.OrderBy(x => x); 


            landingPage.amountHeader.Click();
            InduceDelay(1);
            var sortedAmountString = landingPage.amountColumn.Select(x => x.Text.Trim().Remove(x.Text.Length - 4).Replace(" ","")).ToList();
            var sortedAmountNumber = sortedAmountString.Select(float.Parse).ToList(); 


            bool result = sortedAmountNumber.SequenceEqual(orderedDefaultAmountList); 

            if(result)
            {
                Console.WriteLine("Amount is sorted in ascending order");
            }
            else
            {
                Console.WriteLine("Amount is not sorted");
            }

            Assert.IsTrue(result); 
        }

        [TestMethod]
        [Description("Canvas Chart Test")]
        public void Test4_CanvasChartTest()
        {
            landingPage.compareExpensesBtn.Click();
            InduceDelay(1);

            //Note: Have not automated the validation of bar chart, since could not figure out a way to read the data in canvas tag

            landingPage.showNextYrDataBtn.Click(); 
        }

        [TestMethod]
        [Description("Dynamic Content Test")]
        public void Test5_DynamicContestTest()
        {
            driver.Navigate().GoToUrl("https://demo.applitools.com/hackathonV2.html?showAd=true");
            loginPageUIElements.userNameInput.SendKeys("abcd");
            loginPageUIElements.passwordInput.SendKeys("abcd");
            loginPageUIElements.loginBtn.Click(); 

            InduceDelay(1);

            List<string> expectedFlashSaleGifURI = new List<string>()
            {
                "https://demo.applitools.com/img/flashSale.gif"
                ,"https://demo.applitools.com/img/flashSale2.gif"
            };

            bool numberOfGifsResult = landingPage.ValidateFlashSaleGif(2);
            bool expectedGifsResult = landingPage.ValidateFlashSaleGif(expectedFlashSaleGifURI);

            Assert.IsTrue(numberOfGifsResult && expectedGifsResult); 
        }

        public void InduceDelay(int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        [ClassCleanup]
        public static void Close()
        {
            driver.Quit();
        }
    }
}
