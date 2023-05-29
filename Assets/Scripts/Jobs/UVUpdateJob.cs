using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct UVUpdateJob : IJobParallelFor
{
    public Mesh.MeshData OutputMeshData;
    [ReadOnly] public BiomeGen BiomeGen;
    [ReadOnly] public NativeArray<Hex3> HexArray;
    // [ReadOnly] public NativeArray<Hex3> ObstacleArray;
    [ReadOnly] public Hex3 Center;
    [ReadOnly] public float MapScale;

    public void Execute(int index)
    {
        var localHex = HexArray[index];
        var hex = localHex+Center;
        var worldPos = localHex.ToVector3XZ(MapScale);
        // var biome = ObstacleArray.Contains(hex) ? Biome.Obstacle : BiomeGen.GetBiome(hex+Center);
        var biome = BiomeGen.GetBiome(hex);
        
        var uvs = HexMeshGen.GetUVs(worldPos, biome);
        var outputUVs = OutputMeshData.GetVertexData<float2>(stream:2);

        for (int i = 0; i < uvs.Length; i++)
        {
            var uv = uvs[i]; 
            outputUVs[i + uvs.Length * index] = uv;
        }
        
        uvs.Dispose();
    }
}
