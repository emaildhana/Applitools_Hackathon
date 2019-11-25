using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System; 

namespace Applitools_Hackathon
{
    class LandingPage
    {
        private readonly RemoteWebDriver _driver;

        public IWebElement amountHeader => _driver.FindElementByXPath("//th[@id='amount']");
        public IReadOnlyCollection<IWebElement> amountColumn => _driver.FindElementsByXPath("//tbody//td[5]");
        public IWebElement compareExpensesBtn => _driver.FindElementByXPath("//a[@id='showExpensesChart']");
        public IWebElement showNextYrDataBtn => _driver.FindElementByXPath("//button[@id='addDataset']"); 


        public IReadOnlyCollection<IWebElement> flashSaleGifs => _driver.FindElementsByXPath("//div[contains(@id, 'flashSale')]/img");

        public LandingPage(RemoteWebDriver driver)
        {
            _driver = driver; 
        }

        public bool ValidateFlashSaleGif(int expectedNumberOfGif)
        {
            int actualNumberOfGifs = flashSaleGifs.Count;

            if (expectedNumberOfGif.Equals(actualNumberOfGifs))
            {
                Console.WriteLine("Validation of number of flash sale gifs passed");
                Console.WriteLine($"Number of flash sale gifs present {actualNumberOfGifs}");
                return true;
            }
            else if (actualNumberOfGifs == 0)
            {
                Console.WriteLine("Validation of number of flash sale gifs failed");
                Console.WriteLine($"Number of flash sale gifs present {actualNumberOfGifs}");
                return false;
            }
            else
            {
                Console.WriteLine("Validation of number of flash sale gifs failed");
                Console.WriteLine($"Number of flash sale gifs present {actualNumberOfGifs}");
                return false;
            }
        }

        public bool ValidateFlashSaleGif(List<string> expectedFlashSaleGifsURI)
        {
            List<string> actualURI = new List<string>();

            actualURI = flashSaleGifs.Select(x => x.GetAttribute("src")).ToList();

            if (expectedFlashSaleGifsURI.SequenceEqual(actualURI))
            {
                Console.WriteLine("Validation of gif - Passed");
                return true;
            }
            else
            {
                IEnumerable<string> missingURI = expectedFlashSaleGifsURI.Except(actualURI);
                Console.WriteLine("Validation of gif - failed");
                Console.WriteLine("Expected Gif's not displayed");
                foreach(var item in missingURI)
                {
                    Console.WriteLine(item);
                }
                return false;
            }
        }
    }
}
