using Unity.Collections;

public static class HexJobExtensions
{
    public static NativeList<Hex3> SpiralRing(Hex3 value, int radius)
    {
        var spiralRing = new NativeList<Hex3>(Allocator.Temp){value};
        
        for (int x = -radius; x <= radius; x++)
        {
            var min = x <= 0 ? -radius - x : -radius;
            var max = x <= 0 ? radius : radius - x;
        
            for (int y = min; y <= max; y++)
            {
                var hex = new Hex3(x, y, 0 - (x + y));
                spiralRing.Add(hex+value);
            }
        }
        
        return spiralRing;
    }
    
    public static NativeList<Hex3> Pie(Hex3 value, int radius, int index)
    {
        var pie = new NativeList<Hex3>(Allocator.Temp);

        for (int i = 0; i < radius; i++)
        {
            var cornerPos = value + Hex3.GetDirection(index) * (i + 1);
            
            for (int k = 0; k < i+1; k++)
            {
                pie.Add(cornerPos);
                if (k == i) break;
                cornerPos += Hex3.GetDirection((index + 2) % 6);
            }
        }
        
        pie.RemoveAtSwapBack(pie.Length-1);

        return pie;
    }
}