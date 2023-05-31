using ClearCookiesEasily;
using FakeItEasy;
using NLog;
using ClearCookiesEasily.CookiesDB;

namespace UnitTest
{
    [TestClass]
    public class CookiesDbTest
    {
        [TestMethod]
        public void T01_CountCookies()
        {
            var logger = A.Fake<ILogger>();

            var db = new CookiesDbFactory().Create(logger);
            Assert.IsTrue(db.Init("Brave", @"C:\Users\admin\AppData\Local\BraveSoftware\Brave-Browser\User Data\Default\Network\Cookies"));

            var res = db.CountCookies(db.BuilderTimeRangeModifier(TimeRange.Range.Last7Days));
            Assert.IsTrue(res > 0);
        }
    }
}