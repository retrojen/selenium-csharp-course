using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse;

public class FirstTests
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;

    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void LoginPage_HasCorrectHeader()
    {
        _driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

        var header = _driver.FindElement(By.TagName("h2"));

        Assert.That(header.Text, Is.EqualTo("Login Page"));
    }

    [Test]
    public void Login_WithValidCredentials_ShowsSuccessMessage()
    {
        _driver.Navigate().GoToUrl("https://practice.expandtesting.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        usernameField.SendKeys("practice");
        passwordField.SendKeys("SuperSecretPassword!");
        loginButton.Click();

        var successfulLoginMessage = _wait.Until(d => d.FindElement(By.Id("flash")));

        Assert.That(successfulLoginMessage.Text, Is.EqualTo("You logged into a secure area!"));
    }

    [Test]
    public void Login_WithInvalidUsername_ShowsError()
    {
        _driver.Navigate().GoToUrl("https://practice.expandtesting.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        var nonExistentUser = $"user_{Guid.NewGuid()}";
        usernameField.SendKeys(nonExistentUser);
        passwordField.SendKeys("SuperSecretPassword!");
        loginButton.Click();

        var invalidLoginMessage = _wait.Until(d => d.FindElement(By.Id("flash")));

        Assert.That(invalidLoginMessage.Text, Is.EqualTo("Your username is invalid!"));
    }

    [Test]
    public void Login_WithInvalidPassword_ShowsError()
    {
        _driver.Navigate().GoToUrl("https://practice.expandtesting.com/login");

        var usernameField = _driver.FindElement(By.Id("username"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        var incorrectPassword = Guid.NewGuid().ToString();
        usernameField.SendKeys("practice");
        passwordField.SendKeys(incorrectPassword);
        loginButton.Click();

        var invalidPasswordMessage = _wait.Until(d => d.FindElement(By.Id("flash")));

        Assert.That(invalidPasswordMessage.Text, Is.EqualTo("Your password is invalid!"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();     // закрывает браузер и драйвер
        _driver.Dispose();
    }

}