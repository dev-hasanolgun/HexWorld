using UnityEngine;

public struct HexTileData
{
    public readonly Hex3 HexPosition;
    public readonly Vector3 WorldPosition;

    public TileType Type;
    public Biome Biome;

    public HexTileData(Hex3 hexPos, Vector3 worldPos, TileType type, Biome biome)
    {
        HexPosition = hexPos;
        WorldPosition = worldPos;
        Type = type;
        Biome = biome;
    }
}