using StackExchange.Redis;

namespace Poke.Redis;

public class RedisAgent
{
    private readonly string _redisConnectionString = "localhost:6379";
    private readonly Lazy<ConnectionMultiplexer> _connection;
    private bool _isConnected;

    public RedisAgent()
    {
        _connection = new Lazy<ConnectionMultiplexer>(() =>
        {
            try
            {
                var connection = ConnectionMultiplexer.Connect(_redisConnectionString);
                _isConnected = connection.IsConnected;
                return connection;
            }
            catch (Exception ex)
            {
                // Log the error
                _isConnected = false;
                // For debugging, you might want to log the exception
                // e.g., Serilog.Log.Warning("Redis connection failed: {Message}", ex.Message);
                return null;
            }
        });
    }

    public bool IsConnected => _isConnected;

    public IDatabase? GetDatabase()
    {
        if (_connection.Value != null && _connection.Value.IsConnected)
        {
            return _connection.Value.GetDatabase();
        }
        return null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection.IsValueCreated && _connection.Value != null)
        {
            await _connection.Value.DisposeAsync();
        }
    }
}