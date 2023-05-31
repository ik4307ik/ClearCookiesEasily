using MintPlayer.PlatformBrowser;

namespace ClearCookiesEasily.BCleaner
{
    public interface IBCleaner
    {
        bool Init();
        IEnumerable<Browser> GetValidBrowsers();
        Browser GetDefaultBrowser();
        long AllCookiesCount(string browserName);
        Task<long> CountCookies(string browserName, TimeRange.Range range);

        Task<long> DeleteCookiesAsync(string browserName, TimeRange.Range range);
    }
}