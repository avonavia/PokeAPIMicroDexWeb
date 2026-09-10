namespace PokeAPIAgent.Entities;

public class BattleLog
{
    public Guid Id { get; set; }
    public string WinnerName { get; set; }
    public string LoserName { get; set; }
    public PokemonCard WinningPokemon { get; set; }
    public PokemonCard LosingPokemon { get; set; }
    public int WinnerHPLeft { get; set; }
    public DateTime BattleDate { get; set; }
}