using System.Collections.ObjectModel;
using MintPlayer.PlatformBrowser;
using MintPlayer.PlatformBrowser.Enums;

namespace ClearCookiesEasily.PltBrowser
{
    internal class PltBrowser : IPltBrowser
    {
        private ReadOnlyCollection<Browser>? _browsers;

        public ReadOnlyCollection<Browser> GetInstalledBrowsers()
        {
            return _browsers ??= PlatformBrowser.GetInstalledBrowsers();
        }

        public Browser GetDefaultBrowser(IEnumerable<Browser?> browsers)
        {
            return PlatformBrowser.GetDefaultBrowser(browsers, eProtocolType.Http);
        }
    }
}
