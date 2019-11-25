using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote; 

namespace Applitools_Hackathon
{
    public enum LoginElements
    {
             _logoImage
            ,_loginFormHeader
            ,_userNameLabel
            , _userNameInput
            , _userNameIcon
            , _passwordLabel
            , _passwordInput
            , _passwordIcon
            , _loginBtn
            , _rememberMeLabel
            , _rememberMeChkBox
            , _twitterImage
            , _facebookImage
            , _linkedInImage
    };
    class LoginPageUIElements
    {
        private readonly RemoteWebDriver _driver;

        #region Web Elements in Login page
        public IWebElement logoImage => _driver.FindElementByXPath("//img[@src='img/logo-big.png']");
        //public IWebElement loginFormHeader => _driver.FindElementByXPath("//h4[contains(text(), 'Login Form')]"); Commented due to changes in V2

        public IWebElement loginFormHeader => _driver.FindElementByXPath("//h4[contains(text(), 'Logout Form')]");
        public IWebElement userNameLabel => _driver.FindElementByXPath("//label[contains(text(), 'Username')]");
        //public IWebElement userNameInput => _driver.FindElementByXPath("//input[@id='username' and @placeholder ='Enter your username']");

        public IWebElement userNameInput => _driver.FindElementByXPath("//input[@id='username']");
        // public IWebElement userNameIcon => _driver.FindElementByXPath("//div[contains(@class,'male-circle')]"); Commented due to changes in V2

        public IWebElement passwordLabel => _driver.FindElementByXPath("//label[contains(text(), 'Pwd')]"); //Updated due to changes in V2
        //public IWebElement passwordInput => _driver.FindElementByXPath("//input[@id='password' and @placeholder ='Enter your password']");
        public IWebElement passwordInput => _driver.FindElementByXPath("//input[@id='password']");
        // public IWebElement passwordIcon => _driver.FindElementByXPath("//div[contains(@class,'fingerprint')]"); Commented due to changes in V2

        public IWebElement loginBtn => _driver.FindElementByXPath("//button[@id='log-in']"); 

        public IWebElement rememberMeLabel => _driver.FindElementByXPath("//label[contains(text(), 'Remember Me')]");
        public IWebElement rememberMeChkBox => _driver.FindElementByXPath("//label[contains(text(), 'Remember Me')]/input[@type='checkbox']");

        public IWebElement twitterImage => _driver.FindElementByXPath("//img[@src='img/social-icons/twitter.png']"); 
        public IWebElement facebookImage => _driver.FindElementByXPath("//img[@src='img/social-icons/facebook.png']");
        //public IWebElement linkedInImage => _driver.FindElementByXPath("//img[@src='img/social-icons/linkedin.png']"); Commented due to changes in V2
        #endregion

        public IWebElement alertMsg => _driver.FindElementByXPath("//div[contains(@class, 'alert-warning')]");
        Dictionary<LoginElements, IWebElement> loginPageWebElementMap = new Dictionary<LoginElements, IWebElement>();

        public LoginPageUIElements(RemoteWebDriver driver)
        {
            _driver = driver; 
        }

        /// <summary>
        /// Method to initialize the map between login element and its locator
        /// </summary>
        public void InitLoginPageWebElementMap()
        {
            loginPageWebElementMap.Add(LoginElements._logoImage, logoImage);
            loginPageWebElementMap.Add(LoginElements._loginFormHeader, loginFormHeader);
            loginPageWebElementMap.Add(LoginElements._userNameLabel, userNameLabel);
            loginPageWebElementMap.Add(LoginElements._userNameInput, userNameInput);
            //loginPageWebElementMap.Add(LoginElements._userNameIcon, userNameIcon);
            loginPageWebElementMap.Add(LoginElements._passwordLabel, passwordLabel);
            loginPageWebElementMap.Add(LoginElements._passwordInput, passwordInput);
            //loginPageWebElementMap.Add(LoginElements._passwordIcon, passwordIcon);
            loginPageWebElementMap.Add(LoginElements._loginBtn, loginBtn);
            loginPageWebElementMap.Add(LoginElements._rememberMeLabel, rememberMeLabel);
            loginPageWebElementMap.Add(LoginElements._rememberMeChkBox, rememberMeChkBox);
            loginPageWebElementMap.Add(LoginElements._twitterImage, twitterImage);
            loginPageWebElementMap.Add(LoginElements._facebookImage, facebookImage);
            //loginPageWebElementMap.Add(LoginElements._linkedInImage, linkedInImage);
        }

        /// <summary>
        /// Method to check if the web elements are displayed in the page
        /// </summary>
        /// <param name="expectedElements"></param>
        /// <returns>bool</returns>

        public bool CheckForElementsExist(List<LoginElements> expectedElements)
        {
            List<LoginElements> actualElements = new List<LoginElements>(); 
            foreach (var item in expectedElements)
            {
                if (loginPageWebElementMap[item].Displayed)
                    actualElements.Add(item); 
            }

            if(expectedElements.SequenceEqual(actualElements))
            {
                Console.WriteLine("All the elements are present in login page");
                return true; 
            }
            else
            {
                Console.WriteLine($"Elements not present {expectedElements.Except(actualElements)}");
                return false; 
            }
        }
    }
}

