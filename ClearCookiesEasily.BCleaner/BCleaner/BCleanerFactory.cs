using ClearCookiesEasily.CookiesDB;
using ClearCookiesEasily.PltBrowser;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearCookiesEasily.BCleaner
{
    internal static class BCleanerFactory
    {
        public static IBCleaner Create(ICookiesDbFactory cookiesDbFactory, ILogger logger)
        {
            return new BCleaner(PltBrowserFactory.Create(), cookiesDbFactory, logger);
        }
    }
}
