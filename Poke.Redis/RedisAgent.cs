using StackExchange.Redis;

namespace Poke.Redis;

public class RedisAgent
{
    private readonly string _redisConnectionString = "localhost:6379";
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public RedisAgent()
    {
        _connection = new Lazy<ConnectionMultiplexer>(() =>
        {
            try
            {
                var connection = ConnectionMultiplexer.Connect(_redisConnectionString);
                return connection;
            }
            catch (Exception ex)
            {
                return null;
            }
        });
    }
    
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