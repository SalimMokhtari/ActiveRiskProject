using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace TestSpecFlow
{
    public class CommonMethods
    {

        private IWebDriver webDriver = null;

        public IWebDriver ChromeWebDriver
        {
            get
            {
                if (webDriver == null)
                {
                    webDriver = SetWebDriver();
                }
                return webDriver;
            }
        }

        private IWebDriver SetWebDriver()
        {
            try
            {
                var profile = new ChromeOptions();
                profile.AcceptInsecureCertificates = true;
                profile.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
                return new ChromeDriver(profile);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GoToUrl(string url)
        {
            try
            {
                ChromeWebDriver.Navigate().GoToUrl(url);
                ChromeWebDriver.Manage().Window.Maximize();
                ChromeWebDriver.SwitchTo().ActiveElement();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IWebElement FindElement(By by, int interval = 500, int timeout = 15000)
        {
            IWebElement webElement = null;
            var tick = 0;
            try
            {
                do
                {
                    try
                    {
                        webElement = ChromeWebDriver.FindElement(by);
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (webElement == null && tick <= timeout);
                if (webElement == null)
                {
                    throw new TimeoutException(
                        string.Format("Element {0} was not found within {1} seconds ", by.ToString(), (timeout / 1000).ToString()));
                }
                return webElement;
            }
            catch (Exception)
            {
                throw;
            }
        }  

        public List<IWebElement> FindElements(By by, int interval = 500, int timeout = 15000)
        {

            var elements = new List<IWebElement>();
            var tick = 0;

            try
            {
                do
                {
                    try
                    {
                        elements = ChromeWebDriver.FindElements(by).ToList();
                        if (elements.Count == 0)
                        {
                            Thread.Sleep(interval);
                            tick += interval;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(interval);
                        tick += interval;
                    }
                } while (elements.Count == 0 && tick < timeout);
                return elements;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DisposeDriver()
        {
            try
            {
                ChromeWebDriver.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<string, string> GetUrlsAndBlogNumbers(string blogListXpath)
        {
            var elements = FindElements(By.XPath(blogListXpath));
            Dictionary<string, string> urlAndNumberOfBlogs = new Dictionary<string, string>();

            foreach (IWebElement t in elements) //Looping through the elements found and extracting the url for each month and the expected number of blogs. etc
            {
                string urlFull = t.GetAttribute("innerHTML"); //Getting the link data as it appears on the blog list page i.e: appended with the number of blogs between parentheses
                string url = urlFull.Split(';')[0]; //Splitting the blog url and the number of blogs as they are separated by a ;
                string blogsNumber = urlFull.Split(';')[1];
                string extractedUrl = Regex.Match(url, @"href=\""(.*?)\""").Groups[1].Value;
                string extractedBlogsNumber = Regex.Match(blogsNumber, @"\(([^)]*)\)").Groups[1].Value;
                urlAndNumberOfBlogs.Add(extractedUrl, extractedBlogsNumber); //Storing each month url and its expected blog count in the hash table
            }
            return urlAndNumberOfBlogs;
        }

        public void VerifyEachBlogPage(Dictionary<string, string> urlsAndBlogNumbers)
        {
            Boolean foundError = false;
            foreach (KeyValuePair<string, string> w in urlsAndBlogNumbers)  //Navigating to the blog page each month and verifying if the blog count extracted earlier is correct
            {
                GoToUrl(w.Key); //w.Key contains the url of a specific month and w.Value is its number of expected blogs 
                Thread.Sleep(200);
                var articles = FindElements(By.XPath("//*[*]/article")); //Getting the articles from a particular month blog page
                try
                {
                    Assert.AreEqual(Int32.Parse(w.Value), articles.Count); //Checking that the number of articles found on a particular month page corresponds to the number of blogs extracted earlier from the link  
                }
                catch
                {
                    foundError = true;
                    Console.WriteLine("The number of blogs for the following month " + w.Key + " should be " + w.Value + " but was found to have " + articles.Count + " blogs instead");//Logging the month whose number of blogs is not correct 
                }
            }
            if (foundError)
            {
                DisposeDriver();
                throw new Exception(string.Format("There are months whose displayed number of blogs does not correspond to its actual number of articles. Please see console logs for details"));//Failing the testcase and pointing to the detail of which month has got an incorrect number of blogs
            }

        }
    }
}
