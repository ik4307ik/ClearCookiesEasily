using System.Text;
using ClearCookiesEasily.BCleaner;
using ClearCookiesEasily.CookiesDB;
using MintPlayer.IconUtils;
using MintPlayer.PlatformBrowser;
using NLog;

namespace ClearCookiesEasily
{
    public partial class Form1 : Form
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IBCleaner _bCleaner;
        private List<Browser> _browsers;
        private Browser _defaultBrowser;

        public Form1()
        {
            InitializeComponent();
            _bCleaner = BCleanerFactory.Create(new CookiesDbFactory(), _logger);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                _logger.Debug("Form1_Load");
                comboBoxTimeRange.DataSource = new BindingSource(TimeRange.TimeRangeCollection, null);
                comboBoxTimeRange.DisplayMember = "Value";
                comboBoxTimeRange.ValueMember = "Key";

                // Suspend the drawing of the listview
                lvBrowsers.SuspendLayout();

                // Remove all items from the listview
                lvBrowsers.Items.Clear();

                // Assign a new imagelist
                lvBrowsers.LargeImageList = new ImageList
                {
                    ImageSize = new Size(60, 60),
                    ColorDepth = ColorDepth.Depth32Bit
                };

                if (!_bCleaner.Init())
                {
                    _logger.Error("_bCleaner.Init() false");
                    return;
                }

                // Get all browsers on the system
                var validBrowsers = _bCleaner.GetValidBrowsers().Where(x => true).ToList();

                _browsers = validBrowsers;

                foreach (var browser in _browsers.Select((br, i) => new { Browser = br, Index = i }))
                {
                    var icon = IconExtractor.Split(browser.Browser.IconPath)[browser.Browser is { IconIndex: < 0 } ? 0 : browser.Browser.IconIndex];
                    var icons = IconExtractor.ExtractImagesFromIcon(icon);
                    var largestSize = icons.Max(x => x.Width);
                    var largestIcon = icons.LastOrDefault(x => x.Width == largestSize);
                    if (largestIcon != null) lvBrowsers.LargeImageList.Images.Add(largestIcon);

                    lvBrowsers.Items.Add(new ListViewItem
                    {
                        Text = browser.Browser.Name,
                        Tag = browser.Browser.ExecutablePath.Trim('\"'),
                        ImageIndex = browser.Index,
                    });
                }

                // Get default browser
                _defaultBrowser = _bCleaner.GetDefaultBrowser();

                // Select default browser
                if (!_browsers.Contains(_defaultBrowser))
                {
                    _logger.Error($"list of _browsers does not contains _defaultBrowser {_defaultBrowser.Name}");
                    return;
                }


                //var sss = _browsers.FirstOrDefault(browser => browser.ExecutablePath == _defaultBrowser.ExecutablePath);
                var defaultBrowserListItem = lvBrowsers.Items[_browsers.IndexOf(_defaultBrowser)];
                defaultBrowserListItem.Checked = true;
                defaultBrowserListItem.Focused = defaultBrowserListItem.Selected = true;
            }
            finally
            {
                lvBrowsers.ResumeLayout();
            }
        }

        private async void BtnClear_Click(object sender, EventArgs e)
        {
            var resultMessage = new StringBuilder();
            foreach (var item in lvBrowsers.CheckedItems.Cast<ListViewItem>())
            {
                var browser = _browsers.FirstOrDefault(x => x.Name == item.Text);
                var range = (TimeRange.Range)(comboBoxTimeRange.SelectedValue ?? TimeRange.Range.LastHour);
                if (browser?.Name != null)
                {
                    var deletedCount = await _bCleaner.DeleteCookiesAsync(browser.Name, range);
                    resultMessage.AppendLine($"{browser.Name} - {deletedCount} cookies");
                }

            }
            MessageBox.Show(resultMessage.ToString(), @"Result", MessageBoxButtons.OK);
        }
    }
}