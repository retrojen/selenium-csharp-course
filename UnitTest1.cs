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
    [Ignore("Пока не нужен")]
    public void LoginPage_HasCorrectHeader()
    {
        OpenLoginPage();

        var header = _driver.FindElement(By.TagName("h2"));

        Assert.That(header.Text, Is.EqualTo("How to test the login page?"));
    }

    [Test]
    public void Login_WithValidCredentials_ShowsSuccessMessage()
    {
        OpenLoginPage();

        Login(ValidUsername, ValidPassword);

        Assert.That(GetFlashMessage(), Is.EqualTo("You logged into a secure area!"));
    }

    [Test]
    public void Login_WithInvalidUsername_ShowsError()
    {
        OpenLoginPage();

        var nonExistentUser = $"user_{Guid.NewGuid()}";
        Login(nonExistentUser, ValidPassword);        

        Assert.That(GetFlashMessage(), Is.EqualTo("Your username is invalid!"));
    }

    [Test]
    public void Login_WithInvalidPassword_ShowsError()
    {
        OpenLoginPage();

        var incorrectPassword = Guid.NewGuid().ToString();
        Login(ValidUsername, incorrectPassword);

        Assert.That(GetFlashMessage(), Is.EqualTo("Your password is invalid!"));
    }

    [Test]
    public void Login_EmptyFields_ShowsError()
    {
        OpenLoginPage();

        Login("", "");

        Assert.That(GetFlashMessage(), Is.EqualTo("Your username is invalid!"));
    }

    [Test]
    public void Logout_HasCorrect()
    {
        OpenLoginPage();

        SuccessfulLogin();

        var LogoutButton = _driver.FindElement(By.CssSelector("a[href='/logout']"));

        LogoutButton.Click();

        Assert.That(GetFlashMessage(),Is.EqualTo("You logged out of the secure area!"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    private void OpenLoginPage()
    {
        _driver.Navigate().GoToUrl(LoginUrl);
    }

    private void Login(string username, string password)
    {
        _driver.FindElement(By.Id("username")).SendKeys(username);
        _driver.FindElement(By.Id("password")).SendKeys(password);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
    }

    private void SuccessfulLogin()
    {
        _driver.FindElement(By.Id("username")).SendKeys(ValidUsername);
        _driver.FindElement(By.Id("password")).SendKeys(ValidPassword);
        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        _driver.FindElement(By.CssSelector("a[href='/logout']"));
    }

    private string GetFlashMessage()
    {
        var flash = _wait.Until(d => d.FindElement(By.Id("flash")));
        return flash.Text;
    }

    private const string LoginUrl = "https://practice.expandtesting.com/login";
    private const string ValidUsername = "practice";
    private const string ValidPassword = "SuperSecretPassword!";

}