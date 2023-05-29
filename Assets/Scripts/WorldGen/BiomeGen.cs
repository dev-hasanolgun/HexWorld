using UnityEngine;

public struct BiomeGen
{
    public int Seed;
    public float FrequencyFactor;
    public float ObstacleInterval;

    public float TemperatureFrequency;
    public float HumidityFrequency;
    public float ContinentalnessFrequency;
    public float ErosionFrequency;
    public float WeirdnessFrequency;
        
    public NoiseType TemperatureNoiseType;
    public NoiseType HumidityNoiseType;
    public NoiseType ContinentalnessNoiseType;
    public NoiseType ErosionNoiseType;
    public NoiseType WeirdnessNoiseType;

    public NoiseGen.FBmSettings FBmSettings;

    public TileType GetTileType(Hex3 hex3)
    {
        return NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.x+Seed, hex3.x+Seed, ContinentalnessFrequency*FrequencyFactor,ContinentalnessNoiseType,FBmSettings) < -0.19f ? TileType.Ocean : TileType.Island;
    }
    
    public Biome GetBiome(Hex3 hex3)
    {
        var continentalness = NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.y+Seed, hex3.z+Seed, ContinentalnessFrequency*FrequencyFactor, ContinentalnessNoiseType, FBmSettings);
        var erosion = NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.y+Seed, hex3.z+Seed, ErosionFrequency*FrequencyFactor, ErosionNoiseType, FBmSettings);
        var temperature = NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.y+Seed, hex3.z+Seed, TemperatureFrequency*FrequencyFactor, TemperatureNoiseType, FBmSettings);
        var humidity = NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.y+Seed, hex3.z+Seed, HumidityFrequency*FrequencyFactor, HumidityNoiseType, FBmSettings);
        var weirdness = NoiseGen.FBmNoiseValue(hex3.x+Seed, hex3.y+Seed, hex3.z+Seed, WeirdnessFrequency*FrequencyFactor, WeirdnessNoiseType, FBmSettings);
        var valleys = 1 - Mathf.Abs(3 * Mathf.Abs(weirdness) - 2);

        var continentalnessIndex = 0;
        continentalnessIndex = continentalness switch
        {
            > -0.19f and <= -0.11f => 0,
            > -0.11f and <= 0.03f => 1,
            > 0.03f and <= 0.3f => 2,
            > 0.3f and <= 1f => 3,
            _ => continentalnessIndex
        };
        var erosionIndex = 0;
        erosionIndex = erosion switch
        {
            > -1f and <= -0.78f => 0,
            > -0.78f and <= -0.375f => 1,
            > -0.375f and <= -0.2225f => 2,
            > -0.2225f and <= 0.05f => 3,
            > 0.05f and <= 0.45f => 4,
            > 0.45f and <= 0.55f => 5,
            > 0.55f and <= 1f => 6,
            _ => erosionIndex
        };
        var valleysIndex = 0;
        valleysIndex = valleys switch
        {
            > -1f and <= -0.85f => 0,
            > -0.85f and <= -0.6f => 1,
            > -0.6f and <= 0.2f => 2,
            > 0.2f and <= 0.7f => 3,
            > 0.7f and <= 1f => 4,
            _ => valleysIndex
        };
        var temperatureIndex = 0;
        temperatureIndex = temperature switch
        {
            > -1f and <= -0.45f => 0,
            > -0.45f and <= -0.15f => 1,
            > -0.15f and <= 0.2f => 2,
            > 0.2f and <= 0.55f => 3,
            > 0.55f and <= 1f => 4,
            _ => temperatureIndex
        };
        var humidityIndex = 0;
        humidityIndex = humidity switch
        {
            > -1f and <= -0.35f => 0,
            > -0.35f and <= -0.1f => 1,
            > -0.1f and <= 0.1f => 2,
            > 0.1f and <= 0.3f => 3,
            > 0.3f and <= 1f => 4,
            _ => humidityIndex
        };
        
        if (continentalness < -0.19f)
        {
            return BiomeTables.GetOceanBiome(temperatureIndex);
        }
        
        var biome = BiomeTables.GetSurfaceBiome(continentalnessIndex, erosionIndex, valleysIndex, temperatureIndex, humidityIndex, weirdness);

        if (biome != Biome.River && biome != Biome.FrozenRiver)
        {
            biome = NoiseGen.FBmNoiseValue(hex3.x,hex3.y,hex3.z,0.8f,NoiseType.Perlin,FBmSettings) > ObstacleInterval ? Biome.Obstacle : biome;
        }

        return biome;
    }
}
