using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCourse;

public class TablesTests
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;
    private const string TablesUrl = "https://practice.expandtesting.com/tables";

    [SetUp]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void CustomersTable_ShowsAllCustomersData()
    {
        OpenTablesPage();

        var table = CustomersTable("table1");
        var rows = table.FindElements(By.CssSelector("tbody tr")).Count();

        Assert.That(rows, Is.EqualTo(4));
    }

    [Test]
    public void CustomersTable_ShouldDisplayEmailForClient()
    {
        OpenTablesPage();

        var row = FindCustomerRow("table1", "Doe");
        var email = row.FindElement(By.XPath("./td[3]")).Text;

        Assert.That(email, Is.EqualTo("jdoe@hotmail.com"));
    }

    [Test]
    public void CustomersTable_ShouldDisplayCorrectAmountForClient()
    {
        OpenTablesPage();

        var row = FindCustomerRow("table2", "Bach");
        var due = row.FindElement(By.CssSelector("td.dues")).Text;

        Assert.That(due, Is.EqualTo("$51.00"));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    private void OpenTablesPage()
    {
        _driver.Navigate().GoToUrl(TablesUrl);
    }

    private IWebElement FindCustomerRow(string tableId, string lastName)
    {
        return _driver.FindElement(By.XPath($"//table[@id='{tableId}']//tr[td[text()='{lastName}']]"));
    }

    private IWebElement CustomersTable(string tableId)
    {
        return _driver.FindElement(By.XPath($"//table[@id='{tableId}']//tbody"));
    }
}
