using System.Text;
using PokeAPIAgent.Entities;
using PokeApiNet;
using Polly;
using Polly.Retry;
using Serilog;

namespace PokeAPIAgent;

public class Connection
{
    private readonly PokeApiClient _pokeApiClient;
    private readonly ILogger _logger;
    private readonly Random _rand = new();
    public List<PokemonCard> PokemonList = new();
    public List<AbilityCard> AbilityList = new();
    public readonly int TotalCount = 151;
    public readonly int TotalCountAb = 307;
    private readonly AsyncRetryPolicy _retryPolicy;

    public Connection(PokeApiClient pokeApiClient, ILogger logger)
    {
        _pokeApiClient = pokeApiClient;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(_rand.Next(1, 3), attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.Warning(
                        $"Retry [{retryCount}] due to [{exception.Message}]. Waiting [{timeSpan}] before next attempt...");
                });
    }

    private async Task<Pokemon?> GetPokemonByNumber(int number)
    {
        Pokemon? pokemon = null;

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            pokemon = await _pokeApiClient.GetResourceAsync<Pokemon>(number, cts.Token);
            _logger.Information($"Got: [{pokemon.Name}]");
        });

        return pokemon;
    }

    private async Task<Ability?> GetAbilityById(int id)
    {
        Ability? ab = null;

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            ab = await _pokeApiClient.GetResourceAsync<Ability>(id, cts.Token);
            _logger.Information($"Got: [{ab.Name}]");
        });

        return ab;
    }

    public async IAsyncEnumerable<PokemonCard> GetPokemonStreamAsync(int starting)
    {
        if (starting == 0)
        {
            starting = 1;
        }

        for (int i = starting; i <= TotalCount; i++)
        {
            if (PokemonList.All(p => p.Id != i))
            {
                var p = await GetPokemonByNumber(i);

                if (p != null)
                {
                    StringBuilder sb = new();
                    var types = p.Types;
                    for (int j = 0; j < types.Count; j++)
                    {
                        sb.Append(types[j].Type.Name);
                        if (j < types.Count - 1)
                        {
                            sb.Append("/");
                        }
                    }

                    yield return new PokemonCard
                    {
                        Id = p.Id,
                        Name = await Capitalize(p.Name),
                        Type = sb.ToString(),
                        ImageUrl = p.Sprites.FrontDefault,
                        Weight = p.Weight,
                        ImageUrlBack = p.Sprites.BackDefault
                    };
                }
            }
        }
    }

    public async IAsyncEnumerable<AbilityCard> GetAbilityStreamAsync(int starting)
    {
        if (starting == 0)
        {
            starting = 1;
        }

        for (int i = starting; i <= TotalCountAb; i++)
        {
            if (AbilityList.All(p => p.Id != i))
            {
                var a = await GetAbilityById(i);
                
                if (a != null && a.EffectEntries.FirstOrDefault(e => e.Language.Name == "en") != null)
                    yield return new AbilityCard
                    {
                        Id = a.Id,
                        Name = await Capitalize(a.Name),
                        Effect = a.EffectEntries.FirstOrDefault(e => e.Language.Name == "en").ShortEffect
                    };
            }
        }
    }

    private Task<string> Capitalize(string word)
    {
        return Task.FromResult(word.First().ToString().ToUpper() + word.Substring(1));
    }
}