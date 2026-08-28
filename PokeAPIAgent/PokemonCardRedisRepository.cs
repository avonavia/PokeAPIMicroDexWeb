using System.Text.Json;
using Poke.Redis;
using PokeAPIAgent.Entities;
using Serilog;

namespace PokeAPIAgent;

public class PokemonCardRedisRepository
{
    private readonly RedisAgent _agent;
    private readonly ILogger _logger;

    public PokemonCardRedisRepository(RedisAgent agent, ILogger logger)
    {
        _agent = agent;
        _logger = logger;
    }

    public async Task StorePokemonCardAsync(PokemonCard card)
    {
        var json = JsonSerializer.Serialize(card);
        var db = _agent.GetDatabase();
        if (db != null)
            await db.StringSetAsync($"pokemon:{card.Id}", json);
        else
        {
            _logger.Warning("Redis is not available. Skipping adding data");
        }
    }

    public async Task<PokemonCard?> GetPokemonCardAsync(int id)
    {
        var db = _agent.GetDatabase();
        if (db != null)
        {
            var json = await db.StringGetAsync($"pokemon:{id}");
            if (json.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<PokemonCard>(json.ToString());
        }
        else
        {
            _logger.Warning("Redis is not available. Can't get data");
        }
        
        return null;
    }

    public async Task<List<PokemonCard>> GetAllPokemonCards()
    {
        _logger.Information("Getting all pokemon Cards from Redis...");
        
        var db = _agent.GetDatabase();
        if (db == null)
        {
            _logger.Warning("Redis is not available. Skipping");   
            return new List<PokemonCard>();
        }
        
        var total = 151;
        var cards = new List<PokemonCard>();

        for (int i = 0; i < total + 1; i++)
        {
            var card = await GetPokemonCardAsync(i);

            if (card != null)
                cards.Add(card);
        }
        
        _logger.Information($"Loaded [{cards.Count}] pokemon cards");
        return cards;
    }
}