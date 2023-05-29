public class BiomeTables
{
    public static Biome GetSurfaceBiome(int continentalness, int erosion, int valleys, int temperature, int humidity, float weirdness)
    {
        return (valleys,erosion,continentalness) switch
        {
            // Valleys
            (0,>= 0 and <= 1,0) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,>= 0 and <= 1,1) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,>= 0 and <= 1,2) => temperature < 4 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (0,>= 0 and <= 1,3) => temperature < 4 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (0,>= 2 and <= 5,0) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,>= 2 and <= 5,1) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,>= 2 and <= 5,2) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,>= 2 and <= 5,3) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,6,0) => temperature == 0 ? Biome.FrozenRiver : Biome.River,
            (0,6,1) => temperature switch
            {
                0 => Biome.FrozenRiver,
                >= 1 and <= 2 => Biome.Swamp,
                >= 3 and <= 4 => Biome.MangroveSwamp,
                _ => Biome.FrozenRiver
            },
            (0,6,2) => temperature switch
            {
                0 => Biome.FrozenRiver,
                >= 1 and <= 2 => Biome.Swamp,
                >= 3 and <= 4 => Biome.MangroveSwamp,
                _ => Biome.FrozenRiver
            },
            (0,6,3) => temperature switch
            {
                0 => Biome.FrozenRiver,
                >= 1 and <= 2 => Biome.Swamp,
                >= 3 and <= 4 => Biome.MangroveSwamp,
                _ => Biome.FrozenRiver
            },
            
            // Low Slice
            (1,>= 0 and <= 1,0) => Biome.StonyShore,
            (1,>= 0 and <= 1,1) => temperature == 0 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (1,>= 0 and <= 1,2) => (temperature,humidity) switch
            {
                (0,>= 0 and <= 1) => Biome.SnowySlopes,
                (0,>= 2 and <= 4) => Biome.Grove,
                (>= 1 and <= 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.Grove
            },
            (1,>= 0 and <= 1,3) => (temperature,humidity) switch
            {
                (0,>= 0 and <= 1) => Biome.SnowySlopes,
                (0,>= 2 and <= 4) => Biome.Grove,
                (>= 1 and <= 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.Grove
            },
            (1,2,0) => Biome.StonyShore,
            (1,2,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,2,2) => temperature == 0 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (1,2,3) => temperature == 0 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (1,3,0) => GetBeachBiome(temperature),
            (1,3,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,3,2) => temperature == 0 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (1,3,3) => temperature == 0 ? GetMiddleBiome(temperature,humidity,weirdness) : GetBadlandsBiome(temperature,weirdness),
            (1,4,0) => GetBeachBiome(temperature),
            (1,4,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,4,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,4,3) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,5,0) => (humidity,weirdness) switch
            {
                (_,< 0) => GetBeachBiome(temperature),
                (0 or 1 or 4,> 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (1,5,1) => (humidity,weirdness) switch
            {
                (_,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (0 or 1 or 4,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (1,5,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,5,3) => GetMiddleBiome(temperature,humidity,weirdness),
            (1,6,0) => GetBeachBiome(temperature),
            (1,6,1) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.Swamp
            },
            (1,6,2) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.Swamp
            },
            (1,6,3) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.Swamp
            },
            
            // Mid Slice
            (2,0,0) => Biome.StonyShore,
            (2,0,1) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (2,0,2) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (2,0,3) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (2,1,0) => Biome.StonyShore,
            (2,1,1) => (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,2 or 3 or 4) => Biome.Grove,
                (>= 1 and <= 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.Grove
            },
            (2,1,2) => (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,2 or 3 or 4) => Biome.Grove,
                (>= 1 and <= 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.Grove
            },
            (2,1,3) => (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,2 or 3 or 4) => Biome.Grove,
                (> 0,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (2,2,0) => Biome.StonyShore,
            (2,2,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,2,2) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (2,2,3) => GetPlateauBiome(temperature,humidity,weirdness),
            (2,3,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,3,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,3,2) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (2,3,3) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (2,4,0) => weirdness switch
            {
                < 0 => GetBeachBiome(temperature),
                > 0 => GetMiddleBiome(temperature,humidity,weirdness),
                _ => GetBeachBiome(temperature)
            },
            (2,4,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,4,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,4,3) => GetMiddleBiome(temperature,humidity,weirdness),
            (2,5,0) => (humidity,weirdness) switch
            {
                (_,< 0) => GetBeachBiome(temperature),
                (0 or 1 or 4,> 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (2,5,1) => (humidity,weirdness) switch
            {
                (0 or 1 or 4,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (2,5,2) => GetShatteredBiome(temperature,humidity,weirdness),
            (2,5,3) => GetShatteredBiome(temperature,humidity,weirdness),
            (2,6,0) => weirdness switch
            {
                < 0 => GetBeachBiome(temperature),
                    > 0 => GetMiddleBiome(temperature,humidity,weirdness),
                _ => GetBeachBiome(temperature)
            },
            (2,6,1) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.MangroveSwamp
            },
            (2,6,2) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.MangroveSwamp
            },
            (2,6,3) => temperature switch
            {
                0 => GetMiddleBiome(temperature,humidity,weirdness),
                1 or 2 => Biome.Swamp,
                3 or 4 => Biome.MangroveSwamp,
                _ => Biome.MangroveSwamp
            },
            
            // High Slice
            (3,0,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,0,1) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,> 2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (3,0,2) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (3,0,3) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (3,1,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,1,1) => (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,> 2 or 3 or 4) => Biome.Grove,
                (1 or 2 or 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (3,1,2) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,> 2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (3,1,3) => (temperature,humidity) switch
            {
                (< 3,0 or 1) => Biome.SnowySlopes,
                (< 3,> 2 or 3 or 4) => Biome.Grove,
                (3 or 4,_) => GetPlateauBiome(temperature,humidity,weirdness),
                _ => Biome.Grove
            },
            (3,2,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,2,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,2,2) => GetPlateauBiome(temperature,humidity,weirdness),
            (3,2,3) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (3,3,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,3,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,3,2) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (3,3,3) => GetPlateauBiome(temperature,humidity,weirdness),
            (3,4,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,4,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,4,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,4,3) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,5,0) => (humidity,weirdness) switch
            {
                (0 or 1 or 4,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (3,5,1) => (humidity,weirdness) switch
            {
                (0 or 1 or 4,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (3,5,2) => GetShatteredBiome(temperature,humidity,weirdness),
            (3,5,3) => GetShatteredBiome(temperature,humidity,weirdness),
            (3,6,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,6,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,6,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (3,6,3) => GetMiddleBiome(temperature,humidity,weirdness),
            
            // Peaks
            (4,0,0) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,0,1) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,0,2) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,0,3) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,1,0) =>  (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,> 2 or 3 or 4) => Biome.Grove,
                (1 or 2 or 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (4,1,1) =>  (temperature,humidity) switch
            {
                (0,0 or 1) => Biome.SnowySlopes,
                (0,> 2 or 3 or 4) => Biome.Grove,
                (1 or 2 or 3,_) => GetMiddleBiome(temperature,humidity,weirdness),
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (4,1,2) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,1,3) => (temperature,weirdness) switch
            {
                (0 or 1 or 2,< 0) => Biome.JaggedPeaks,
                (0 or 1 or 2,> 0) => Biome.FrozenPeaks,
                (3,_) => Biome.StonyPeaks,
                (4,_) => GetBadlandsBiome(temperature,weirdness),
                _ => Biome.FrozenPeaks
            },
            (4,2,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,2,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,2,2) => GetPlateauBiome(temperature,humidity,weirdness),
            (4,2,3) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (4,3,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,3,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,3,2) => temperature switch
            {
                < 4 => GetMiddleBiome(temperature,humidity,weirdness),
                4 => GetBadlandsBiome(temperature,weirdness),
                _ => GetMiddleBiome(temperature,humidity,weirdness),
            },
            (4,3,3) => GetPlateauBiome(temperature,humidity,weirdness),
            (4,4,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,4,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,4,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,4,3) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,5,0) => (humidity,weirdness) switch
            {
                (0 or 1 or 4,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (4,5,1) => (humidity,weirdness) switch
            {
                (0 or 1 or 4,< 0) => GetMiddleBiome(temperature,humidity,weirdness),
                (2 or 3,> 0) => Biome.WindsweptSavanna,
                _ => GetMiddleBiome(temperature,humidity,weirdness)
            },
            (4,5,2) => GetShatteredBiome(temperature,humidity,weirdness),
            (4,5,3) => GetShatteredBiome(temperature,humidity,weirdness),
            (4,6,0) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,6,1) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,6,2) => GetMiddleBiome(temperature,humidity,weirdness),
            (4,6,3) => GetMiddleBiome(temperature,humidity,weirdness),
            _ => Biome.Plains
        };
    }
    
    private static Biome GetBeachBiome(int temperature)
    {
        return temperature switch
        {
            0 => Biome.SnowyBeach,
            >= 1 and <= 3 => Biome.Beach,
            4 => Biome.Desert,
            _ => Biome.Beach
        };
    }
    
    private static Biome GetBadlandsBiome(int temperature, float weirdness)
    {
        return temperature switch
        {
            >= 0 and <= 1 => weirdness < 0 ? Biome.Badlands : Biome.ErodedBadlands,
            2 => Biome.Badlands,
            >= 3 and <= 4 => Biome.WoodedBadlands,
            _ => Biome.Badlands
        };
    }
    
    private static Biome GetMiddleBiome(int temperature, int humidity, float weirdness)
    {
        return (temperature,humidity) switch
        {
            (0,0) => weirdness < 0 ? Biome.SnowyPlains : Biome.IceSpikes,
            (0,1) => Biome.SnowyPlains,
            (0,2) => weirdness < 0 ? Biome.SnowyPlains : Biome.SnowyTaiga,
            (0,3) => Biome.SnowyTaiga,
            (0,4) => Biome.Taiga,
            (1,0) => Biome.Plains,
            (1,1) => Biome.Plains,
            (1,2) => Biome.Forest,
            (1,3) => Biome.Taiga,
            (1,4) => weirdness < 0 ? Biome.OldGrowthSpruceTaiga : Biome.OldGrowthPineTaiga,
            (2,0) => weirdness < 0 ? Biome.FlowerForest : Biome.SunflowerPlains,
            (2,1) => Biome.Plains,
            (2,2) => Biome.Forest,
            (2,3) => weirdness < 0 ? Biome.BirchForest : Biome.OldGrowthBirchForest,
            (2,4) => Biome.DarkForest,
            (3,0) => Biome.Savanna,
            (3,1) => Biome.Savanna,
            (3,2) => weirdness < 0 ? Biome.Forest : Biome.Plains,
            (3,3) => weirdness < 0 ? Biome.Jungle : Biome.SparseJungle,
            (3,4) => weirdness < 0 ? Biome.Jungle : Biome.BambooJungle,
            (4,0) => Biome.Desert,
            (4,1) => Biome.Desert,
            (4,2) => Biome.Desert,
            (4,3) => Biome.Desert,
            (4,4) => Biome.Desert,
            _ => Biome.Plains
        };
    }
    
    private static Biome GetPlateauBiome(int temperature, int humidity, float weirdness)
    {
        return (temperature,humidity) switch
        {
            (0,0) => weirdness < 0 ? Biome.SnowyPlains : Biome.IceSpikes,
            (0,1) => Biome.SnowyPlains,
            (0,2) => Biome.SnowyPlains,
            (0,3) => Biome.SnowyTaiga,
            (0,4) => Biome.SnowyTaiga,
            (1,0) => weirdness < 0 ? Biome.Meadow : Biome.CherryGrove,
            (1,1) => Biome.Meadow,
            (1,2) => weirdness < 0 ? Biome.Forest : Biome.Meadow,
            (1,3) => weirdness < 0 ? Biome.Taiga : Biome.Meadow,
            (1,4) => weirdness < 0 ? Biome.OldGrowthSpruceTaiga : Biome.OldGrowthPineTaiga,
            (2,0) => weirdness < 0 ? Biome.Meadow : Biome.CherryGrove,
            (2,1) => weirdness < 0 ? Biome.Meadow : Biome.CherryGrove,
            (2,2) => weirdness < 0 ? Biome.Meadow : Biome.Forest,
            (2,3) => weirdness < 0 ? Biome.Meadow : Biome.BirchForest,
            (2,4) => Biome.DarkForest,
            (3,0) => Biome.SavannaPlateau,
            (3,1) => Biome.SavannaPlateau,
            (3,2) => Biome.Forest,
            (3,3) => Biome.Forest,
            (3,4) => Biome.Jungle,
            (4,0) => weirdness < 0 ? Biome.Badlands : Biome.ErodedBadlands,
            (4,1) => weirdness < 0 ? Biome.Badlands : Biome.ErodedBadlands,
            (4,2) => Biome.Badlands,
            (4,3) => Biome.WoodedBadlands,
            (4,4) => Biome.WoodedBadlands,
            _ => Biome.Meadow
        };
    }
    
    private static Biome GetShatteredBiome(int temperature, int humidity, float weirdness)
    {
        return (temperature,humidity) switch
        {
            (0,0) => Biome.WindsweptGravellyHills,
            (0,1) => Biome.WindsweptGravellyHills,
            (0,2) => Biome.WindsweptHills,
            (0,3) => Biome.WindsweptForest,
            (0,4) => Biome.WindsweptForest,
            (1,0) => Biome.WindsweptGravellyHills,
            (1,1) => Biome.WindsweptGravellyHills,
            (1,2) => Biome.WindsweptHills,
            (1,3) => Biome.WindsweptForest,
            (1,4) => Biome.WindsweptForest,
            (2,0) => Biome.WindsweptHills,
            (2,1) => Biome.WindsweptHills,
            (2,2) => Biome.WindsweptHills,
            (2,3) => Biome.WindsweptForest,
            (2,4) => Biome.WindsweptForest,
            (3,0) => Biome.Savanna,
            (3,1) => Biome.Savanna,
            (3,2) => weirdness < 0 ? Biome.Forest : Biome.Plains,
            (3,3) => weirdness < 0 ? Biome.Jungle : Biome.SparseJungle,
            (3,4) => weirdness < 0 ? Biome.Jungle : Biome.BambooJungle,
            (4,0) => Biome.Desert,
            (4,1) => Biome.Desert,
            (4,2) => Biome.Desert,
            (4,3) => Biome.Desert,
            (4,4) => Biome.Desert,
            _ => Biome.WindsweptHills
        };
    }

    public static Biome GetOceanBiome(int temperature)
    {
        return temperature switch
        {
            0 => Biome.FrozenOcean,
            1 => Biome.ColdOcean,
            2 => Biome.Ocean,
            3 => Biome.LukewarmOcean,
            4 => Biome.WarmOcean,
            _ => Biome.Ocean
        };
    }
}
