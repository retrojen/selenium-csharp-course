using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse;

public class FirstTests
{
    private const string LoginUrl = "https://practice.expandtesting.com/login";
    private const string ValidUsername = "practice";
    private const string ValidPassword = "SuperSecretPassword!";

    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;

    private static readonly By UsernameField = By.Id("username");
    private static readonly By PasswordField = By.Id("password");
    private static readonly By LogoutButton = By.CssSelector("a[href='/logout']");
    private static readonly By LoginButton = By.CssSelector("button[type='submit']");

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
        OpenLoginPage();

        var header = _driver.FindElement(By.TagName("h1"));

        Assert.That(header.Text, Is.EqualTo("Test Login page for Automation Testing Practice"));
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
    public void Login_WithEmptyFields_ShowsError()
    {
        OpenLoginPage();

        Login("", "");

        Assert.That(GetFlashMessage(), Is.EqualTo("Your username is invalid!"));
    }

    [Test]
    public void Logout_AfterLogin_ShowsLogoutMessage()
    {
        OpenLoginPage();

        LoginAsValidUser();

        Logout();

        Assert.That(GetFlashMessage(), Is.EqualTo("You logged out of the secure area!"));
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
        _driver.FindElement(UsernameField).SendKeys(username);
        _driver.FindElement(PasswordField).SendKeys(password);
        _driver.FindElement(LoginButton).Click();
    }

    private void LoginAsValidUser()
    {
        Login(ValidUsername, ValidPassword);
        _wait.Until(d => d.FindElement(LogoutButton));
    }

    private void Logout()
    {
       _driver.FindElement(LogoutButton).Click();
       _wait.Until(d => d.FindElement(LoginButton));
    }

    private string GetFlashMessage()
    {
        var flash = _wait.Until(d => d.FindElement(By.Id("flash")));
        return flash.Text;
    }
}