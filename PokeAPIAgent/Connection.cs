using System.Text;
using PokeAPIAgent.Entities;
using PokeApiNet;
using Serilog;

namespace PokeAPIAgent;

public class Connection
{
    private readonly PokeApiClient _pokeApiClient;
    private readonly ILogger _logger;
    private readonly Random _rand = new();
    public List<PokemonCard> PokemonList = new();
    public int TotalCount = 151;

    public Connection(PokeApiClient pokeApiClient, ILogger logger)
    {
        _pokeApiClient = pokeApiClient;
        _logger = logger;
    }

    private async Task<Pokemon> GetPokemonByNumber(int number)
    {
        Pokemon pokemon = await _pokeApiClient.GetResourceAsync<Pokemon>(number);
        
        _logger.Information($"Got: {pokemon.Name}");
        
        return pokemon;
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
            
                var secondsToWait = _rand.Next(1, 3);
                _logger.Information($"Waiting for [{secondsToWait}] seconds...");
                await Task.Delay(TimeSpan.FromSeconds(secondsToWait));

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

    private Task<string> Capitalize(string word)
    {
       return Task.FromResult(word.First().ToString().ToUpper() + word.Substring(1));
    }
}