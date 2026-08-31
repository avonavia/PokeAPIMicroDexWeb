namespace PokeAPIAgent.Entities;

public class PokemonCard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string ImageUrl { get; set; }
    
    public int Weight { get; set; }
    public string ImageUrlBack { get; set; }
    
    public bool IsHovered { get; set; }
}