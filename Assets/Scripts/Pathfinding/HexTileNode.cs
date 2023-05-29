using System;

public struct HexTileNode : IEquatable<HexTileNode>
{
    public Hex3 Hex;
    public Hex3 ParentHex;
    public float GCost;
    public float HCost;
    public NodeState State; 

    public float FCost => GCost + HCost;

    public bool Equals(HexTileNode other)
    {
        return Hex.Equals(other.Hex);
    }

    public override bool Equals(object obj)
    {
        return obj is HexTileNode other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hex);
    }

    public enum NodeState
    {
        Unvisited,
        Open,
        Closed
    }
}