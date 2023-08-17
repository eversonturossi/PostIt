namespace PostIt.Services;

public interface ICheckDBService
{
    Task Check();
}

public class CheckDBService : ICheckDBService
{
    public readonly ISQLiteConnectionFactory _connectionFactory;
    public CheckDBService(ISQLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Check()
    {
        var sql = @"CREATE TABLE IF NOT EXISTS Notes (
                    ID INTEGER PRIMARY KEY,
                    User INTEGER NOT NULL,
                    Message TEXT NOT NULL,
                    CreatedAt DATETIME NOT NULL,
                    ReadAt DATETIME,
                    UserSent INTEGER NOT NULL);";
        _connectionFactory.Database = "PostIt.db";
        await _connectionFactory.ExecuteAsync(sql, null, null);
    }
}