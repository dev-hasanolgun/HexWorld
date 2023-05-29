using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct FloodFillJob : IJobParallelFor
{
    [ReadOnly] public BiomeGen BiomeGen;
    [ReadOnly] public Hex3 Center;
    [ReadOnly] public int ChunkSize;
    [ReadOnly] public float ObstaclePercent;
    [NativeDisableParallelForRestriction]
    public NativeArray<Hex3> ObstacleArray;
    public int Seed;

    public void Execute(int index)
    {
        var hexAmount = 3 * ChunkSize * (ChunkSize + 1);
        var pieHexAmount = hexAmount / 6;
        var pie = HexJobExtensions.Pie(Center, ChunkSize, index);
        var obstacles = DefineObstacles(pie,index);
        
        for (int i = 0; i < obstacles.Length; i++)
        {
            ObstacleArray[i + index * pieHexAmount] = obstacles[i];
        }

        pie.Dispose();
        obstacles.Dispose();
    }
    
    private NativeList<NativeList<Hex3>> DefineRegions(NativeList<Hex3> hexList)
    {
        var definedRegions = new NativeList<NativeList<Hex3>>(Allocator.Temp);
        
        while (hexList.Length > 0)
        {
            var openList = new NativeList<Hex3>(Allocator.Temp);
            var closeList = new NativeList<Hex3>(Allocator.Temp);

            openList.Add(hexList[0]);
            closeList.Add(hexList[0]);

            while (openList.Length > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    var neighbor = Hex3.GetNeighbor(openList[0],i);
                    
                    if (hexList.Contains(neighbor) && !closeList.Contains(neighbor) && !openList.Contains(neighbor))
                    {
                        var neighborTileType = BiomeGen.GetTileType(neighbor);
                        var neighborBiomeType = BiomeGen.GetBiome(neighbor);
                        
                        if (neighborTileType != TileType.Ocean && neighborBiomeType != Biome.River && neighborBiomeType != Biome.FrozenRiver)
                        {
                            closeList.Add(neighbor);
                            openList.Add(neighbor);
                        }
                    }
                }
                hexList.RemoveAt(hexList.IndexOf(openList[0]));
                openList.RemoveAt(0);
            }

            definedRegions.Add(closeList);
            openList.Dispose();
        }

        return definedRegions;
    }
    
    private NativeList<Hex3> DefineObstacles(NativeList<Hex3> hexList, int index)
    {
        var regions = DefineRegions(hexList);
        var obstacleList = new NativeList<Hex3>(Allocator.Temp);

        for (int i = 0; i < regions.Length; i++)
        {
            var region = regions[i];
            var obstacleCount = (int)(region.Length * ObstaclePercent);
            var currentObstacleCount = 0;

            region = region.Shuffle(Seed * (index + 1));
        
            for (int j = 0; j < obstacleCount; j++)
            {
                var randomHex = region[j];
                var neighborObstacleAmount = 0;
                
                for (int k = 0; k < 6; k++)
                {
                    var neighbor = Hex3.GetNeighbor(randomHex,k);
                    if (obstacleList.Contains(neighbor)) neighborObstacleAmount++;
                }
                
                currentObstacleCount++;
                
                obstacleList.Add(randomHex);
                
                // if (neighborObstacleAmount <= 0) continue;
                if (MapFullyAccessible(region, obstacleList, region[0], region.Length, currentObstacleCount)) continue;
                
                obstacleList.RemoveAt(obstacleList.IndexOf(randomHex));
                currentObstacleCount--;
            }

            region.Dispose();
        }

        regions.Dispose();

        return obstacleList;
    }
    
    private bool MapFullyAccessible(NativeList<Hex3> hexList, NativeList<Hex3> obstacleList, Hex3 hex, int targetAccessibleTileCount, int currentObstacleCount)
    {
        var openList = new NativeList<Hex3>(Allocator.Temp) {hex};
        var closeList = new NativeList<Hex3>(Allocator.Temp) {hex};

        var accessibleTileCount = 0;
        
        while (openList.Length > 0)
        { 
            for (int i = 0; i < 6; i++)
            {
                var neighbor = Hex3.GetNeighbor(openList[0], i);
                if (!obstacleList.Contains(neighbor) && !closeList.Contains(neighbor) && !openList.Contains(neighbor) && hexList.Contains(neighbor))
                {
                    closeList.Add(neighbor);
                    openList.Add(neighbor);
                    accessibleTileCount++;
                }
            }
            openList.RemoveAt(0);
        }

        openList.Dispose();
        closeList.Dispose();

        return targetAccessibleTileCount-currentObstacleCount == accessibleTileCount;
    }
}