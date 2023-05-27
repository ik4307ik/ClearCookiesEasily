using System.Collections.ObjectModel;
using MintPlayer.PlatformBrowser;
using MintPlayer.PlatformBrowser.Enums;

namespace ClearCookiesEasily.PltBrowser
{
    internal interface IPltBrowser
    {
        ReadOnlyCollection<Browser> GetInstalledBrowsers();
        Browser GetDefaultBrowser(IEnumerable<Browser> browsers);
    }
}
