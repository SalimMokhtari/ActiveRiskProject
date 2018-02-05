using System;
using TechTalk.SpecFlow;
using System.Text;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace TestSpecFlow.CodeBindings
{
    [Binding]
    public class CheckForTheNumberOfBlogsSteps : CommonMethods
    {

    static string pageTitle;
    static List<IWebElement> elements;
    static Dictionary<string, string> UrlsAndBlogNumbers;

        //Scenario: Blog page is loading check.Verification
        [Given(@"I navigated to the main blog page _ first TC")]
        public void GivenINavigatedToTheMainBlogPage_FirstTC()
        {
            GoToUrl("http://www.sword-activerisk.com/news-resources/blog/");
        }

        [When(@"I search for a particular element on this page")]
        public void WhenISearchForAParticularElementOnThisPage()
        {
            var pageHeader = FindElement(By.XPath("//*[@id='sub-header']"));
            pageTitle = pageHeader.GetAttribute("innerText");
            pageTitle = pageTitle.TrimEnd('\r', '\n');
        }

        [Then(@"this element should be there")]
        public void ThenThisElementShouldBeThere()
        {
            DisposeDriver();
            Assert.AreEqual(pageTitle, "THE ACTIVE RISK BLOG");
        }

//Scenario: Check that the blog post by date table exists. Verification
        [Given(@"I navigated to the main blog page _ second TC")]
        public void GivenINavigatedToTheMainBlogPage_SecondTC()
        {
            GoToUrl("http://www.sword-activerisk.com/news-resources/blog/");
        }

        [When(@"I search for the list of blogs")]
        public void WhenISearchForTheListOfBlogs()
        {
            var listOfBlogs = FindElements(By.XPath("//*[@id='sidebar-blog']/div[3]/div/li[*]"));
            elements = listOfBlogs;
            Console.WriteLine("Number of blogs is : " + elements.Count);
        }

        [Then(@"The list of blogs is not empty")]
        public void ThenTheListOfBlogsIsNotEmpty()
        {
            DisposeDriver();
            Assert.AreNotEqual(0, elements.Count);
        }

//Scenario: Search the table and verify the result. Verification
        [Given(@"I navigated to the main blog page  _ third TC")]
        public void GivenINavigatedToTheMainBlogPage_ThirdTC()
        {
            GoToUrl("http://www.sword-activerisk.com/news-resources/blog/");
        }

        [When(@"I search the list of blogs and their number of blogs")]
        public void WhenISearchTheListOfBlogsAndTheirNumberOfBlogs()
        {
            UrlsAndBlogNumbers = GetUrlsAndBlogNumbers("//*[@id='sidebar-blog']/div[3]/div/li[*]");
        }
        
        [Then(@"Navigating to Each blog page shows the expected number of blogs")]
        public void ThenNavigatingToEachBlogPageShowsTheExpectedNumberOfBlogs()
        {
            VerifyEachBlogPage(UrlsAndBlogNumbers);
            DisposeDriver();
        }
    }
}
