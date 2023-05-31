using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearCookiesEasily.CookiesDB
{
    public interface ICookiesDb
    {
        
        string? BrowserName { get; }
        bool Init(string browserName, string connectionString);
        public bool IsInit { get; }
        long AllCookiesCount();

        Task<long> DeleteCookiesAsync(string range);
        Task<long> CountCookiesAsync(string range);

        long CountCookies(string range);


        string BuilderTimeRangeModifier(TimeRange.Range range);
    }


}
