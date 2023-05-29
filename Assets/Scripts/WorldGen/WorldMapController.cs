using System.Collections.Generic;
using Pooling;
using UnityEngine;

public class WorldMapController : MonoBehaviour
{
    public int ChunkSize;
    public int Seed;
    public float FrequencyFactor;
    public float MapScale;
    [Range(0,0.99f)]
    public float FloodFillObstaclePercent = 0.2f;
    public float NoiseValueObstacleInterval = 0.2f;
    public int LoadDistance = 1;
    public int UnloadDistance = 2;
    public Material TileMat;
    public ChunkMono ChunkPrefab;
    public BiomeGen BiomeGen;

    private WorldMapGen _worldGen;
    private Dictionary<ChunkData,ChunkMono> _drawnChunks = new();
    
    private void Start()
    {
        InitializeWorld();
    }
    
    public void InitializeWorld()
    {
        BiomeGen = new BiomeGen
        {
            Seed = Seed,
            FrequencyFactor = FrequencyFactor,
            ObstacleInterval = NoiseValueObstacleInterval,
            TemperatureFrequency = 0.003f,
            HumidityFrequency = 0.005f,
            ContinentalnessFrequency = 0.005f,
            ErosionFrequency = 0.008f,
            WeirdnessFrequency = 0.01f,
            TemperatureNoiseType = NoiseType.Simplex,
            HumidityNoiseType = NoiseType.Simplex,
            ContinentalnessNoiseType = NoiseType.Perlin,
            ErosionNoiseType = NoiseType.Perlin,
            WeirdnessNoiseType = NoiseType.Simplex,
            FBmSettings = new NoiseGen.FBmSettings(6,2f,0.5f)
        };
        _worldGen = new WorldMapGen(Seed, ChunkSize, MapScale, FloodFillObstaclePercent, BiomeGen);
    }

    public void LoadChunksInRange(Transform player)
    {
        var center = Hex3.XZToHex3(player.transform.position, MapScale);
        var chunksInRange = _worldGen.GetChunksInDistance(center-Hex3.InverseHexMod(Hex3.HexMod(center,ChunkSize),ChunkSize),LoadDistance);
        
        for (int i = 0; i < chunksInRange.Length; i++)
        {
            var chunkData = chunksInRange[i];

            if (!_drawnChunks.ContainsKey(chunkData))
            {
                var chunkMesh = _worldGen.GenerateChunkMesh(chunkData.Center);
                var chunkObj = ChunkPrefab.Retrieve("ChunkObj"); 
                chunkObj.MeshFilter.mesh = chunkMesh;
                chunkObj.MeshRenderer.material = TileMat;
                chunkObj.transform.SetParent(transform);
                chunkObj.transform.position = chunkData.Center.ToVector3XZ(MapScale);
                _drawnChunks.Add(chunkData,chunkObj);
            }
        }
        
        // StaticBatchingUtility.Combine(gameObject);
        chunksInRange.Dispose();
    }

    public void UnloadChunksInRange(Transform player)
    {
        var removeKeysList = new List<ChunkData>();
        var center = Hex3.XZToHex3(player.transform.position, MapScale);

        foreach (var drawnChunk in _drawnChunks)
        {
            var chunkData = drawnChunk.Key;

            if (Hex3.Distance(chunkData.Center,center) > (UnloadDistance - 1) * ChunkSize * 2 + ChunkSize)
            {
                removeKeysList.Add(drawnChunk.Key);
            }
        }

        for (int i = 0; i < removeKeysList.Count; i++)
        {
            var chunkObj = _drawnChunks[removeKeysList[i]];
            chunkObj.Pool("ChunkObj");
            chunkObj.gameObject.SetActive(false);
            _drawnChunks.Remove(removeKeysList[i]);
        }
    }
    
    public void UpdateChunks()
    {
        foreach (var chunk in _drawnChunks)
        {
            chunk.Value.MeshFilter.mesh = _worldGen.UpdateChunkUVs(chunk.Value.MeshFilter.mesh,chunk.Key.Center);
        }
    }
}