using Unity.Collections;
using Unity.Mathematics;

public class HexMeshGen
{
    private const float Deg2Rad = 0.017453292f;
    private const float InitialAngle = 30f;
    private const float AngleIncrement = 60 * Deg2Rad;
    private const float UVScale = 1f / 64;

    private static float2 GetUV(Biome biome)
    {
        var atlasSize = (int) math.ceil(math.sqrt(64));
        while (atlasSize * atlasSize < 64)
        {
            atlasSize++;
        }
        
        var gridSize = 1.0f / atlasSize;

        for (int i = 0; i < 64; i++)
        {
            var x = i % atlasSize;
            var y = atlasSize - 1 - (i / atlasSize);

            var pos = new float2(x * gridSize + gridSize / 2, y * gridSize + gridSize / 2);

            if (biome == (Biome)i) return pos;
        }

        return default;
    }
    
    public static NativeArray<float3> GetVertices(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var vertices = new NativeList<float3>(6,Allocator.Temp);
    
        // Get top and bottom face mesh data
        var topMeshData = GetTopFace(center, biome, height, radius);
        // var bottomMeshData = GetBottomFace(center, biome, height, radius);
    
        
        vertices.AddRange(topMeshData.Vertices);
        // vertices.AddRange(bottomMeshData.Vertices);

        // Add side face mesh data
        // for (int i = 0; i < 6; i++)
        // {
        //     var sideMeshData = GetSideFace(center, i, biome, height, radius);
        //     vertices.AddRange(sideMeshData.Vertices);
        //     
        //     sideMeshData.Dispose();
        // }
        
        topMeshData.Dispose();
        // bottomMeshData.Dispose();

        return vertices.AsArray();
    }
    
    public static NativeArray<float2> GetUVs(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var uvs = new NativeList<float2>(6,Allocator.Temp);
    
        // Get top and bottom face mesh data
        var topMeshData = GetTopFace(center, biome, height, radius);
        // var bottomMeshData = GetBottomFace(center, biome, height, radius);

        uvs.AddRange(topMeshData.UVs);
        // uvs.AddRange(bottomMeshData.UVs);

        // Add side face mesh data
        // for (int i = 0; i < 6; i++)
        // {
        //     var sideMeshData = GetSideFace(center, i, biome, height, radius);
        //     uvs.AddRange(sideMeshData.UVs);
        //     
        //     sideMeshData.Dispose();
        // }
        
        topMeshData.Dispose();
        // bottomMeshData.Dispose();

        return uvs.AsArray();
    }
    
    public static NativeArray<uint> GetIndices(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var triangles = new NativeList<uint>(12,Allocator.Temp);
    
        // Get top and bottom face mesh data
        var topMeshData = GetTopFace(center, biome, height, radius);
        // var bottomMeshData = GetBottomFace(center, biome, height, radius);

        triangles.AddRange(topMeshData.Triangles);

        // Set top face indices
        // for (int i = 0; i < topMeshData.Triangles.Length; i++)
        // {
        //     triangles.Add(bottomMeshData.Triangles[i] + 6);
        // }

        // Add side face mesh data
        // for (int i = 0; i < 6; i++)
        // {
        //     var sideMeshData = GetSideFace(center, i, biome, height, radius);
        //         
        //     // Set top face indices
        //     for (int j = 0; j < sideMeshData.Triangles.Length; j++)
        //     {
        //         triangles.Add(sideMeshData.Triangles[j] + 12 + 4 * (uint) i);
        //     }
        //     
        //     sideMeshData.Dispose();
        // }
        
        topMeshData.Dispose();
        // bottomMeshData.Dispose();

        return triangles.AsArray();
    }
    
    public static NativeArray<float3> GetNormals(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var normals = new NativeList<float3>(6,Allocator.Temp);
    
        // Get top and bottom face mesh data
        var topMeshData = GetTopFace(center, biome, height, radius);
        // var bottomMeshData = GetBottomFace(center, biome, height, radius);

        normals.AddRange(topMeshData.Normals);
        // normals.AddRange(bottomMeshData.Normals);

        // Add side face mesh data
        // for (int i = 0; i < 6; i++)
        // {
        //     var sideMeshData = GetSideFace(center, i, biome, height, radius);
        //     normals.AddRange(sideMeshData.Normals);
        //     
        //     sideMeshData.Dispose();
        // }
        
        topMeshData.Dispose();
        // bottomMeshData.Dispose();

        return normals.AsArray();
    }

