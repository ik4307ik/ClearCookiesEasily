using ClearCookiesEasily.BCleaner;
using ClearCookiesEasily.CookiesDB.SQLite;
using ClearCookiesEasily.PltBrowser;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearCookiesEasily.CookiesDB
{
    public class CookiesDbFactory : ICookiesDbFactory
    {
        public ICookiesDb Create(ILogger logger) 
        {
            return new SqLiteDb(logger);
        }
    }
}
