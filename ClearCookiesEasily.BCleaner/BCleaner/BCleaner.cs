using System.Text;
using System.Text.RegularExpressions;
using ClearCookiesEasily.CookiesDB;
using ClearCookiesEasily.PltBrowser;
using MintPlayer.PlatformBrowser;
using NLog;

namespace ClearCookiesEasily.BCleaner
{
    internal class BCleaner : IBCleaner
    {
        private readonly ILogger _logger;

        private readonly IPltBrowser _pltBrowser;
        private readonly List<ExtBrowser> _browserList = new(new List<ExtBrowser>());
        private readonly ICookiesDbFactory _cookiesDbFactory;
        //private readonly Dictionary<string,List<ICookiesDb>> _cookiesDb = new();

        public BCleaner(IPltBrowser pltBrowser, ICookiesDbFactory cookiesDbFactory, ILogger logger)
        {
            _logger = logger;
            _pltBrowser = pltBrowser;
            _cookiesDbFactory = cookiesDbFactory;
        }

        public bool Init()
        {
            try
            {
                var path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
                    .FullName;
                var cookiesFiles = Directory.GetFiles(path, "Cookies", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true });
                if (!cookiesFiles.Any())
                {
                    _logger.Error("Сookies were not found");
                    return false;
                }

                var browsers = _pltBrowser.GetInstalledBrowsers();
                if (!browsers.Any())
                {
                    _logger.Error("Installed browsers were not found");
                    return false;
                }

                foreach (var browser in browsers)
                {
                    var cookiesList = ExtractCookiesList(browser, cookiesFiles);
                    if (cookiesList.Any() != true) continue;

                    var cookiesDbs = cookiesList.Select(x => 
                    {
                        var db = _cookiesDbFactory.Create(_logger);
                        db.Init(browser.Name, x);
                        return db;
                    });

                    _browserList.Add(new ExtBrowser
                    {
                        Browser = browser,
                        CookiesPath = cookiesList,
                        CookiesDbs = cookiesDbs.Where(x => x.IsInit).ToList()
                    });
                }

                return _browserList.Any();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        private static List<string> ExtractCookiesList(Browser browser, IEnumerable<string> cookiesFiles)
        {
            var pattern = browser.Name.Split(" ").Select(y => @$"(?=.*\b{y}\b)").Aggregate((p, c) => $"{p}{c}");
            //.Select(z => $"^{z}.*$"

            var cookiesList = cookiesFiles.Where(x => Regex.Match(x, pattern, RegexOptions.IgnoreCase).Success).Where(IsSqLiteDatabase).ToList();
            return cookiesList;
        }

        private static bool IsSqLiteDatabase(string pathToFile)
        {
            var result = false;

            if (!File.Exists(pathToFile)) return result;

            using var stream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var header = new byte[16];

            for (var i = 0; i < 16; i++)
            {
                header[i] = (byte)stream.ReadByte();
            }

            result = Encoding.UTF8.GetString(header).Contains("SQLite format 3");

            stream.Close();

            return result;
        }

        public IEnumerable<Browser> GetValidBrowsers()
        {
            try
            {
                if (!_browserList.Any()) Init();
                return _browserList.Select(x => x.Browser)!;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return null!;
            }
        }

        public Browser GetDefaultBrowser()
        {
            return _pltBrowser.GetDefaultBrowser(_browserList.Select(x => x.Browser)!);
        }

        public async Task<long> CountCookies(string browserName, TimeRange.Range range)
        {
            long result = 0;
            var browser = _browserList.FirstOrDefault(x => x.Browser?.Name == browserName);

            if (browser?.CookiesDbs == null) return result;
            
            foreach (var db in browser.CookiesDbs)
            {
                result += await db.CountCookiesAsync(db.BuilderTimeRangeModifier(range));
            }

            return result;
        }

        public long AllCookiesCount(string browserName)
        {
            long result = 0;
            var browser = _browserList.FirstOrDefault(x => x.Browser?.Name == browserName);

            return browser?.CookiesDbs.Sum(db => db.AllCookiesCount()) ?? result;
        }

        public async Task<long> DeleteCookiesAsync(string browserName, TimeRange.Range range)
        {
            try
            {
                long result = 0;
                var browser = _browserList.FirstOrDefault(x => x.Browser?.Name == browserName);

                if (browser?.CookiesDbs == null) return result;

                foreach (var db in browser.CookiesDbs)
                {
                    result += await db.DeleteCookiesAsync(db.BuilderTimeRangeModifier(range));
                }

                return result;
            }
            catch (Exception e)
            {

                _logger.Error(e);
                return 0;
            }
        }

    }
}
