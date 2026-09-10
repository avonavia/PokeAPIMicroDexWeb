namespace PokeAPIMicroDex.Components.Entities;

public static class PokemonTypeAgent
{
    public enum PokemonType
    {
        Normal, // 0
        Fire, // 1
        Water, // 2
        Grass, // 3
        Electric, // 4
        Ice, // 5
        Fighting, // 6
        Poison, // 7
        Ground, // 8
        Flying, // 9
        Psychic, // 10
        Bug, // 11
        Rock, // 12
        Ghost, // 13
        Dragon, // 14
        Dark, // 15
        Steel, // 16
        Fairy // 17
    }
    
    public enum EffectivenessRating
    {
        NoEffect,             // 0x
        NotVeryEffective,     // 0.25x - 0.5x
        Normal,               // 1x
        SuperEffective        // 2x - 4x
    }
    
    public static PokemonType? GetTypeFromString(string typeName)
    {
        if (Enum.TryParse<PokemonType>(typeName, true, out PokemonType result))
        {
            return result;
        }

        return null;
    }

    private static readonly float[,] Effectiveness =
    {
        // Nor, Fir, Wat, Gra, Ele, Ice, Fig, Poi, Gro, Fly, Psy, Bug, Roc, Gho, Dra, Dar, Ste, Fai
        /* Normal */ { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0f, 1f, 1f, 0.5f, 1f },
        /* Fire */ { 1f, 0.5f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 0.5f, 1f, 2f, 1f },
        /* Water */ { 1f, 2f, 0.5f, 0.5f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 1f, 1f },
        /* Grass */ { 1f, 0.5f, 2f, 0.5f, 1f, 1f, 1f, 0.5f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 0.5f, 1f, 0.5f, 1f },
        /* Electric */ { 1f, 1f, 2f, 0.5f, 0.5f, 1f, 1f, 1f, 0f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f },
        /* Ice */ { 1f, 0.5f, 0.5f, 2f, 1f, 0.5f, 1f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f },
        /* Fighting */ { 2f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f, 0.5f, 0.5f, 2f, 0f, 1f, 2f, 2f, 0.5f },
        /* Poison */ { 1f, 1f, 1f, 2f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 0f, 2f },
        /* Ground */ { 1f, 2f, 1f, 0.5f, 2f, 1f, 1f, 2f, 1f, 0f, 1f, 0.5f, 2f, 1f, 1f, 1f, 2f, 1f },
        /* Flying */ { 1f, 1f, 1f, 2f, 0.5f, 1f, 2f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 0.5f, 1f },
        /* Psychic */ { 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 1f, 1f, 0.5f, 1f, 1f, 1f, 1f, 0f, 0.5f, 1f },
        /* Bug */ { 1f, 0.5f, 1f, 2f, 1f, 1f, 0.5f, 0.5f, 1f, 0.5f, 2f, 1f, 1f, 0.5f, 1f, 2f, 0.5f, 0.5f },
        /* Rock */ { 1f, 2f, 1f, 1f, 1f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f },
        /* Ghost */ { 0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 2f, 1f, 0.5f, 1f, 1f },
        /* Dragon */ { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 0f },
        /* Dark */ { 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f },
        /* Steel */ { 1f, 0.5f, 0.5f, 1f, 0.5f, 2f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 0.5f, 2f },
        /* Fairy */ { 1f, 0.5f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 0.5f, 1f }
    };

    private static float GetEffectiveness(PokemonType? attackType, PokemonType? defendType)
    {
        return Effectiveness[(int)attackType, (int)defendType];
    }
    
    public static float CalculateTotalEffectiveness(PokemonType? attackType, PokemonType? defendType1,
        PokemonType? defendType2 = null)
    {
        float multiplier = GetEffectiveness(attackType, defendType1);

        if (defendType2.HasValue)
        {
            multiplier *= GetEffectiveness(attackType, defendType2.Value);
        }

        return multiplier;
    }

    private static EffectivenessRating GetRating(float multiplier)
    {
        if (multiplier == 0f) return EffectivenessRating.NoEffect;
        if (multiplier < 1f) return EffectivenessRating.NotVeryEffective;
        if (multiplier > 1f) return EffectivenessRating.SuperEffective;
        return EffectivenessRating.Normal;
    }
    
    public static string GetEffectivenessMessage(float multiplier)
    {
        EffectivenessRating rating = GetRating(multiplier);

        return rating switch
        {
            EffectivenessRating.SuperEffective => "It's super effective!",
            EffectivenessRating.NotVeryEffective => "It's not very effective...",
            EffectivenessRating.NoEffect => "It doesn't affect the opponent...",
            _ => "The attack hit normally."
        };
    }
}