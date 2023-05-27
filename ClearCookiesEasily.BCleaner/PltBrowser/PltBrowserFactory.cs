using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearCookiesEasily.PltBrowser
{
    internal class PltBrowserFactory
    {
        public static IPltBrowser Create()
        {
            return new PltBrowser();
        }
    }
}
