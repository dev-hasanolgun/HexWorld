public struct ChunkData
{
    public Hex3 Center;
    public int Size;

    public ChunkData(Hex3 center, int size)
    {
        Center = center;
        Size = size;
    }
}