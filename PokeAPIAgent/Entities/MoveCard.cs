namespace PokeAPIAgent.Entities;

public class MoveCard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Accuracy { get; set; }
    public int EffectChance { get; set; }
    public int Pp { get; set; }
    public int Priority { get; set; }
    public int Power { get; set; }
    public string DamageClass { get; set; }
    public string Effect { get; set; }
    public string Element { get; set; }
    public List<string> PokemonNames { get; set; }
}