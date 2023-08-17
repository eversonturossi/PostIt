using System.Data;
using System.Data.SQLite;

namespace PostIt.Connections;

public interface ISQLiteConnectionFactory : IDisposable
{
    string Database { get; set; }
    bool ReadOnly { get; set; }

    Task<T> GetAsync<T>(int id) where T : class;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

    Task<int> InsertAsync<T>(T obj) where T : class;
    Task<int> InsertAsync<T>(IEnumerable<T> objectList);

    Task<bool> UpdateAsync<T>(T obj) where T : class;
    Task<bool> UpdateAsync<T>(IEnumerable<T> objectList) where T : class;

    Task<bool> DeleteAsync<T>(T obj) where T : class;
    Task<bool> DeleteAsync<T>(IEnumerable<T> objectList) where T : class;

    Task<T> QueryFirstOrDefaultAsync<T>(string sql);
    Task<IEnumerable<T>> QueryAsync<T>(string sql);

    Task ExecuteAsync(string sql, object? param, IDbTransaction? transaction);
    Task<object> ExecuteScalarAsync(string sql, object? param, IDbTransaction? transaction);
    Task ExecuteCommandAsync(string sql, SQLiteTransaction? transaction);

    T QueryFirstOrDefault<T>(string sql);
    IEnumerable<T> Query<T>(string sql);

    void ClearPool();

    Task VaccumAsync();
}

public sealed class SQLiteConnectionFactory : ISQLiteConnectionFactory
{
    private readonly SQLiteConnection _connection;

    public string Database { get; set; } = string.Empty;
    public bool ReadOnly { get; set; } = true;

    public SQLiteConnectionFactory()
    {
        _connection = new SQLiteConnection();
        _connection.StateChange += new StateChangeEventHandler(StateChangeHandler);
    }

    private static void StateChangeHandler(object mySender, StateChangeEventArgs myEvent) { }

    public void ClearPool() => SQLiteConnection.ClearPool(_connection);

    private void Connect()
    {
        _connection.ConnectionString = SQLiteConnectionString.Get(Database, ReadOnly);
        _connection.Open();
    }

    private async Task ConnectAsync()
    {
        _connection.ConnectionString = SQLiteConnectionString.Get(Database, ReadOnly);
        await _connection.OpenAsync();
    }

    private void Disconnect()
    {
        try
        {
            _connection.Close();
        }
        finally
        {
            ClearPool();
        }
    }

    private async Task DisconnectAsync()
    {
        try
        {
            await _connection.CloseAsync();
        }
        finally
        {
            ClearPool();
        }
    }

    public async Task VaccumAsync()
    {
        try
        {
            await ConnectAsync();
            await _connection.ExecuteScalarAsync("VACUUM");
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<T> GetAsync<T>(int id) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.GetAsync<T>(id);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.GetAllAsync<T>();
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<int> InsertAsync<T>(T obj) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.InsertAsync<T>(obj);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<int> InsertAsync<T>(IEnumerable<T> objectList)
    {
        await ConnectAsync();
        try
        {
            return await _connection.InsertAsync<T[]>(objectList.ToArray<T>());
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<bool> UpdateAsync<T>(T obj) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.UpdateAsync<T>(obj);
        }

        finally
        {
            await DisconnectAsync();
        }

    }

    public async Task<bool> UpdateAsync<T>(IEnumerable<T> objectList) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.UpdateAsync<T[]>(objectList.ToArray<T>());
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<bool> DeleteAsync<T>(T obj) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.DeleteAsync<T>(obj);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<bool> DeleteAsync<T>(IEnumerable<T> objectList) where T : class
    {
        await ConnectAsync();
        try
        {
            return await _connection.DeleteAsync<T[]>(objectList.ToArray<T>());
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql)
    {
        await ConnectAsync();
        try
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
    {
        await ConnectAsync();
        try
        {
            return await _connection.QueryAsync<T>(sql);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public T QueryFirstOrDefault<T>(string sql)
    {
        Connect();
        try
        {
            return _connection.QueryFirstOrDefault<T>(sql);
        }
        finally
        {
            Disconnect();
        }
    }

    public IEnumerable<T> Query<T>(string sql)
    {
        Connect();
        try
        {
            return _connection.Query<T>(sql);
        }
        finally
        {
            Disconnect();
        }
    }

    public async Task ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        await ConnectAsync();
        try
        {
            await _connection.ExecuteAsync(sql, param, transaction);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task<object> ExecuteScalarAsync(string sql, object? param = null, IDbTransaction? transaction = null)
    {
        await ConnectAsync();
        try
        {
            return await _connection.ExecuteScalarAsync(sql, param, transaction);
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public async Task ExecuteCommandAsync(string sql, SQLiteTransaction? transaction = null)
    {
        await ConnectAsync();
        try
        {
            using var command = new SQLiteCommand(sql, _connection, transaction);
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await DisconnectAsync();
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        return await _connection.BeginTransactionAsync();
    }
}