using Unity.Mathematics;

public class NoiseGen
{
    public static float FBmNoiseValue(int x, int y, float frequency, NoiseType noiseType, FBmSettings fBmSettings)
    {
        var octaves = fBmSettings.Octaves;
        var lacunarity = fBmSettings.Lacunarity;
        var gain = fBmSettings.Gain;

        return FBmNoiseValue(x, y, frequency, octaves, lacunarity, gain, noiseType);
    }
    
    public static float FBmNoiseValue(int x, int y, int z, float frequency, NoiseType noiseType, FBmSettings fBmSettings)
    {
        var octaves = fBmSettings.Octaves;
        var lacunarity = fBmSettings.Lacunarity;
        var gain = fBmSettings.Gain;

        return FBmNoiseValue(x, y, z, frequency, octaves, lacunarity, gain, noiseType);
    }
    
    public static float FBmNoiseValue(int x, int y, float frequency, int octaves, float lacunarity, float gain, NoiseType noiseType = NoiseType.Perlin)
    {
        var noiseValue = 0f;
        var currentFrequency = frequency;
        var currentGain = 1f;
        var maxValue = 1f;
        
        for (int i = 0; i < octaves; i++)
        {
            var value = noiseType switch
            {
                NoiseType.Perlin => noise.cnoise(new float2(x * currentFrequency, y * currentFrequency)),
                NoiseType.Simplex => noise.snoise(new float2(x * currentFrequency, y * currentFrequency)),
                _ => 0f
            };
            currentFrequency *= lacunarity;
            noiseValue += value * currentGain;
            currentGain *= gain;
            maxValue += currentGain;
        }
        
        var rangeFactor = noiseType switch
        {
            NoiseType.Perlin => 0.25f,
            NoiseType.Simplex => -0.25f,
            _ => 0f
        };
    
        return noiseValue / (maxValue + rangeFactor);
    }
    
    public static float FBmNoiseValue(int x, int y, int z, float frequency, int octaves, float lacunarity, float gain, NoiseType noiseType = NoiseType.Perlin)
    {
        var noiseValue = 0f;
        var currentFrequency = frequency;
        var currentGain = 1f;
        var maxValue = 1f;
        
        for (int i = 0; i < octaves; i++)
        {
            var value = noiseType switch
            {
                NoiseType.Perlin => noise.cnoise(new float3(x * currentFrequency, y * currentFrequency, z * currentFrequency)),
                NoiseType.Simplex => noise.snoise(new float3(x * currentFrequency, y * currentFrequency, z * currentFrequency)),
                _ => 0f
            };
            currentFrequency *= lacunarity;
            noiseValue += value * currentGain;
            currentGain *= gain;
            maxValue += currentGain;
        }
        
        var rangeFactor = noiseType switch
        {
            NoiseType.Perlin => 0.25f,
            NoiseType.Simplex => -0.25f,
            _ => 0f
        };
    
        return noiseValue / (maxValue + rangeFactor);
    }

    public struct FBmSettings
    {
        public readonly int Octaves;
        public readonly float Lacunarity;
        public readonly float Gain;

        public FBmSettings(int octaves, float lacunarity, float gain)
        {
            Octaves = octaves;
            Lacunarity = lacunarity;
            Gain = gain;
        }
    }
}