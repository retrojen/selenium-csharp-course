using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;

namespace SeleniumCourse;

public class FirstTests
{
    private IWebDriver _driver = null!;

    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [Test]
    public void LoginPage_HasCorrectHeader()
    {
        _driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

        var header = _driver.FindElement(By.TagName("h2"));

        Assert.That(header.Text, Is.EqualTo("Login Page"));
    }

    [Test]
    public void LoginPage_HasSuccessfulLogin()
    {
        _driver.Navigate().GoToUrl("https://practice.expandtesting.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        usernameField.SendKeys("practice");
        passwordField.SendKeys("SuperSecretPassword!");
        loginButton.Click();

        var successfulLoginMessage = _driver.FindElement(By.Id("flash"));

        Assert.That(successfulLoginMessage.Text, Is.EqualTo("You logged into a secure area!"));

    }

    [Test]
    public void LoginPage_HasInvalidLogin()
    {
        _driver.Navigate().GoToUrl("https://practice.expandtesting.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        usernameField.SendKeys("p");
        passwordField.SendKeys("SuperSecretPassword!");
        loginButton.Click();

        var invalidLoginMessage = _driver.FindElement(By.Id("flash"));

        Assert.That(invalidLoginMessage.Text, Is.EqualTo("Your username is invalid!"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();     // закрывает браузер и драйвер
        _driver.Dispose();
    }

}