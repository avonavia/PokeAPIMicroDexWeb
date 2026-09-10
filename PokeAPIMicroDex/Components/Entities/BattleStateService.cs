namespace PokeAPIMicroDex.Components.Entities;

public class BattleStateService
{
    public Player? Player1 { get; set; }
    public Player? Player2 { get; set; }
    
    public bool ShouldReLoadLogs { get; set; }
}