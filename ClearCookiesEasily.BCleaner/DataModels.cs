using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCookiesEasily.CookiesDB;
using MintPlayer.PlatformBrowser;

namespace ClearCookiesEasily
{
    public class ExtBrowser
    {
        public Browser? Browser { get; set; }
        public List<string> CookiesPath { get; set; }
        public List<ICookiesDb> CookiesDbs { get; set; }
    }


    public class TimeRange
    {
        public enum Range
        {
            LastHour,
            Last24Hours,
            Last7Days,
            Last4Week,
            AllTime

        }
        public static readonly SortedDictionary<Range, string> TimeRangeCollection = new()
        {
            { Range.LastHour, "Last hour"},
            { Range.Last24Hours, "Last 24 hours"},
            { Range.Last7Days, "Last 7 days"},
            { Range.Last4Week, "Last 4 week" },
            { Range.AllTime, "All time" }
        };

    }
}
