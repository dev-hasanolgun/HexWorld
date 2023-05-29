using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Jobs;
using UnityEngine.Rendering;

public class WorldMapGen
{
    private readonly int _seed;
    private readonly int _chunkSize;
    private readonly int _hexAmount;
    private readonly float _mapScale;
    private readonly Bounds _bounds;
    private readonly float _obstaclePercent;
    
    private NativeList<Hex3> _nativeHexPositions;
    private BiomeGen _biomeGen;

    public WorldMapGen(int seed, int chunkSize, float mapScale, float obstaclePercent, BiomeGen biomeGen)
    {
        _chunkSize = chunkSize;
        _hexAmount = 1 + 3 * _chunkSize * (_chunkSize + 1);
        _mapScale = mapScale;
        _obstaclePercent = obstaclePercent;
        _biomeGen = biomeGen;
        _seed = seed;
        _nativeHexPositions = new NativeList<Hex3>(_hexAmount, Allocator.Persistent);
        
        for (int x = -_chunkSize; x <= _chunkSize; x++)
        {
            var min = x <= 0 ? -_chunkSize - x : -_chunkSize;
            var max = x <= 0 ? _chunkSize : _chunkSize - x;
        
            for (int y = min; y <= max; y++)
            {
                var hex = new Hex3(x, y, 0 - (x + y));
                _nativeHexPositions.Add(hex);
            }
        }

        var sizeX = (Hex3.right * _chunkSize).ToVector3XZ(mapScale).x - (Hex3.left * _chunkSize).ToVector3XZ(mapScale).x;
        var sizeY = 1f;
        var sizeZ = (Hex3.upLeft * _chunkSize).ToVector3XZ(mapScale).z - (Hex3.downLeft * _chunkSize).ToVector3XZ(mapScale).z;
        _bounds = new Bounds(Vector3.zero, new Vector3(sizeX, sizeY, sizeZ));
    }

    public HexTileData GetHexTile(Hex3 hex3)
    {
        var worldPos = Hex3.ToVector3XZ(hex3, _mapScale);

        var tileType = _biomeGen.GetTileType(hex3);
        var biome = _biomeGen.GetBiome(hex3);
        var blockData = new HexTileData(hex3, worldPos, tileType, biome);

        return blockData;
    }

    public NativeList<ChunkData> GetChunksInDistance(Hex3 center, int distance)
    {
        var chunkList = new NativeList<ChunkData>(Allocator.Temp);

        var centerChunk = new ChunkData(center, _chunkSize);
        chunkList.Add(centerChunk);

        for (int i = 1; i < distance; i++)
        {
            var cornerHex = new Hex3(2 * _chunkSize + 1, -_chunkSize, -_chunkSize - 1) * i;
            
            for (int j = 0; j < 6; j++)
            {
                var previousCorner = cornerHex;
                cornerHex = Hex3.RotateClockwise(cornerHex);
                chunkList.Add(new ChunkData(cornerHex+center, _chunkSize));
                
                var dir = (previousCorner - cornerHex) / i;
                
                for (int k = 1; k < i; k++)
                {
                    var hexPos = cornerHex + dir * k;
                    chunkList.Add(new ChunkData(hexPos+center, _chunkSize));
                }
            }
        }

        return chunkList;
    }

    public Mesh UpdateChunkUVs(Mesh chunkMesh, Hex3 center)
    {
        var meshData = Mesh.AllocateWritableMeshData(chunkMesh);
        var hexAmount = 1 + 3 * _chunkSize * (_chunkSize + 1);
        // var obstacles = GetChunkObstacles(center);

        var jobs = new UVUpdateJob
        {
            HexArray = _nativeHexPositions.AsArray(),
            // ObstacleArray = obstacles,
            Center = center,
            MapScale = _mapScale,
            BiomeGen = _biomeGen,
            OutputMeshData = meshData[0]
        };
        var flags = MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontResetBoneBounds;
        var handle = jobs.Schedule(hexAmount, 64);
        handle.Complete();
        Mesh.ApplyAndDisposeWritableMeshData(meshData, new[]{chunkMesh}, flags);

        // obstacles.Dispose();
        jobs.HexArray.Dispose();
        return chunkMesh;
    }

    public Mesh GenerateChunkMesh(Hex3 center)
    {
        var worldMapMesh = new Mesh();
        var hexAmount = 1 + 3 * _chunkSize * (_chunkSize + 1);
        // var obstacles = GetChunkObstacles(center);

        var hexMeshGenJob = new HexMeshGenJob
        {
            HexArray = _nativeHexPositions.AsArray(),
            // ObstacleArray = obstacles,
            Center = center,
            MapScale = _mapScale,
            BiomeGen = _biomeGen
        };

        var dataArray = Mesh.AllocateWritableMeshData(1);
        hexMeshGenJob.OutputMeshData = dataArray[0];
        hexMeshGenJob.OutputMeshData.subMeshCount = 1;
        hexMeshGenJob.OutputMeshData.SetIndexBufferParams(hexAmount * 12, IndexFormat.UInt32);
        hexMeshGenJob.OutputMeshData.SetVertexBufferParams(hexAmount * 6, 
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32), 
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, stream:1),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, stream:2));
        
        var flags = MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontResetBoneBounds;
        var meshDesc = new SubMeshDescriptor(0, hexAmount * 12, MeshTopology.Triangles);
        hexMeshGenJob.OutputMeshData.SetSubMesh(0, meshDesc, flags);
        
        var handle = hexMeshGenJob.Schedule(hexAmount, 64);
        handle.Complete();
        worldMapMesh.bounds = _bounds;

        Mesh.ApplyAndDisposeWritableMeshData(dataArray, new[]{worldMapMesh}, flags);

        // obstacles.Dispose();
        hexMeshGenJob.HexArray.Dispose();
        return worldMapMesh;
    }
    
    public NativeArray<Hex3> GetChunkObstacles(Hex3 center)
    {
        var hexAmount = 1 + 3 * _chunkSize * (_chunkSize + 1);
        var chunkObstacles = new NativeArray<Hex3>(hexAmount,Allocator.TempJob);
        
        var floodFillJob = new FloodFillJob
        {
            BiomeGen = _biomeGen,
            Center = center,
            ChunkSize = _chunkSize,
            ObstaclePercent = _obstaclePercent,
            ObstacleArray = chunkObstacles,
            Seed = Random.Range(1,100000)
        };
        var handle = floodFillJob.Schedule(6, 64);
        handle.Complete();
        
        return floodFillJob.ObstacleArray;
    }
}