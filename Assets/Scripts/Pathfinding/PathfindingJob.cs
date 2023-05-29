using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct PathfindingJob : IJob // TODO Implement flowfield with A* for chunking pathfinding
{
    [ReadOnly] public BiomeGen BiomeGen;
    [ReadOnly] public Hex3 Start;
    [ReadOnly] public Hex3 End;
    public NativeList<Hex3> Path;

    public void Execute()
    {
        FindPath();
    }
    
    public void FindPath()
    {
        var heap = new PriorityQueue<HexTileNode,float>(Allocator.Temp);
        var nodeMap = new NativeHashMap<Hex3,HexTileNode>(1,Allocator.Temp);
        var openNodes = 1;
        var startNode = new HexTileNode
        {
            Hex = Start
        };
        
        heap.Enqueue(startNode,startNode.FCost);
        nodeMap.Add(Start,startNode);

        while (openNodes > 0)
        {
            var currentNode = heap.Dequeue();
            var currentHex = currentNode.Hex;
            currentNode.State = HexTileNode.NodeState.Closed;
            nodeMap[currentHex] = currentNode;
            openNodes--;

            if (currentHex == End)
            {
                RetracePath(nodeMap);
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                var neighbor = Hex3.GetNeighbor(currentHex,i);
                nodeMap.TryGetValue(neighbor, out var neighborNode);
                neighborNode.Hex = neighbor;

                var biome = BiomeGen.GetBiome(neighbor);
                var traversable = BiomeGen.GetTileType(neighbor) == TileType.Island && biome != Biome.River && biome != Biome.FrozenRiver && biome != Biome.Obstacle;
                
                if (!traversable || neighborNode.State == HexTileNode.NodeState.Closed || Hex3.Distance(currentHex, Start) > 50) continue;

                var costToNeighbor = currentNode.GCost + Hex3.Distance(currentNode.Hex, neighbor);

                if (costToNeighbor < neighborNode.GCost || neighborNode.State != HexTileNode.NodeState.Open)
                {
                    neighborNode.GCost = costToNeighbor;
                    neighborNode.HCost = Hex3.Distance(neighbor, End);
                    neighborNode.ParentHex = currentNode.Hex;
                    nodeMap[neighbor] = neighborNode;

                    if (neighborNode.State != HexTileNode.NodeState.Open)
                    {
                        neighborNode.State = HexTileNode.NodeState.Open;
                        nodeMap[neighbor] = neighborNode;
                        heap.Enqueue(neighborNode,neighborNode.FCost);
                        openNodes++;
                    }
                }
            }
        }
    }

    private void RetracePath(NativeHashMap<Hex3,HexTileNode> nodeMap)
    {
        var currentNode = nodeMap[End];

        while (currentNode.Hex != Start)
        {
            Path.Add(currentNode.Hex);
            currentNode = nodeMap[currentNode.ParentHex];
        }
        Path = Path.Reverse();
    }
}