using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct HexMeshGenJob : IJobParallelFor
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
        var biome = BiomeGen.GetBiome(hex);
            
        var verts = HexMeshGen.GetVertices(worldPos, biome);
        var normals = HexMeshGen.GetNormals(worldPos, biome);
        var uvs = HexMeshGen.GetUVs(worldPos, biome);
        var tris = HexMeshGen.GetIndices(worldPos, BiomeGen.GetBiome(localHex));
            
        var outputVerts = OutputMeshData.GetVertexData<float3>();
        var outputNormals = OutputMeshData.GetVertexData<float3>(stream:1);
        var outputUVs = OutputMeshData.GetVertexData<float2>(stream:2);
        var outputTris = OutputMeshData.GetIndexData<uint>();

        for (int i = 0; i < verts.Length; i++)
        {
            var vertex = verts[i];
            outputVerts[i + verts.Length * index] = vertex;
            
            var normal = normals[i];
            outputNormals[i + verts.Length * index] = normal;
            
            var uv = uvs[i]; 
            outputUVs[i + verts.Length * index] = uv;
            
        }
        
        for (var i = 0; i < tris.Length; ++i)
        {
            var idx = tris[i];
            outputTris[i + tris.Length * index] = idx + (uint) (verts.Length * index);
        }

        verts.Dispose();
        uvs.Dispose();
    }
}
