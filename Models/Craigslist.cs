using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JobSearchV2
{
  public class CraigslistClass
  {
    public string Title { get; set; }
    public string Url { get; set; }
    public string Location { get; set; }
    public string Date { get; set; }

    CraigslistClass(string title, string url, string location, string date)
    {
      Title = title;
      Url = url;
      Location = location;
      Date = date;
    }

    public static List<CraigslistClass> RunSearch(string jobName, string jobLocation)
    {
      List<CraigslistClass> craigslistJobs = new List<CraigslistClass> { };
      string tempTitle = "";
      string tempLink = "";
      string tempLocation = "";
      string tempDate = "";
      ChromeDriver driver = new ChromeDriver();
      try
      {
        driver.Navigate().GoToUrl("https://" + jobLocation + ".craigslist.org/d/jobs/search/jjj");
        var searchForm = driver.FindElementById("query");
        searchForm.SendKeys(jobName);
        searchForm.Submit();
        IList<IWebElement> anchors = driver.FindElements(By.ClassName("hdrlnk"));
        for (int i = 1; i < anchors.Count; i++)
        {
          tempTitle = anchors[i].Text;
          tempLink = anchors[i].GetAttribute("href");
          IWebElement location = driver.FindElement(By.XPath("//*[@id='sortable-results']/ul/li[" + i + "]/p/span[2]/span[1]"));
          tempLocation = location.Text;
          IWebElement date = driver.FindElement(By.XPath("//*[@id='sortable-results']/ul/li[" + i + "]/p/time"));
          tempDate = date.Text;
          CraigslistClass tempjob = new CraigslistClass(tempTitle, tempLink, tempLocation, tempDate);
          craigslistJobs.Add(tempjob);
        }
        // driver.Close();
        Console.WriteLine("JOBBBBSSS: " + craigslistJobs[0].Title + "LOCATED AT: " + craigslistJobs[0].Location);
        return craigslistJobs;
      }
      catch
      {
        CraigslistClass tempjob = new CraigslistClass("Oops, try again! Make sure you're entering a city, not a state or country", "We encountered an error", "", "");
        craigslistJobs.Add(tempjob);
        driver.Close();
        return craigslistJobs;
      }
    }
  }
}
