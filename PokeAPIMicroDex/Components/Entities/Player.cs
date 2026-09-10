using PokeAPIAgent.Entities;

namespace PokeAPIMicroDex.Components.Entities;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PokemonCard Pokemon { get; set; }
    
    public MoveCard Move1 { get; set; }
    public MoveCard Move2 { get; set; }
    public MoveCard Move3 { get; set; }
    public MoveCard Move4 { get; set; }
    
    public int HP { get; set; }
}