using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCourse;

public class FirstTests
{
    private IWebDriver _driver = null!;

    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
    }

    [Test]
    public void LoginPage_HasCorrectHeader()
    {
        _driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

        var header = _driver.FindElement(By.TagName("h2"));

        Assert.That(header.Text, Is.EqualTo("Login Page"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();     // закрывает браузер и драйвер
        _driver.Dispose();
    }
}