    private static MeshData GetTopFace(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var vertices = new NativeArray<float3>(6,Allocator.Temp);
        var triangles = new NativeArray<uint>(12,Allocator.Temp);
        var uvs = new NativeArray<float2>(6,Allocator.Temp);
        var normals = new NativeArray<float3>(6, Allocator.Temp);
        
        for (int i = 0; i < 6; i++)
        {
            var angle = InitialAngle * Deg2Rad + AngleIncrement * i;
            var x = radius * 2 * math.cos(angle);
            var y = height;
            var z = radius * 2 * math.sin(angle);
            
            var vertex = new float3(x, y, z) + center;
            vertices[i] = vertex;

            var uv = new float2(x,z) * (UVScale / math.sqrt(3)) + GetUV(biome);
            uvs[i] = uv;
            
            // Calculate the normal vector
            normals[i] = new float3(0,1,0);
        }

        var j = 0;
        for (uint i = 0; i < 4; i++, j += 3)
        {
            triangles[j] = i + 2;
            triangles[j + 1] = i + 1;
            triangles[j + 2] = 0;
        }

        var meshData = new MeshData(vertices, triangles, uvs, normals);
        
        return meshData;
    }

    private static MeshData GetBottomFace(float3 center, Biome biome, float height = 1f, float radius = 1f)
    {
        var vertices = new NativeArray<float3>(6,Allocator.Temp);
        var triangles = new NativeArray<uint>(12,Allocator.Temp);
        var uvs = new NativeArray<float2>(6,Allocator.Temp);
        var normals = new NativeArray<float3>(6, Allocator.Temp);

        for (int i = 0; i < 6; i++)
        {
            var angle = InitialAngle * Deg2Rad + AngleIncrement * i;
            var x = radius * 2 * math.cos(angle);
            var y = -height;
            var z = radius * 2 * math.sin(angle);
            
            var vertex = new float3(x, y, z) + center;
            vertices[i] = vertex;
            
            var uv = new float2(x,z) * (UVScale / math.sqrt(3)) + GetUV(biome);
            uvs[i] = uv;
            
            // Calculate the normal vector
            normals[i] = new float3(0,-1,0);
        }

        var j = 0;
        for (uint i = 0; i < 4; i++, j += 3)
        {
            triangles[j] = 0;
            triangles[j + 1] = i + 1;
            triangles[j + 2] = i + 2;
        }
        
        var meshData = new MeshData(vertices, triangles, uvs, normals);

        return meshData;
    }

    private static MeshData GetSideFace(float3 center, int sideIndex, Biome biome, float height = 1f, float radius = 1f)
    {
        var angle1 = Deg2Rad * (InitialAngle + 60 * sideIndex);
        var angle2 = Deg2Rad * (InitialAngle + 60 * ((sideIndex + 1) % 6));

        var vertices = new NativeArray<float3>(4, Allocator.Temp);

        vertices[0] = new float3(radius * 2 * math.cos(angle1), -height, radius * 2 * math.sin(angle1)) + center;
        vertices[1] = new float3(radius * 2 * math.cos(angle2), -height, radius * 2 * math.sin(angle2)) + center;
        vertices[2] = new float3(radius * 2 * math.cos(angle1), height, radius * 2 * math.sin(angle1)) + center;
        vertices[3] = new float3(radius * 2 * math.cos(angle2), height, radius * 2 * math.sin(angle2)) + center;

        var triangles = new NativeArray<uint>(6, Allocator.Temp);
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;
        
        var uvs = new NativeArray<float2>(4, Allocator.Temp);
        
        uvs[0] = new float2(-0.5f, -0.5f) * UVScale + GetUV(biome);
        uvs[1] = new float2(0.5f, -0.5f) * UVScale + GetUV(biome);
        uvs[2] = new float2(-0.5f, 0.5f) * UVScale + GetUV(biome);
        uvs[3] = new float2(0.5f, 0.5f) * UVScale + GetUV(biome);
        
        var normals = new NativeArray<float3>(4, Allocator.Temp);
        
        normals[0] = new float3(0, 0, 1);
        normals[1] = new float3(0, 0, 1);
        normals[2] = new float3(0, 0, 1);
        normals[3] = new float3(0, 0, 1);
    
        var meshData = new MeshData(vertices, triangles, uvs, normals);

        return meshData;
    }

    private struct MeshData
    {
        public NativeArray<float3> Vertices;
        public NativeArray<uint> Triangles;
        public NativeArray<float2> UVs;
        public NativeArray<float3> Normals;

        public MeshData(NativeArray<float3> vertices, NativeArray<uint> triangles, NativeArray<float2> uvs, NativeArray<float3> normals)
        {
            Vertices = vertices;
            Triangles = triangles;
            UVs = uvs;
            Normals = normals;
        }
        
        public void Dispose()
        {
            Vertices.Dispose();
            UVs.Dispose();
            Triangles.Dispose();
            Normals.Dispose();
        }
    }
}
