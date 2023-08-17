using System.Data.SQLite;

namespace PostIt.Connections;
public static class SQLiteConnectionString
{
    public static string Get(string database, bool readOnly = false)
    {
        var connectionString = new SQLiteConnectionStringBuilder
        {
            Version = 3,
            DataSource = database,
            Pooling = false,
            DefaultTimeout = 5000,
            SyncMode = SynchronizationModes.Off,
            JournalMode = SQLiteJournalModeEnum.Default,
            PageSize = 4096,
            CacheSize = 16777216,
            FailIfMissing = false,
            ReadOnly = readOnly
        };

        return connectionString.ToString();
    }
}