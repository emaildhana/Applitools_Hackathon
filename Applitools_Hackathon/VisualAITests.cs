using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using System.Threading;
using Applitools.Selenium;
using OpenQA.Selenium;
using Applitools;

namespace Applitools_Hackathon
{
    [TestClass]
    public class VisualAITests
    {
        static RemoteWebDriver driver;
        static LoginPageUIElements loginPageUIElements;
        static LandingPage landingPage;
                string appName = "Applitools Hackathon"; 

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            //driver.Navigate().GoToUrl("https://demo.applitools.com/hackathon.html"); //Version 1
            driver.Navigate().GoToUrl("https://demo.applitools.com/hackathonV2.html"); //Version 2

            loginPageUIElements = new LoginPageUIElements(driver);
            landingPage = new LandingPage(driver);
        }

        [TestMethod]
        [Description("Login Page UI Elements Test")]
        public void Test1_LoginPageUIElementsTest()
        {

            Eyes eyes = new Eyes();
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_HACKATHON_API_KEY");

            eyes.Open(driver, appName, "Login Page UI Elements Test");
            eyes.CheckWindow();
            eyes.Close();
            eyes.AbortIfNotClosed();
        }

        [DataTestMethod]
        [DataRow("", "", "Both Username and Password must be present", "", "1")] 
        [DataRow("abcd", "", "Password must be present", "", "2")]
        [DataRow("", "abcd", "Username must be present", "","3")]
        [DataRow("abcd", "abcd", "", "https://demo.applitools.com/hackathonAppV2.html", "4")]

        [Description("Data Driven Test")]

        public void Test2_DataDrivenTest(string userNameValue, string passwordValue, string alertMsgValue, string urlValue, string testNameValue)
        {
            Eyes eyes = new Eyes();
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_HACKATHON_API_KEY");
            BatchInfo batchInfo = new BatchInfo("Data Driven Test");
            batchInfo.Id = "DataDrivenTest";
            eyes.Batch = batchInfo;
            string testValue = $"Data Driven Test - {testNameValue}";
            eyes.Open(driver,appName, testValue); 
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

            eyes.CheckWindow(); 

            try
            {
                loginPageUIElements.userNameInput.Clear();
                loginPageUIElements.passwordInput.Clear();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("User name and password elements are not present");
            }

            eyes.Close();
            eyes.AbortIfNotClosed();
        }

        [TestMethod]
        [Description("Table Sort Test")]
        public void Test3_TableSortTest()
        {
            Eyes eyes = new Eyes();
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_HACKATHON_API_KEY");

            InduceDelay(1);
            eyes.ForceFullPageScreenshot = true; 
            eyes.Open(driver, appName, "Table Sort Test");

            landingPage.amountHeader.Click();
            InduceDelay(1);
            eyes.CheckWindow();

            eyes.Close();
            eyes.AbortIfNotClosed();
        }

        [TestMethod]
        [Description("Canvas Chart Test")]
        public void Test4_CanvasChartTest()
        {
            Eyes eyes = new Eyes();
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_HACKATHON_API_KEY");

            BatchInfo batchInfo = new BatchInfo("Canvas Chart Test");
            batchInfo.Id = "CanvasChartTest";

            eyes.Batch = batchInfo;

            eyes.Open(driver, appName, "Canvas Chart Test");

            landingPage.compareExpensesBtn.Click();
            InduceDelay(1);
            eyes.CheckWindow();
            eyes.Close();
            
            eyes.Open(driver, appName, "Canvas Chart Test - Next Year");
            landingPage.showNextYrDataBtn.Click();
            InduceDelay(1);
            eyes.CheckWindow();

            eyes.Close();
            eyes.AbortIfNotClosed();
        }

        [TestMethod]
        [Description("Dynamic Content Test")]
        public void Test5_DynamicContestTest()
        {
            Eyes eyes = new Eyes();
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_HACKATHON_API_KEY");

            eyes.Open(driver, appName, "Dynamic Content Test"); 
            //driver.Navigate().GoToUrl("https://demo.applitools.com/hackathon.html?showAd=true");
            driver.Navigate().GoToUrl("https://demo.applitools.com/hackathonV2.html?showAd=true");
            loginPageUIElements.userNameInput.SendKeys("abcd");
            loginPageUIElements.passwordInput.SendKeys("abcd");
            loginPageUIElements.loginBtn.Click();

            InduceDelay(1);
            eyes.CheckWindow();

            eyes.Close();
            eyes.AbortIfNotClosed();
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
