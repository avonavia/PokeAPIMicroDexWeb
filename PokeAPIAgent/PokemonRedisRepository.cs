using System.Text.Json;
using Poke.Redis;
using PokeAPIAgent.Entities;
using Serilog;

namespace PokeAPIAgent;

public class PokemonRedisRepository
{
    private readonly RedisAgent _agent;
    private readonly ILogger _logger;
    private readonly int _totalCountPokemon = 151;
    private readonly int _totalCountAb = 307;

    public PokemonRedisRepository(RedisAgent agent, ILogger logger)
    {
        _agent = agent;
        _logger = logger;
    }

    public async Task StorePokemonCardAsync(PokemonCard card)
    {
        var json = JsonSerializer.Serialize(card);
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Adding [pokemon:{card.Id}] to Redis");
            await db.StringSetAsync($"pokemon:{card.Id}", json);
        }
        else
        {
            _logger.Warning("Redis is not available. Skipping adding data");
        }
    }

    private async Task<PokemonCard?> GetPokemonCardAsync(int id)
    {
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Trying to get [pokemon:{id}] from Redis");
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
    
    private async Task<AbilityCard?> GetAbilityCardAsync(int id)
    {
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Trying to get [ab:{id}] from Redis");
            var json = await db.StringGetAsync($"ab:{id}");
            if (json.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<AbilityCard>(json.ToString());
        }
        else
        {
            _logger.Warning("Redis is not available. Can't get data");
        }
        
        return null;
    }
    
    public async Task StoreAbilityCardAsync(AbilityCard card)
    {
        var json = JsonSerializer.Serialize(card);
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Adding [ab:{card.Id}] to Redis");
            await db.StringSetAsync($"ab:{card.Id}", json);
        }
        else
        {
            _logger.Warning("Redis is not available. Skipping adding data");
        }
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
        
        var cards = new List<PokemonCard>();

        for (int i = 0; i < _totalCountPokemon + 1; i++)
        {
            var card = await GetPokemonCardAsync(i);

            if (card != null)
                cards.Add(card);
        }
        
        _logger.Information($"Loaded [{cards.Count}] pokemon cards");
        return cards;
    }
    
    public async Task<List<AbilityCard>> GetAllAbilityCards()
    {
        _logger.Information("Getting all ability Cards from Redis...");
        
        var db = _agent.GetDatabase();
        if (db == null)
        {
            _logger.Warning("Redis is not available. Skipping");   
            return new List<AbilityCard>();
        }
        
        var cards = new List<AbilityCard>();

        for (int i = 0; i < _totalCountAb + 1; i++)
        {
            var card = await GetAbilityCardAsync(i);

            if (card != null)
                cards.Add(card);
        }
        
        _logger.Information($"Loaded [{cards.Count}] ability cards");
        return cards;
    }
}