using NLog;
using System.Data;
using System.Data.SQLite;

namespace ClearCookiesEasily.CookiesDB.SQLite
{
    internal class SqLiteDb : ICookiesDb
    {
        private const string CookiesTable = "cookies";

        private readonly ILogger _logger;
        private string? _connectionString; 

        public bool IsInit { get; private set; }
        public string? BrowserName { get; private set; }

        public SqLiteDb(ILogger logger)
        {
            _logger = logger;
        }

        public bool Init(string browserName, string connectionString)
        {
            BrowserName = browserName;
            
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = connectionString
            };
            _connectionString = builder.ToString();

            using (var sqlConn = new SQLiteConnection(_connectionString))
            {
                sqlConn.Open();
                if ((sqlConn.State & ConnectionState.Open) != 0)
                {
                    _logger.Debug($"The connection is successfully connected. {connectionString}");
                    IsInit = true;
                    return true;
                }
            }

            _logger.Error($"The connection is not successfully connected. {connectionString}");
            IsInit = false;
            return false;
        }

        public long AllCookiesCount()
        {
            try
            {
                using var sqlConn = new SQLiteConnection(_connectionString);
                sqlConn.Open();
                var command = new SQLiteCommand($"SELECT COUNT(*) FROM {CookiesTable}", sqlConn);
                return command.ExecuteScalar() is long longRes ? longRes : 0;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return 0;
            }
        }

        public string BuilderTimeRangeModifier(TimeRange.Range range)
        {
            return range switch
            {
                TimeRange.Range.LastHour => "-1 hours",
                TimeRange.Range.Last24Hours => "-24 hours",
                TimeRange.Range.Last7Days => "-7 days",
                TimeRange.Range.Last4Week => "-28 days",
                TimeRange.Range.AllTime => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(range), range, null)
            };
        }

        public async Task<long> CountCookiesAsync(string range)
        {
            try
            {
                var sQLiteCommand = $"SELECT COUNT(*) FROM {CookiesTable}";
                if (!string.IsNullOrEmpty(range))
                {
                    sQLiteCommand += " WHERE [creation_utc] > (((strftime('%s', CURRENT_TIMESTAMP, @value)) + (strftime('%s', '1601-01-01') * -1)) * 1000000)";
                }

                await using var sqlConn = new SQLiteConnection(_connectionString);
                
                sqlConn.Open();

                var command = new SQLiteCommand(sQLiteCommand, sqlConn);
                if (!string.IsNullOrEmpty(range)) command.Parameters.Add(new SQLiteParameter("@value", range));

                var taskResult = await command.ExecuteScalarAsync();

                var result = taskResult is long longRes ? longRes : 0;

                _logger.Info($"CountCookiesAsync range => {result};  {_connectionString}");

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return 0;
            }
        }

        public long CountCookies(string range)
        {
            try
            {
                //var query = $@"(((strftime('%s', CURRENT_TIMESTAMP, '@value')) + (strftime('%s', '1601-01-01') * -1)) * 1000000)";

                var sQLiteCommand = $"SELECT COUNT(*) FROM {CookiesTable}";
                if (!string.IsNullOrEmpty(range))
                {
                    sQLiteCommand += " WHERE [creation_utc] > (((strftime('%s', CURRENT_TIMESTAMP, @value)) + (strftime('%s', '1601-01-01') * -1)) * 1000000)";
                }

                using var sqlConn = new SQLiteConnection(_connectionString);

                sqlConn.Open();

                var command = new SQLiteCommand(sQLiteCommand, sqlConn);
                if (!string.IsNullOrEmpty(range)) command.Parameters.Add(new SQLiteParameter("@value", range));

                var taskResult = command.ExecuteScalar();

                var result = taskResult is long longRes ? longRes : 0;

                _logger.Info($"CountCookiesAsync range => {result};  {_connectionString}");

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return 0;
            }
        }


        public async Task<long> DeleteCookiesAsync(string range)
        {
            try
            {
                var countBefore = await CountCookiesAsync(range);
                
                if (countBefore == 0)
                {
                    _logger.Warn($"DeleteCookiesAsync - counter before = 0. Nothing to delete. {_connectionString}");
                    return 0;
                }
                
                var sQLiteCommand = $"DELETE FROM {CookiesTable} ";
                if (!string.IsNullOrEmpty(range)) sQLiteCommand += " WHERE creation_utc > (((strftime('%s', CURRENT_TIMESTAMP, @value)) + (strftime('%s', '1601-01-01') * -1)) * 1000000)";

                await using (var sqlConn = new SQLiteConnection(_connectionString))
                {
                    sqlConn.Open();
                    var command = new SQLiteCommand(sQLiteCommand, sqlConn);
                    if (!string.IsNullOrEmpty(range)) command.Parameters.Add(new SQLiteParameter("@value", range));

                    // Execute the command.
                    await command.ExecuteNonQueryAsync();
                }

                var countAfter = await CountCookiesAsync(range);
                if (countAfter <= 0) return countBefore;
                
                _logger.Warn($"DeleteCookiesAsync - counter after = {countAfter}. Delete operation faild.  {_connectionString}");
                return 0;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return 0;
            }
        }
    }
}
