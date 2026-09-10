using System.Text.Json;
using Poke.Redis;
using PokeAPIAgent.Entities;
using Serilog;
using StackExchange.Redis;

namespace PokeAPIAgent;

public class PokemonRedisRepository
{
    private readonly RedisAgent _agent;
    private readonly ILogger _logger;
    private readonly int _totalCountPokemon = 151;
    private readonly int _totalCountAb = 307;
    private readonly int _totalCountMove = 165;

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
            _logger.Warning($"Redis is not available. Skipping adding pokemon [{card.Id}]");
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
            _logger.Warning($"Redis is not available. Can't get pokemon [{id}]");
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
            _logger.Warning($"Redis is not available. Skipping adding ability [{card.Id}]");
        }
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
            _logger.Warning($"Redis is not available. Can't get ability [{id}]");
        }
        
        return null;
    }
    
    public async Task StoreMoveCardAsync(MoveCard card)
    {
        var json = JsonSerializer.Serialize(card);
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Adding [move:{card.Id}] to Redis");
            await db.StringSetAsync($"move:{card.Id}", json);
        }
        else
        {
            _logger.Warning($"Redis is not available. Skipping adding move [{card.Id}]");
        }
    }
    
    private async Task<MoveCard?> GetMoveCardAsync(int id)
    {
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Trying to get [move:{id}] from Redis");
            var json = await db.StringGetAsync($"move:{id}");
            if (json.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<MoveCard>(json.ToString());
        }
        else
        {
            _logger.Warning($"Redis is not available. Can't get move [{id}]");
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
    
    public async Task<List<MoveCard>> GetAllMoveCards()
    {
        _logger.Information("Getting all move Cards from Redis...");
        
        var db = _agent.GetDatabase();
        if (db == null)
        {
            _logger.Warning("Redis is not available. Skipping");   
            return new List<MoveCard>();
        }
        
        var cards = new List<MoveCard>();

        for (int i = 0; i < _totalCountMove + 1; i++)
        {
            var card = await GetMoveCardAsync(i);

            if (card != null)
                cards.Add(card);
        }
        
        _logger.Information($"Loaded [{cards.Count}] move cards");
        return cards;
    }
    
    private async Task<BattleLog?> GetBattleLogAsync(RedisKey key)
    {
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Trying to get [{key}] from Redis");
            var json = await db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<BattleLog>(json.ToString());
        }
        else
        {
            _logger.Warning($"Redis is not available. Can't get log [{key}]");
        }
        
        return null;
    }
    
    public async Task StoreBattleLogAsync(BattleLog log)
    {
        var json = JsonSerializer.Serialize(log);
        var db = _agent.GetDatabase();
        if (db != null)
        {
            _logger.Information($"Adding [log:{log.Id}] to Redis");
            await db.StringSetAsync($"log:{log.Id}", json);
        }
        else
        {
            _logger.Warning($"Redis is not available. Skipping adding log [{log.Id}]");
        }
    }
    
    public async Task<List<BattleLog>> GetAllBattleLogs()
    {
        _logger.Information("Getting all Battle Logs from Redis...");
        
        var db = _agent.GetDatabase();
        if (db == null)
        {
            _logger.Warning("Redis is not available. Skipping");   
            return new List<BattleLog>();
        }
        
        var logs = new List<BattleLog>();
        
        var keys = _agent.GetKeys();

        if (keys != null && keys.Any())
        {
            var logKeys = keys.Where(k => k.ToString().Contains("log")).ToList();
            
            foreach (var key in logKeys)
            {
                var log = await GetBattleLogAsync(key);

                if (log != null)
                    logs.Add(log);
            }
            
            _logger.Information($"Loaded [{logs.Count}] Battle Logs from Redis");
        }
        else
        {
            _logger.Information("Redis is empty");
        }

        return logs;
    }
}