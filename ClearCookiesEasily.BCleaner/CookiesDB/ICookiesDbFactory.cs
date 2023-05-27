using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearCookiesEasily.CookiesDB
{
    internal interface ICookiesDbFactory
    {
        ICookiesDb Create(ILogger logger);
    }
}
